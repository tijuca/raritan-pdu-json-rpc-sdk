#!/usr/bin/python3
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2019 Raritan Inc. All rights reserved.

import sys, csv, logging, argparse, time, threading, codecs
from enum import Enum
try:
    from queue import Queue
except ImportError:
    from Queue import Queue

sys.path.append("pdu-python-api")
from raritan.rpc import Agent, HttpException, JsonRpcErrorException, firmware

# timeout for zeroconf discovery
ZEROCONF_TIMEOUT_DEFAULT = 4
# seconds to wait for a valid response during device update
UPDATE_TIMEOUT_DEFAULT = 90
# seconds to wait for a response during availability check
AGENT_AVAILABILITY_TIMEOUT_DEFAULT = 120
# seconds to wait for a response during device update
AGENT_FWUPDATE_TIMEOUT_DEFAULT = 4
# seconds to wait for queued devices
AVAILABILITY_TIMEOUT_DEFAULT = 90
# the default number of threads used to process the updates
NUM_THREADS_DEFAULT = 25
# default user
USER_DEFAULT = "admin"
# default password
PASSWORD_DEFAULT = "raritan"
# default protocol
PROTOCOL_DEFAULT = "https"
# cascading master id
CASCADING_MASTER = 1

logger = logging.getLogger(__name__)
arg_parser = argparse.ArgumentParser(formatter_class = argparse.ArgumentDefaultsHelpFormatter)

class BulkUpdateStatus(Enum):
    SUCCESS = 0
    SKIPPED = 1
    FAILED = 2
    FAILED_NORESPONSE = 3
    PREPARED = 4
    READY = 5
    UPDATING = 6

    def __str__(self):
        return self.name
class PduConnectionData: # pylint: disable=too-few-public-methods
    def __init__(self, ip = None, user = None, pw = None): # pylint: disable=redefined-outer-name
        self.__dict__ = {"ip" : ip, "user": user, "pw": pw}

class CascadeUnit: # pylint: disable=too-many-instance-attributes
    def __init__(self, con_data, link=CASCADING_MASTER, master=None):
        self.con_data = con_data
        self.agent = Agent(PROTOCOL_DEFAULT, con_data.ip, con_data.user, con_data.pw, disable_certificate_verification=True, timeout=AGENT_AVAILABILITY_TIMEOUT_DEFAULT)
        self.links = []
        self.link_id = link
        self.master = master
        self.image_version = ""
        self.last_update = 0
        if link is CASCADING_MASTER:
            self.fw_proxy = firmware.Firmware("/firmware", self.agent)
        else:
            self.fw_proxy = firmware.Firmware("/link/%i/firmware" % link, self.agent)
        try:
            logger.info("Availability check for %s link %i", self.agent.url, self.link_id)
            self.old_version = self.fw_proxy.getVersion()
            if link is CASCADING_MASTER: self._create_links()
            self.state = BulkUpdateStatus.READY
            logger.debug("Cascade Unit created for %s link %i. Firmware version: %s", self.agent.url, self.link_id, self.old_version)
        except (HttpException, JsonRpcErrorException):
            logger.info("%s link %i device connection failed", self.agent.url, self.link_id)
            self.state = BulkUpdateStatus.FAILED_NORESPONSE

    def _create_links(self):
        try:
            from raritan.rpc import cascading
            cascade_status = cascading.CascadeManager("/cascade", self.agent).getStatus()
            for link_id in list(cascade_status.linkUnits.keys()):
                self.links.append(CascadeUnit(self.con_data, link_id, master=self))
        except (ImportError, HttpException, JsonRpcErrorException):
            logger.debug("Not a cascadable device.")

    def is_image_present_and_valid(self):
        image_present, image_info = self.fw_proxy.getImageInfo()
        self.image_version = image_info.version
        logger.debug(image_info)
        logger.info("%s link %i image valid? %s", self.agent.url, self.link_id, str(image_info.valid))
        return image_present and image_info.valid

    def start_fw_update(self, update_args):
        self.state = BulkUpdateStatus.UPDATING
        self.agent.timeout = AGENT_FWUPDATE_TIMEOUT_DEFAULT
        self.fw_proxy.startUpdate(update_args)
        logger.info("Update started for: %s link: %i", self.agent.url, self.link_id)

    def get_fw_update_status(self):
        targetUrl = "/cgi-bin/fwupdate_progress.cgi"
        if self.link_id > CASCADING_MASTER:
            targetUrl = "/link/%i%s" % (self.link_id, targetUrl)
        try:
            resp = firmware.FirmwareUpdateStatus(targetUrl, self.agent).getStatus()
            # after reboot we receive NONE. check fw update history instead
            if resp.state == "NONE":
                hist = self.fw_proxy.getUpdateHistory()
                if len(hist) > 0:
                    cmp_versions = compare_versions(hist[-1].imageVersion, self.image_version)
                    success = hist[-1].status == firmware.UpdateHistoryStatus.SUCCESSFUL and cmp_versions == 0
                else:
                    success = False
                resp.state = "SUCCESS" if success else "FAIL"
        except (HttpException, JsonRpcErrorException) as e:
            logger.debug("%s on %s",str(e).split("\n")[0], targetUrl)
            resp = firmware.UpdateStatus("WAITING", -1, -1, "")
        logger.debug("%s link %i: %s", self.agent.url, self.link_id, str(resp))
        # set status to an final state
        if resp.state in ["SUCCESS","FAIL"]:
            self.state = BulkUpdateStatus.SUCCESS if resp.state == "SUCCESS" else BulkUpdateStatus.FAILED
        return resp

