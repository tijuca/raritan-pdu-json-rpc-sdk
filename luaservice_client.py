#!/usr/bin/python
# -*- coding: utf-8 -*-
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2015 Raritan Inc. All rights reserved.

# To show help use the '-h' option: luaservice_client -h

import sys, os, time, signal
# If your pdu-python-api has a different location then update 'syst.path.append(..)'.
sys.path.append('pdu-python-api')
import argparse
from raritan.rpc import luaservice
from raritan.rpc import Agent

if sys.version_info[:2] >= (3, 0):
    # python 3
    get_input = input
else:
    # python 2
    get_input = raw_input

# ------------------------------------------------------------------------------
# data types
# ------------------------------------------------------------------------------

err_dict = {
    luaservice.Manager.NO_ERROR: "no error",
    luaservice.Manager.ERR_INVALID_NAME: "script name is not valid",
    luaservice.Manager.ERR_NO_SUCH_SCRIPT: "script name not found",
    luaservice.Manager.ERR_MAX_SCRIPT_NUMBERS_EXCEEDED: "maximum amount of stored script files is reached",
    luaservice.Manager.ERR_MAX_SCRIPT_SIZE_EXCEEDED: "maximum size of a script file is reached",
    luaservice.Manager.ERR_MAX_ALL_SCRIPT_SIZE_EXCEEDED: "maximum size of all script files is reached",
    luaservice.Manager.ERR_NOT_TERMINATED: "script is not terminated",
    luaservice.Manager.ERR_NOT_RUNNING: "script is not running",
    luaservice.Manager.ERR_INVALID_ADDR: "address parameter is wrong",
    luaservice.Manager.ERR_TOO_MANY_ARGUMENTS: "too many arguments",
    luaservice.Manager.ERR_ARGUMENT_NOT_VALID: "the argument has one or more invalid character(s)"
}

exec_state_dict = {
    luaservice.ScriptState.ExecState.STAT_NEW: "new",
    luaservice.ScriptState.ExecState.STAT_RUNNING: "is running",
    luaservice.ScriptState.ExecState.STAT_TERMINATED: "has terminated",
    luaservice.ScriptState.ExecState.STAT_RESTARTING: "is restarting"
}

# ------------------------------------------------------------------------------
# functions
# ------------------------------------------------------------------------------

# errors to string
def err_to_str(err):
    return err_dict.get(err, "unknown failure: " + str(err))

def state_to_str(st):
    return exec_state_dict.get(st, "unknown state")

# returns None or pduMetaData
def choose_pdu(lst):
    print("list of pdus:")
    for idx, pdu in enumerate(lst):
        print("  " + str(idx) + ": " + pdu.ip)
    print("  q: quit")
    nbr = get_input("choose one pdu and press enter: ")
    if nbr == 'q':
        do_exit()
    try:
        ret = lst[int(nbr)]
    except:
        print("ERROR: can't parse selection")
        ret = None
    return ret

# list environment for pdu
def list_env(mgr):
    env = mgr.getEnvironment()
    print("  maximum virtual memory per script: " +
            "{:0.3f}".format(env.maxScriptMemoryGrowth/ 1024.0**2) + " MiB")
    print("  number of scripts that can be stored: " + str(env.maxAmountOfScripts))
    print("  number of scripts that are stored: " + str(env.amountOfScripts))
    print("  maximum size of a script file: " + "{:0.3f}".format(env.maxScriptSize / 1024.0) + " kiB")
    print("  maximum size of all script files: " + "{:0.3f}".format(env.maxAllScriptSize / 1024.0) + " kiB")
    print("  size of all script files: " + "{:0.3f}".format(env.allScriptSize / 1024.0) + " kiB")
    print("  output buffer size per script: " + "{:0.3f}".format(env.outputBufferSize / 1024.0) + " kiB")
    print("  minimum delay to next (re)start: " + "{:0.3f}".format(env.restartInterval / 1000.0) + " s")
    print("  minimum delay to 'autoStart' a script: " + "{:0.3f}".format(env.autoStartDelay / 1000.0) + " s")

# exits the app
def do_exit():
    print("app will close")
    sys.exit()

