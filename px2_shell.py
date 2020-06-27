#!/usr/bin/python3
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2012 Raritan Inc. All rights reserved.

import time, datetime, sys, os, atexit, code, getpass
import rlcompleter
try:
    import readline
except:
    # no readline support, error message will be generated when used below
    pass

sys.path.append("pdu-python-api")
import raritan.rpc
from raritan.rpc import Agent, idl
from raritan.rpc import net, firmware, cfg, session, event
from raritan.rpc import pdumodel, tfw, test, sensors, usermgmt

def parse_url(url):
    scheme = 'https'
    username = 'admin'
    password = 'raritan'
    hostname = url

    parts = hostname.split('://')
    if len(parts) >= 2:
        (scheme, hostname) = parts

    parts = hostname.split('@')
    if len(parts) >= 2:
        (username, hostname) = parts

    parts = username.split(':')
    if len(parts) >= 2:
        (username, password) = parts

    return (scheme, hostname, username, password)

try:
    (scheme, hostname, username, password) = parse_url(sys.argv[1])
except:
    print("Usage: px2_shell.py [scheme://][user[:password]@]host")
    sys.exit(1)

try:
    readline.parse_and_bind('tab: complete')
    history = os.path.join(os.environ["HOME"], ".px2_shell_history")
    try:
        readline.read_history_file(history)
    except IOError:
        pass
    atexit.register(readline.write_history_file, history)
except:
    print("Sorry, no readline support!")

agent = Agent(scheme, hostname, username, password, disable_certificate_verification = True)

try:
    pdumodel.Pdu("/model/pdu/0", agent).getMetaData()
except raritan.rpc.HttpException as e:
    if "HTTP Error 451" in str(e):
        print("Changing the default password for user 'admin' is required.")
        admin = usermgmt.User("/auth/user/admin", agent)
        password = getpass.getpass()
        admin.setAccountPassword(password)
        agent.passwd = password

# selected sysrpc proxies
net_proxy = net.Net("/net", agent)
firmware_proxy = firmware.Firmware("/firmware", agent)
cfg_proxy = cfg.Cfg("/cfg", agent)
session_manager = session.SessionManager("/session", agent)
event_engine = event.Engine("/event_engine", agent)
event_service = event.Service("/eventservice", agent)

# selected modelrpc proxies
core = tfw.CoreCtrl("/model/core", agent)
unit = pdumodel.Unit("/model/unit", agent)
pdu = pdumodel.Pdu("/model/pdu/0", agent)
inlets = pdu.getInlets()
ocps = pdu.getOverCurrentProtectors()
outlets = pdu.getOutlets()

def poll_events(types = None):
    if types == None:
        types = [ raritan.rpc.idl.Event ]
    elif not isinstance(types, list):
        types = [ types ]
    channel = event_service.createChannel()
    channel.demandEventTypes(types)
    print("Polling for events, Ctrl-C to stop ...")
    try:
        while True:
            _, events = channel.pollEvents()
            ts = datetime.datetime.now().strftime("[%H:%M:%S]")
            for event in events:
                print("%s %s" % (ts, event))
    except KeyboardInterrupt:
        print()

code.interact(local=globals())