class BulkUpdater: # pylint: disable=too-many-instance-attributes
    def __init__(self, arguments, con_data = None):
        self.connection_data_list = con_data # the list of the connection data for the master
        self.parsed_fw_file = None # the content of the firmware file
        self.fw_file_version = "" # the version of the firmware file
        self.connection_data_queue = None # the queue for the connection data
        self.device_queue = None # the queue for the master devices
        self.wait_for_status_queue = None # the queue for master and link devices which are in update state
        self.done_list = [] # the list of finished devices
        self.arguments = arguments # the arguments from cli/gui
        self.stop_event = None # event which is stopping threads and act as join handler

    def start(self):
        if not self._prepare():
            logger.error("Preparation failed")
            sys.exit(1)
        fw_file = self.arguments.fw_file[0]
        logger.debug("fw binary file %s", fw_file)
        logger.debug("preparation success. %i device(s) found", self.device_queue.qsize())
        # python2 qt encodes default with ascii this will cause errors inside urllib2
        if sys.version_info < (3, 0):
            reload(sys) # pylint: disable=undefined-variable
            sys.setdefaultencoding("latin-1") # pylint: disable=no-member
        self.parsed_fw_file = open(fw_file, "rb").read()
        version_string = self.parsed_fw_file[31:37].decode()
        build_nr = int(codecs.encode(self.parsed_fw_file[43:47], 'hex'), 16)
        devel_phase = int(codecs.encode(self.parsed_fw_file[177:178], 'hex'), 16)
        version_parts = [int(version_string[i:i+2]) for i in range(0, len(version_string), 2)]
        version_parts.append(devel_phase)
        self.fw_file_version = ".".join(str(i) for i in version_parts) + "-%i" % build_nr
        # start threading
        self.stop_event = threading.Event()
        threads = []
        logger.info("Reading IP list")
        for i in range(self.arguments.threads):
            t = threading.Thread(target=self._creation_worker)
            t.setDaemon(True)
            t.start()
            threads.append(t)

        #block until tasks done
        self.connection_data_queue.join()

        logger.debug("define creation thread stops")
        for i in range(self.arguments.threads):
            self.connection_data_queue.put(None)

        logger.debug("create update threads")
        for i in range(self.arguments.threads):
            t = threading.Thread(target=self._update_worker)
            t.setDaemon(True)
            t.start()
            threads.append(t)

        #status worker
        logger.debug("create status threads")
        t = threading.Thread(target=self._status_worker)
        t.setDaemon(True)
        t.start()
        threads.append(t)

        #block until tasks done
        self.device_queue.join()

        logger.debug("define update thread stops")
        for i in range(self.arguments.threads):
            self.device_queue.put(None)

        logger.debug("all update threads signaled to stop")
        self.wait_for_status_queue.put(None)
        self.wait_for_status_queue.join()

        #join threads
        for t in threads:
            t.join()

    def _prepare(self):
        if not self.connection_data_list:
            return False

        logger.debug("Start preparation")
        del self.done_list[:]
        self.wait_for_status_queue = Queue()
        self.connection_data_queue = Queue()
        self.device_queue = Queue()
        [self.connection_data_queue.put(con_data) for con_data in self.connection_data_list]
        return True

    def _creation_worker(self):
        while not self.stop_event.is_set():
            con_data = self.connection_data_queue.get()
            if con_data is None or self.stop_event.is_set():
                logger.debug("Stopping creation worker...")
                self.connection_data_queue.task_done()
                break
            logger.info("Configuring ip: %s %s %s", con_data.ip, con_data.user, con_data.pw)
            unit = CascadeUnit(con_data)
            self.device_queue.put(unit)
            self.connection_data_queue.task_done()

    def _update_worker(self): # pylint: disable=too-many-branches
        logger.debug("Remaining devices: %i", self.device_queue.qsize())
        while not self.stop_event.is_set():
            master = self.device_queue.get()
            if master is None or self.stop_event.is_set():
                logger.debug("Stopping update worker...")
                self.device_queue.task_done()
                break
            # check if the update is necessary
            if self._check_for_update(master):
                # one or more devices need an update
                # upload binary
                logger.info("Upload binary to %s", master.agent.url)
                firmware.upload(master.agent, self.parsed_fw_file)
                # wait a bit to prevent JsonRpcErrors on devices with more relayboards
                time.sleep(5)
                # list for flattening the structure
                uploaded = []
                # error counter
                error_count = 0
                # first the links
                for link in reversed(master.links):
                    if BulkUpdater._check_image_present(link):
                        uploaded.append(link)
                    else:
                        error_count += 1
                        self.done_list.append(link)
                # the last one is the master
                if BulkUpdater._check_image_present(master):
                    uploaded.append(master)
                else:
                    error_count += 1
                    self.done_list.append(master)
                # no further processing on this device. set all to skipped
                if error_count > 0:
                    for device in uploaded:
                        device.state = BulkUpdateStatus.SKIPPED
                        self.done_list.append(device)
                    uploaded = []
                # iterate over all devices to trigger update
                for device in uploaded:
                    device.start_fw_update([firmware.UpdateFlags.ALLOW_UNTRUSTED] if self.arguments.allow_untrusted else []) # pylint: disable=no-member
                    # put them in the queue for status watching
                    self.wait_for_status_queue.put(device)
            else:
                # no device need an update
                self.done_list.append(master)
                for link in master.links:
                    self.done_list.append(link)
            self.device_queue.task_done()

    def _status_worker(self):
        while not self.stop_event.is_set():
            device = self.wait_for_status_queue.get()
            if device == None:
                if self.wait_for_status_queue.empty() or self.stop_event.is_set():
                    logger.debug("Stopping status worker...")
                    self.wait_for_status_queue.task_done()
                    break
                self.wait_for_status_queue.task_done()
                self.wait_for_status_queue.put(device)
                continue
            status = device.get_fw_update_status().state
            have_master = device.master != None

            if status in ["SUCCESS", "FAIL"]:
                self.wait_for_status_queue.task_done()
                self.done_list.append(device)
                continue
            elif not status == "WAITING": # update timestamp while not in waiting state
                logger.debug("%s link %i update timestamp", device.agent.url, device.link_id)
                device.last_update = time.time()
            elif status == "WAITING" and have_master and device.master.state == BulkUpdateStatus.UPDATING:
                # master itself updates, no connection to the linked units
                logger.debug("%s link %i master not reachable. update timestamp, master state %s", device.agent.url, device.link_id, str(device.master.state))
                device.last_update = time.time()
            elif status == "WAITING" and (time.time() - device.last_update) > UPDATE_TIMEOUT_DEFAULT: # check for timeout
                logger.info("%s link %i Timeout", device.agent.url, device.link_id)
                device.state = BulkUpdateStatus.FAILED_NORESPONSE
                self.wait_for_status_queue.task_done()
                self.done_list.append(device)
                continue

            self.wait_for_status_queue.task_done()
            self.wait_for_status_queue.put(device)
            time.sleep(1) # throttle

        logger.debug("No devices left to update")
        # availability
        self._check_availability()
        # summary
        self._print_summary()

    def _should_update_device(self, device):
        try:
            version = device.old_version
            should_update = self.should_update(version, self.fw_file_version)
            if not should_update:
                device.state = BulkUpdateStatus.SKIPPED
                logger.info("%s link %i: SKIPPED. Installed Firmware has higher or same version.", device.agent.url, device.link_id)
            return should_update
        except (HttpException, JsonRpcErrorException) as e:
            device.state = BulkUpdateStatus.FAILED_NORESPONSE
            logger.info("%s: FAILED_NORESPONSE: %s", device.url, str(e).split("\n")[0])
            return False

    def _check_for_update(self, master):
        if master.state == BulkUpdateStatus.FAILED_NORESPONSE:
            return False
        should_update = self._should_update_device(master)
        for device in master.links:
            if device.state == BulkUpdateStatus.FAILED_NORESPONSE:
                return False
            should_update |= self._should_update_device(device)
        return should_update

    @staticmethod
    def _check_image_present(device):
        agent = device.agent
        link_id = device.link_id
        try:
            image_present = device.is_image_present_and_valid()
            logger.debug("is_image_present_and_valid %s", str(image_present))
            if image_present:
                logger.info("%s link %i: SUCCESS", agent.url, link_id)
                device.state = BulkUpdateStatus.PREPARED
                device.last_update = 0
                return True
            # upload failed
            device.state = BulkUpdateStatus.FAILED
            logger.info("%s link %i: FAILED. No image present.", agent.url, link_id)
            return False
        except (HttpException, JsonRpcErrorException) as e:
            device.state = BulkUpdateStatus.FAILED_NORESPONSE
            logger.info("%s link %i: FAILED_NORESPONSE: %s", agent.url, link_id, str(e).split("\n")[0])
            return False

    def should_update(self, current, image_version):
        cmp_versions = compare_versions(current, image_version)
        if cmp_versions == 0: # same version
            logger.debug("Update to same version: %s", str(self.arguments.allow_same_version))
            return self.arguments.allow_same_version
        if cmp_versions == 1: # downgrade
            logger.debug("Downgrade: %s", str(self.arguments.allow_downgrade))
            return self.arguments.allow_downgrade
        # image version is higher than currently installed
        return True

    def _check_availability(self):
        final_states = [BulkUpdateStatus.SKIPPED, BulkUpdateStatus.FAILED_NORESPONSE, BulkUpdateStatus.FAILED]
        if all(pdu.state in final_states for pdu in self.done_list):
            return
        logger.info("Check device availability after %i seconds", AVAILABILITY_TIMEOUT_DEFAULT)
        time.sleep(AVAILABILITY_TIMEOUT_DEFAULT)
        for pdu in self.done_list:
            if "fw_proxy" in pdu.__dict__:
                try:
                    pdu.fw_proxy.getVersion()
                except (HttpException, JsonRpcErrorException):
                    agent = pdu.agent
                    link_id = pdu.link_id
                    logger.debug("%s link %i: FAILED_NORESPONSE: Version check failed", agent.url, link_id)
                    pdu.state = BulkUpdateStatus.FAILED_NORESPONSE

    def _print_summary(self):
        out = "Summary:\n"
        for pdu in self.done_list:
            if pdu.state is not BulkUpdateStatus.FAILED_NORESPONSE:
                out += "\t%s link %i: %s: update from %s to %s\n" % (pdu.agent.url, pdu.link_id, str(pdu.state), pdu.old_version, self.fw_file_version)
            else:
                out += "\t%s link %i: %s: device not ready\n" % (pdu.agent.url, pdu.link_id, str(pdu.state))
        logger.info(out)
        logger.info("--- Finished ---")