# gets and prints the state for a script
def print_scp_state(nm):
    ret, state = mgr.getScriptState(nm)
    if (ret == mgr.NO_ERROR):
        print("  " + nm + ": " + state_to_str(state.execState))
        if state.execState != luaservice.ScriptState.ExecState.STAT_NEW:
            if state.exitType == luaservice.ScriptState.ExitType.EXIT_CODE:
                print("    last exit code: " + str(state.exitStatus))
            else:
                print("    terminated by signal: " + str(state.exitStatus))
    else:
        print("  get script State returns an error: " + err_to_str(ret))
    return ret

# for all in list lst execute function func
def for_all_do(lst, func):
    if len(lst) == 0:
        print("no script on pdu")
    else:
        for nm in lst:
            func(nm)

# prints the script to stdout
def print_scp(nm):
    ret, scp = mgr.getScript(nm)
    if ret == mgr.NO_ERROR:
        print("  content of the script:")
        print(scp)
    else:
        print("  get script returns an error: " + err_to_str(ret))
    return ret

def print_arg_map_in_a_line(arg_map):
    comma = False
    for key, val in arg_map.items():
        if comma == True:
            sys.stdout.write(", ")
        comma = True
        sys.stdout.write("\'" + key + "\' : \'" + val + "\'")
    sys.stdout.write('\n')
    sys.stdout.flush()

# get arguments from user
def get_args_from_user(args = dict()):
    if len(args) > 0:
        sys.stdout.write("  arguments: ")
        print_arg_map_in_a_line(args)
        while True:
            in_str = get_input("  \'k\' to keep or \'d\' to delete arguments: ")
            if in_str == 'd':
                args = dict()
                break
            elif in_str == 'k':
                break
    print("  note: to go on press return if you will asked for the \'key\'")
    while True:
        in_key = get_input("    write key and/or press return: ")
        if not in_key:
            break
        in_val = get_input("    write value and/or press return: ")
        args[in_key] = in_val
        sys.stdout.write("  arguments: ")
        print_arg_map_in_a_line(args)
    return args

# starts a script and print the return value
def start_scp(nm):
    ret = mgr.startScript(nm)
    print("  starting " + nm + " returns: " + err_to_str(ret))
    return ret

# starts a script with arguments and print the return value
# the user will be asked for more arguments
def start_scp_with_args(nm):
    args = get_args_from_user()
    ret = mgr.startScriptWithArgs(nm, args)
    print("  starting " + nm + " returns: " + err_to_str(ret))
    return ret

# terminates a script and prints the return value
def term_scp(nm):
    ret = mgr.terminateScript(nm)
    print("  terminating " + nm + " returns: " + err_to_str(ret))
    return ret

# deletes a script and prints the return value
def delete_scp(nm):
    ret = mgr.deleteScript(nm)
    if ret == mgr.NO_ERROR:
        print("  script " + nm + "has been deleted")
    else:
        print("  deleting " + nm + "returns an error: " + err_to_str(ret))
    return ret

# get script options from user
def get_opt_from_usr(opt):
    print("  write \'true\', \'false\' or press enter for default value")
    while True:
        in_str = get_input("    start script after booting (" + str(opt.autoStart) + "): ")
        in_str = in_str.lower()
        if in_str == "":
            break
        elif in_str == "true":
            opt.autoStart = True
            break
        elif in_str == "false":
            opt.autoStart = False
            break
    while True:
        in_str = get_input("    restart script after termination or crash (" + str(opt.autoRestart) + "): ")
        in_str = in_str.lower()
        if in_str == "":
            break
        elif in_str == "true":
            opt.autoRestart = True
            break
        elif in_str == "false":
            opt.autoRestart = False
            break
    opt.defaultArgs = get_args_from_user(opt.defaultArgs)
    return opt

# get name for script form user
def get_name_from_usr():
    while True:
        in_str = get_input("    write script name: ")
        if len(in_str) != 0:
            break
    return in_str

# get the filename from user and return the content
def get_script_from_usr():
    while True:
        path = ""
        if args.file != None:
            print("    press enter for default value")
            path = " (" + args.file.name + ")"
        try:
            path = get_input("    path to file" + path + ": ")
            if path == "":
                if args.file != None:
                    scp = args.file
                    break
            else:
                scp = open(os.path.expanduser(path), 'r')
                break
        except:
            print("    can't open file at path: " + path)
    return scp.read()

