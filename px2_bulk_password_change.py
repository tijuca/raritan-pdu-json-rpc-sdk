#!/usr/bin/python3

import sys, csv, argparse, traceback, time, socket
from raritan.rpc import Agent, usermgmt, HttpException
from raritan import zeroconf

HTTP_AGENT_TIMEOUT_DEFAULT = 5
ZEROCONF_TIMEOUT_DEFAULT = 4
USER_DEFAULT = "admin"
PASSWORD_DEFAULT = "raritan"

class PduPasswordChangeData:
    def __init__(self, ip = None, user = None, pw = None, newPw = None):
        self.__dict__ = {"ip" : ip, "user": user, "pw": pw, "newPw": newPw}

def parse_csv(csv_file):
    print("Reading from CSV file: %s" % csv_file)
    unit_data = []
    with open(csv_file, 'r') as csv_data:
        reader = csv.reader(csv_data, delimiter=',', quotechar='|')
        for row in reader:
            if len(row) != 4:
                print("Row %s not correctly formatted. Required format: IP,USERNAME,PASSWORD,NEW_PASSWORD" % reader.line_num)
                continue
            unit_data.append(PduPasswordChangeData(ip = row[0], user = row[1], pw = row[2], newPw = row[3]))
    return unit_data

def parse_ips(ip_list):
    ret = []
    for ip in ip_list:
        splitted_ips = ip.split("..")
        if len(splitted_ips) > 1:
            import ipaddress
            start_ip = ipaddress.ip_address(splitted_ips[0])
            end_ip = ipaddress.ip_address(splitted_ips[1])
            ret.extend([str(ipaddress.ip_address(i)) for i in range(int(start_ip), int(end_ip) + 1)])
        else:
            ret.append(ip)
    return ret

arg_parser = argparse.ArgumentParser(formatter_class = argparse.ArgumentDefaultsHelpFormatter)
# mutually exclusive args
arg_group = arg_parser.add_argument_group("Specify one of the following")
group = arg_group.add_mutually_exclusive_group(required=True)
group.add_argument("-l","--list", help="change passwords by using a pdu IP list. You can specify mutliple IPs (separated with space) and use IP ranges like 10.0.0.2..10.0.0.4", nargs="+", default=[], metavar='PDU-IP')
group.add_argument("-c","--csv", help="change passwords by using a csv file. Format: \"<PDU_IP>,<PDU_USER>,<PDU_PASSWORD>,<NEW_PASSWORD>\"")
group.add_argument("-z","--zeroconf", help="update by using zeroconf to find PDUs in the subnet", action="store_true")
#optional args
arg_parser.add_argument("-n", "--new-password", dest="new_password", help="the password to set, required with --list and --zeroconf", type=str)
arg_parser.add_argument("-u", "--user", help="the username for authentication (used with --list or --zeroconf)", type=str, default=USER_DEFAULT)
arg_parser.add_argument("-p", "--password", help="the password for authentication (used with --list or --zeroconf)", type=str, default=PASSWORD_DEFAULT)
arg_parser.add_argument("-t", "--timeout", dest="agent_timeout", help="provide the timeout for JSON-RPC requests", type=int, default=HTTP_AGENT_TIMEOUT_DEFAULT)
arg_parser.add_argument("--zeroconf-timeout", dest="zeroconf_timeout", help="provide the timeout for zeroconf lookup", type=int, default=ZEROCONF_TIMEOUT_DEFAULT)

args = arg_parser.parse_args()
if args.new_password is None and (len(args.list) != 0 or args.zeroconf):
    print("Need --new-password when using --list or --zeroconf")
    sys.exit(1)

units = []
if args.csv:
    units = parse_csv(args.csv)
elif args.list:
    units = [PduPasswordChangeData(ip = ip, user = args.user, pw = args.password, newPw = args.new_password) for ip in parse_ips(args.list)]
elif args.zeroconf:
    device_list = zeroconf.discover(args.zeroconf_timeout)
    if (len(device_list) == 0):
        print("No PDUs discovered")
        sys.exit()
    answer = input("Do you want to change the password of these PDUs [y/N]").lower().strip()
    if answer and (answer[0] == "y"):
        deviceIps = [device .get("ip") for device in device_list]
        units = [PduPasswordChangeData(ip = ip, user = args.user, pw = args.password, newPw = args.new_password) for ip in deviceIps]

for unit in units:
    try:
        agent = Agent("https", unit.ip, unit.user, unit.pw, disable_certificate_verification = True, timeout = args.agent_timeout)
        user = usermgmt.User("/auth/user/%s" % unit.user, agent)
        idlRet = user.setAccountPassword(unit.newPw)
        result = ""
        if idlRet == 0:
            result = "Changed password successfully"
        elif idlRet == usermgmt.User.ERR_PASSWORD_UNCHANGED:
            result = "The new password has to differ from old password."
        elif idlRet == usermgmt.User.ERR_PASSWORD_EMPTY:
            result = "The password must not be empty."
        elif idlRet == usermgmt.User.ERR_PASSWORD_TOO_SHORT:
            result = "The password is too short."
        elif idlRet == usermgmt.User.ERR_PASSWORD_TOO_LONG:
            result = "The password is too long."
        elif idlRet == usermgmt.User.ERR_PASSWORD_CTRL_CHARS:
            result = "The password must not contain control characters."
        elif idlRet == usermgmt.User.ERR_PASSWORD_NEED_LOWER:
            result = "The password has to contain at least one lower case character."
        elif idlRet == usermgmt.User.ERR_PASSWORD_NEED_UPPER:
            result = "The password has to contain at least one upper case character."
        elif idlRet == usermgmt.User.ERR_PASSWORD_NEED_NUMERIC:
            result = "The password has to contain at least one numeric character."
        elif idlRet == usermgmt.User.ERR_PASSWORD_NEED_SPECIAL:
            result = "The password has to contain at least one printable special character."
        elif idlRet == usermgmt.User.ERR_PASSWORD_IN_HISTORY:
            result = "The password already is in history."
        elif idlRet == usermgmt.User.ERR_PASSWORD_TOO_SHORT_FOR_SNMP:
            result = "SNMPv3 USM is activated for the user and the password shall be used as auth passphrase. For this case, the password is too short (must be at least 8 characters)"
        else:
            result = "Unknown IDL error"

    except HttpException as httpExc:
        if "HTTP Error 401" in str(httpExc):
            result = "Authentication failed"
        elif "HTTP Error" in str(httpExc):
            result = "HTTP request failed"
        else:
            result = "HTTP connection failed"
    except:
        result = "Unknown error"

    print("Device %s: %s" % (unit.ip, result))