def compare_versions(version_one, version_two):
    splitted = version_one.split('.')
    v1 = [ int(part) for part in splitted[:3] ]
    v1.extend([ int(i) for i in splitted[3].split('-') ])
    splitted = version_two.split('.')
    v2 = [ int(part) for part in splitted[:3] ]
    v2.extend([ int(i) for i in splitted[3].split('-') ])
    # python3 cmp expression https://docs.python.org/3.0/whatsnew/3.0.html#ordering-comparisons
    # return 0 if equal, 1 if version_one is bigger and -1 if version_two is bigger
    return (v1 > v2) - (v1 < v2)

def setup_logger(arguments):
    level = logging.DEBUG if arguments.debug else logging.INFO
    logger.setLevel(level)
    logging.basicConfig(format='[%(asctime)s] %(levelname)-8s %(message)s', datefmt='%d %b %Y %H:%M:%S')

def setup_arguments():
    #required argument
    arg_group = arg_parser.add_argument_group("Specify one of the following")
    group = arg_group.add_mutually_exclusive_group(required=True)
    group.add_argument("-f", "--fw-file", dest="fw_file", help="the firmware binary file to use", nargs=1, type=str)
    group.add_argument("-g", "--gui", help="use a graphical interface instead of CLI", action="store_true")
    # define mutually exclusive args
    arg_group = arg_parser.add_argument_group("Specify one of the following if running without GUI")
    group = arg_group.add_mutually_exclusive_group()
    group.add_argument("-c","--csv", help="update by using a csv file. Format: \"<PDU-IP>,<PDU_USER>,<PDU-PASSWORD>\"")
    group.add_argument("-l","--list", help="update by using a pdu IP list", nargs="+", default=[], metavar='PDU-IP')
    group.add_argument("-z","--zeroconf", help="update by using zeroconf to find PDUs in the subnet", action="store_true")
    #optional flags
    arg_parser.add_argument("-d", "--debug", help="enable debug output", action="store_true")
    arg_parser.add_argument("--allow-untrusted", dest="allow_untrusted", help="skip image certificate check", action="store_true")
    #optional args
    arg_parser.add_argument("--allow-downgrade", dest="allow_downgrade", help="allow to downgrade the firmware", action="store_true")
    arg_parser.add_argument("--allow-same-version", dest="allow_same_version", help="allow to update the firmware to the same version", action="store_true")
    arg_parser.add_argument("-t", "--threads", help="the number of threads used for updateing devices", type=int, default=NUM_THREADS_DEFAULT)
    arg_parser.add_argument("-u", "--user", help="the username for authentication (only used if --list or --zeroconf is specified)", type=str, default=USER_DEFAULT)
    arg_parser.add_argument("-p", "--password", help="the password for authentication (only used if --list is specified)", type=str, default=PASSWORD_DEFAULT)
    arg_parser.add_argument("--zeroconf-timeout", dest="zeroconf_timeout", help="provide the timeout for zeroconf lookup", type=int, default=ZEROCONF_TIMEOUT_DEFAULT)
    return arg_parser.parse_args()