# set options for a script
def set_scp_opt(nm):
    print("  set options for script " + nm)
    ret, opt = mgr.getScriptOptions(nm)
    if ret != mgr.NO_ERROR:
        print("  get script state returns an error: " + err_to_str(ret))
        return
    opt = get_opt_from_usr(opt)
    ret = mgr.setScriptOptions(nm, opt)
    if ret != mgr.NO_ERROR:
        print("  set script returns an error: " + err_to_str(ret))
    return ret

# gets the option for script and print it
def print_scp_opt(nm):
    ret, opt = mgr.getScriptOptions(nm)
    print("  options for script " + nm + ":")
    if ret == mgr.NO_ERROR:
        print("    start script after booting: " + str(opt.autoStart))
        print("    restart script after termination or crash: " + str(opt.autoRestart))
        print("    default arguments (key value pair per line):")
        for key, val in opt.defaultArgs.items():
            print("      \'" + key + "\' : \'" + val + "\'")
    else:
        print("  get script State returns an error: " + err_to_str(ret))
    return ret

# set default arguments
def set_scp_dfl_args(nm):
    ret, opt = mgr.getScriptOptions(nm)
    if ret == mgr.NO_ERROR:
        print("  change default arguments for script " + nm + ":")
        args = get_args_from_user(opt.defaultArgs)
        opt.defaultArgs = args
        ret = mgr.setScriptOptions(nm, opt)
        if ret == mgr.NO_ERROR:
            print("  change default args returns no error")
        else:
            print("  change default args returns an error: " + err_to_str(ret))
    else:
        print("    get default arguments returns an error: " + err_to_str(ret))
    return ret

# get the default arguments for script and puts it out
def print_scp_dfl_args(nm):
    ret, opt = mgr.getScriptOptions(nm)
    if ret == mgr.NO_ERROR:
        print("  default arguments for script " + nm + ":")
        for key, val in opt.defaultArgs.items():
            print("    \'" + key + "\' : \'" + val + "\'")
    else:
        print("    get default arguments returns an error: " + err_to_str(ret))
    return ret

# to set a script
def set_scp(name = None, script = None, options = None):
    print("  set script at pdu ")
    if name == None:
        name = get_name_from_usr()
    if script == None:
        script = get_script_from_usr()
    if options == None:
        options = get_opt_from_usr(luaservice.ScriptOptions(dict(), False, False))
    ret = mgr.setScript(name, script, options)
    print("    set script returns: " + err_to_str(ret))
    return ret

# read the output from a starting point
def print_scp_output(nm):
    print("  print output for script " + nm)
    while True:
        try:
            iAddr = get_input("    read from address (0): ")
            if iAddr == "":
                iAddr = int(0)
            else:
                iAddr = int(iAddr)
        except:
            print("    not a valid number")
        else:
            break
    ret, oAddr, nAddr, o_str, more = mgr.getScriptOutput(nm, iAddr)
    if ret != mgr.NO_ERROR:
        print("ERROR: reading from output caused an error: " + err_to_str(ret))
    else:
        print("    read from addr " + str(oAddr) + " to addr " + str(nAddr))
        if more == True:
            print("    more output is available")
        print(o_str)
    return ret

# exit when signal is received
def ctrl_c_handler(signum, frame):
    do_exit()

# to read the output continuously
def print_scp_output_cont(nm):
    print("  print output for script " + nm)
    print("    refresh interval is every second")
    print("    to abort press Crtl-C")
    signal.signal(signal.SIGINT, ctrl_c_handler)
    nAddr = int(0)
    while True:
        iAddr = nAddr
        ret, oAddr, nAddr, o_str, more = mgr.getScriptOutput(nm, iAddr)
        if ret != mgr.NO_ERROR:
            print("ERROR: reading from output caused an error: " + err_to_str(ret))
            break
        if iAddr != oAddr:
            print("    *** starts reading at " + str(oAddr) + " address ***")
        if iAddr != nAddr:
            print(o_str)
        if more == False:
            time.sleep(1)
    return ret

