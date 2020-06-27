#!/usr/bin/python3
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2019 Raritan Inc. All rights reserved.

from http.server import HTTPServer, BaseHTTPRequestHandler
import argparse, json, os
from netifaces import interfaces, ifaddresses, AF_INET
import os.path

def parse_cmdline():
    DESCR = """Starts a server which handles Xerus data push sensor log messages."""
    USAGE = "Usage: %prog [options]"
    script_name = os.path.splitext(os.path.basename(__file__))[0]
    default_path = "/tmp"
    default_output_file_path = "{0}/{1}.csv".format(default_path, script_name)

    parser = argparse.ArgumentParser(usage = USAGE, description = DESCR)

    h = "General options"
    grp = parser.add_argument_group(parser, h)
    grp.add_argument("-o", "--output", dest = "sensor_output_file_arg", default = default_output_file_path, help = "write received sensor logs to this file")
    grp.add_argument("-p", "--port", type=int, dest = "port_arg", default = 5080, help = "Port to connect to")

    return parser.parse_args()

class SensorPushHandler(BaseHTTPRequestHandler):
    SEPERATOR = ";"
    TIMESTAMP_ATTR = 'timestamp'
    LABEL_ATTR = 'label'
    ID_ATTR = 'id'
    STATE_ATTR = 'state'
    AVG_VALUE_ATTR = 'avgValue'

    def generate_csv_line(self, data_list):
        str_list = [ str(string) for string in data_list ]
        return self.SEPERATOR.join(str_list)

    def write_csv(self, fname, json_data):
        sensors = json_data['sensors']
        rows = json_data['rows']

        if len(sensors) > 0 and len(rows) > 0:
            titles = [ self.TIMESTAMP_ATTR ]
            for index, sensor in enumerate(sensors):
                label = sensor['device'][self.LABEL_ATTR].replace(" ", "-")
                sensor_id = sensor[self.ID_ATTR]
                sensor_title = "{0}_{1}_{2}".format(index, sensor_id, label)
                state_val_title = "{0}.{1}".format(sensor_title, self.STATE_ATTR)
                avg_val_title = "{0}.{1}".format(sensor_title, self.AVG_VALUE_ATTR)

                sensor_titles = [ state_val_title, avg_val_title ]
                titles.extend(sensor_titles)

            file_aready_exists = os.path.isfile(fname)
            with open(fname, mode="a+", encoding = "utf-8") as fd:
                titles_str = self.generate_csv_line(titles)

                if not file_aready_exists:
                    fd.write(titles_str)
                else:
                    # Move to the beginning of the file
                    fd.seek(0)
                    lines = fd.readlines()
                    last_line = lines[-1].split(self.SEPERATOR)
                    # Move to the end of the file
                    fd.seek(0, os.SEEK_END)

                    if len(titles) != len(last_line):
                        fd.write("\n\n" + titles_str)

                for index, row in enumerate(rows):
                    timestamp = row[self.TIMESTAMP_ATTR]

                    sensor_data = [ timestamp ]
                    for record in row['records']:
                        state_value = record[self.STATE_ATTR]
                        avg_value = record[self.AVG_VALUE_ATTR]

                        record_data = [ state_value, avg_value ]
                        sensor_data.extend(record_data)

                    sensor_data_str = self.generate_csv_line(sensor_data)
                    fd.write("\n" + sensor_data_str)
        else:
            print("Sensor data was empty.")

    def do_POST(self):
        content_length = int(self.headers['Content-Length'])
        body = self.rfile.read(content_length)
        self.send_response(200)
        self.end_headers()

        data_str = body.decode("utf-8")
        data = json.loads(data_str)
        self.write_csv(sensor_output_file, data)

class SensorLogListenerServer():
    httpd = None

    def __init__(self, port):
        # listen to everything not only localhost
        self.address = ("", port)
        self.start()

    def __del__(self):
        self.stop()

    def stop(self):
        if self.httpd != None:
            print("Shutting down server.")
            self.httpd.shutdown()
            self.httpd.server_close()

    def start(self):
        try:
            self.httpd = HTTPServer(self.address, SensorPushHandler)
            self.httpd.serve_forever()
        except KeyboardInterrupt:
            exit(1)

def ip4_addresses():
    ip_list = []
    for interface in interfaces():
        addrs = ifaddresses(interface)
        if AF_INET in addrs:
            for link in addrs[AF_INET]:
                if 'addr' in link:
                    ip_addr = link['addr']
                    ip_addr_str = "http://{0}:{1}".format(ip_addr, args.port_arg)
                    ip_list.append(ip_addr_str)
    return ip_list

args = parse_cmdline()

sensor_output_file = os.path.expanduser(args.sensor_output_file_arg)
ip_addresses_str = ", ".join(ip4_addresses())

print("Starting server and listen on the following addresses: {0}".format(ip_addresses_str))
print("Writing received data to '{0}'".format(sensor_output_file))

server = SensorLogListenerServer(args.port_arg)
server.start()