def parse_csv(csv_file):
    logger.info("Reading from CSV file: %s", csv_file)
    con_data = []
    with open(csv_file, 'r') as csv_data:
        reader = csv.reader(csv_data, delimiter=',', quotechar='|')
        for row in reader:
            if len(row) != 3:
                logger.warning("Row %s not correctly formatted. Required format: IP,USERNAME,PASSWORD", reader.line_num)
                continue
            con_data.append(PduConnectionData(ip = row[0], user = row[1], pw = row[2]))
    return con_data

def do_zeroconf_discover(timeout):
    try:
        from raritan import zeroconf
        devices = zeroconf.discover(timeout=timeout)
        pdus = []
        for device in devices:
            pdus.append(device.get("ip"))
        answer = input("Do you want to update these PDUs [y/N]").lower().strip()
        if answer and (answer[0] == "y" or answer[0] == "j"):
            return pdus
        sys.exit()
    except ImportError:
        sys.exit(1)

def parse_ips(ip_list):
    ret = []
    for ip in ip_list:
        splitted_ips = ip.split("..")
        if len(splitted_ips) > 1:
            import ipaddress
            start_ip = ipaddress.ip_address(splitted_ips[0])
            end_ip = ipaddress.ip_address(splitted_ips[1])
            logger.debug("Parse ip range from %s to %s", str(start_ip), str(end_ip))
            ret.extend([str(ipaddress.ip_address(i)) for i in range(int(start_ip), int(end_ip) + 1)])
        else:
            ret.append(ip)
    return ret