# manage a existing lua script
def manage_script(scp):
    while True:
        print("")
        print("Lua script options for \'" + scp + "\':")
        print("  a: set script")
        print("  p: print script")
        print("  d: delete script")
        print("  h: set option")
        print("  g: get option")
        print("  j: set default arguments")
        print("  k: get default arguments")
        print("  s: start script")
        print("  x: start script with additional args")
        print("  t: terminate script")
        print("  l: get script state")
        print("  o: get output")
        print("  c: get output continuously")
        print("  b: back")
        print("  q: quit application")

        opt = get_input("choose an option and press enter: ")
        ret = mgr.NO_ERROR
        if opt == 'a':
            ret = set_scp(scp)
        elif opt == 'p':
            ret = print_scp(scp)
        elif opt == 'd':
            delete_scp(scp)
            break
        elif opt == 'h':
            ret = set_scp_opt(scp)
        elif opt == 'g':
            ret = print_scp_opt(scp)
        elif opt == 'j':
            ret = set_scp_dfl_args(scp)
        elif opt == 'k':
            ret = print_scp_dfl_args(scp)
        elif opt == 's':
            ret = start_scp(scp)
        elif opt == 'x':
            ret = start_scp_with_args(scp)
        elif opt == 't':
            ret = term_scp(scp)
        elif opt == 'l':
            ret = print_scp_state(scp)
        elif opt == 'o':
            ret = print_scp_output(scp)
        elif opt == 'c':
            ret = print_scp_output_cont(scp)
        elif opt == 'b':
            break
        elif opt == 'q':
            do_exit()
        else:
            print("ERROR: can't parse selection")

        if ret == mgr.ERR_NO_SUCH_SCRIPT:
            break  # script is missing, back to manage pdu

# manage a pdu
def manage_pdu():
    names = mgr.getScriptNames()
    if len(names) == 0:
        print("Lua script on" + agent.url + ": No script uploaded")
    else:
        print("Lua scripts on " + agent.url + ":")
        for idx, nm in enumerate(names):
            print("  " + str(idx) + ": " + nm)
    print("options:")
    print("  l: list script states")
    print("  s: start all scripts")
    print("  t: terminate all scripts")
    print("  n: add a new script")
    print("  e: list environment")
    print("  q: quit application")

    opt = get_input("choose a script or option and press enter: ")

    if opt == 'l':
        for_all_do(names, print_scp_state)

    elif opt == 's':
        for_all_do(names, start_scp)

    elif opt == 't':
        for_all_do(names, term_scp)

    elif opt == 'e':
        list_env(mgr)

    elif opt == 'n':
        set_scp()

    elif opt == 'q':
        do_exit()

    else:
        try:
            nm = names[int(opt)]
            names = None
        except:
            print("ERROR: can't parse selection")
        else:
            # script opterations
            manage_script(nm)

# ------------------------------------------------------------------------------
# main prog
# ------------------------------------------------------------------------------

# command line args
parser = argparse.ArgumentParser(prog="luaservice_client", description="Manage Lua scripts")

mutable_grp = parser.add_mutually_exclusive_group()
mutable_grp.add_argument("-r", "--run", help="set script, start script, show output", type=argparse.FileType('r'))
mutable_grp.add_argument("-f", "--file", help="to set a default script", type=argparse.FileType('r'))
addr_grp = parser.add_argument_group("pdu address")
addr_grp.add_argument("-a", "--addr", help="an address or ip address of the pdu", required=True)
addr_grp.add_argument("-P", "--port", help="the port of the pdu", default=443, type=int)
addr_grp.add_argument("-p", "--passwd", help="the password", default="raritan")
addr_grp.add_argument("-u", "--user", help="the user name", default="admin")
addr_grp.add_argument("-n", "--no_secure", dest="secure", action="store_false", help="protocol to use - type True for https or False for http")

args = parser.parse_args()
# create luaservice manager
agent = Agent("https" if args.secure else "http", args.addr + ":" + str(args.port),
        args.user, args.passwd, debug = False,
        disable_certificate_verification=True) # No SSL certification verification
mgr = luaservice.Manager("/luaservice", agent)
print("agent created: url " + agent.url)

# option --run
if args.run != None:
    nm = os.path.splitext(os.path.basename(args.run.name))[0]
    print ("name " + nm)
    opts = luaservice.ScriptOptions(dict(), False, False)
    scp = args.run.read()
    ret = mgr.setScript(nm, scp, opts)
    if ret != mgr.NO_ERROR:
        print("  set script returns an error: " + err_to_str(ret))
        do_exit()
    ret = start_scp(nm)
    if ret != mgr.NO_ERROR:
        do_exit()
    print_scp_output_cont(nm)
    do_exit()

# manager operations ...
while True:
    print("")
    manage_pdu()