def do_all_in_gui(arguments): # pylint: disable=too-many-locals, too-many-statements
    from PyQt5.QtWidgets import QApplication, QWidget, QPushButton, QLineEdit, QVBoxLayout, QGroupBox, QRadioButton, QCheckBox, QLabel, QFormLayout, QSpinBox
    # main layout
    app = QApplication([])
    app.setStyle('Fusion') # QStyleFactory.keys()
    window = QWidget()
    parent_layout = QVBoxLayout(window)
    file_name_input = create_fileselector(parent_layout, "Firmware File...", "Select firmware file", "bin(*.bin)")
    ## radio group
    ip_group_box = QGroupBox("IP source", window)
    ip_group_layout = QVBoxLayout(ip_group_box)
    csv_radio = QRadioButton("CSV File")
    zeroconf_radio = QRadioButton("Zeroconf")
    list_radio = QRadioButton("IP list (comma separated)")
    list_line_edit = QLineEdit()
    ip_group_layout.addWidget(csv_radio)
    csv_file_name_input = create_fileselector(ip_group_layout, "CSV File...", "Select CSV file", "csv(*.csv)", radio_widget=csv_radio)
    ip_group_layout.addWidget(list_radio)
    ip_group_layout.addWidget(list_line_edit)
    ip_group_layout.addWidget(zeroconf_radio)
    parent_layout.addWidget(ip_group_box)
    ## additional options
    options_group_box = QGroupBox("Options")
    options_layout = QFormLayout(options_group_box)
    allow_untrusted_cb = QCheckBox("Allow untrusted")
    allow_downgrade_cb = QCheckBox("Allow downgrade")
    allow_same_cb = QCheckBox("Allow same version")
    thread_spinner = QSpinBox()
    thread_spinner.setMinimum(1)
    thread_spinner.setValue(arguments.threads)
    user_input = QLineEdit(arguments.user)
    pw_input = QLineEdit(arguments.password)
    options_layout.addRow(QLabel("User:"), user_input)
    options_layout.addRow(QLabel("PW:"), pw_input)
    options_layout.addRow(QLabel("Threads:"), thread_spinner)
    options_layout.addRow(allow_untrusted_cb)
    options_layout.addRow(allow_downgrade_cb)
    options_layout.addRow(allow_same_cb)
    parent_layout.addWidget(options_group_box)
    ## start fw button
    start_button = QPushButton("Start Firmware Update")
    parent_layout.addWidget(start_button)
    ## console
    create_console(parent_layout)
    window.setWindowTitle("PX Bulk Update")
    # listeners
    ## ip list text toggle
    list_radio.toggled.connect(lambda:list_line_edit.setDisabled(not list_radio.isChecked()))
    ## start button -- parsing the whole stuff from gui to args and build ip list
    def on_start():
        args.gui = False
        args.allow_untrusted = allow_untrusted_cb.isChecked()
        args.allow_downgrade = allow_downgrade_cb.isChecked()
        args.allow_same_version = allow_same_cb.isChecked()
        args.threads = thread_spinner.value()
        args.user = str(user_input.text())
        args.password = str(pw_input.text())
        args.fw_file = [str(file_name_input.text())]
        if csv_radio.isChecked():
            args.csv = csv_file_name_input.text()
        elif zeroconf_radio.isChecked():
            args.zeroconf = True
        elif list_radio.isChecked():
            args.list = list_line_edit.text().split(",")
        start_button.setDisabled(True)
        #putting into separate thread, because this is blocking the gui thread
        t = threading.Thread(target=main, args=(args,), kwargs={"finished_cb":lambda:start_button.setDisabled(False)})
        t.start()
    start_button.clicked.connect(on_start)
    # execute app
    csv_radio.setChecked(True)
    list_line_edit.setDisabled(True)
    window.resize(700, 800)
    window.show()
    sys.exit(app.exec_())

def create_fileselector(parent_layout, btn_text, dlg_title, file_type, radio_widget=None):
    from PyQt5.QtWidgets import QHBoxLayout, QPushButton, QLineEdit, QFileDialog
    file_selector_layout = QHBoxLayout()
    file_button = QPushButton(btn_text)
    file_name_input = QLineEdit() # https://www.tutorialspoint.com/pyqt/pyqt_qlineedit_widget.htm
    file_name_input.setReadOnly(True)
    file_selector_layout.addWidget(file_button)
    file_selector_layout.addWidget(file_name_input)
    parent_layout.addLayout(file_selector_layout)
    ## fw file selector
    def on_file_select():
        dlg = QFileDialog() # https://www.tutorialspoint.com/pyqt/pyqt_qfiledialog_widget.htm
        fname = dlg.getOpenFileName(None, dlg_title, "", file_type)
        if fname[0]:
            file_name_input.setText(fname[0])
    file_button.clicked.connect(on_file_select)
    if radio_widget:
        radio_widget.toggled.connect(lambda:file_button.setDisabled(not radio_widget.isChecked()))
    return file_name_input

def create_console(parent_layout):
    from PyQt5.QtWidgets import QVBoxLayout, QTextEdit, QGroupBox
    from PyQt5.QtCore import QObject, pyqtSignal
    console_group_box = QGroupBox("Console")
    console_layout = QVBoxLayout(console_group_box)
    console_text = QTextEdit()
    console_text.setReadOnly(True)
    console_layout.addWidget(console_text)
    parent_layout.addWidget(console_group_box)
    class ConsoleHandler(QObject, logging.StreamHandler):
        new_record = pyqtSignal(object)
        def emit(self, record):
            msg = self.format(record)
            self.new_record.emit(msg)
    handler = ConsoleHandler()
    def print_msg(msg):
        console_text.append(msg)
    handler.new_record.connect(print_msg)
    formatter = logging.Formatter("[%(asctime)s] %(levelname)-12s %(message)s", datefmt='%d %b %Y %H:%M:%S')
    handler.setFormatter(formatter)
    logger.addHandler(handler)

def noop(): pass

def main(arguments, finished_cb=noop):
    if arguments.gui:
        try:
            do_all_in_gui(arguments)
        except ImportError as e:
            logger.debug(e)
            logger.error("Please install PyQt5 to use the GUI")
    else:
        if arguments.csv:
            logger.debug("CSV")
            con_data_list = parse_csv(arguments.csv)
        elif arguments.list:
            logger.debug("IP list")
            con_data_list = [PduConnectionData(ip = ip, user = arguments.user, pw = arguments.password) for ip in parse_ips(arguments.list)]
        elif arguments.zeroconf:
            logger.debug("zeroconf")
            ip_list = do_zeroconf_discover(arguments.zeroconf_timeout)
            con_data_list = [PduConnectionData(ip = ip, user = arguments.user, pw = arguments.password) for ip in ip_list]
        else:
            arg_parser.print_help()
            sys.exit(1)

        bulk_updater = BulkUpdater(arguments, con_data = con_data_list)
        try:
            bulk_updater.start()
        except KeyboardInterrupt:
            logger.info("KeyboardInterrupt. Stopping.")
            if bulk_updater.stop_event:
                bulk_updater.stop_event.set()
        finally:
            finished_cb()

if __name__ == "__main__":
    args = setup_arguments()
    setup_logger(args)
    main(args)
