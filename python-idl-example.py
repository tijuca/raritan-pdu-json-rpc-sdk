#!/usr/bin/python3
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2015 Raritan Inc. All rights reserved.

import sys
import csv

sys.path.append("pdu-python-api")
import raritan.rpc
from raritan.rpc import servermon, event, usermgmt, um, devsettings, pdumodel, cert, sensors

hostname = '10.0.42.2'
username = 'admin'
password = 'raritan'

rpcagent = raritan.rpc.Agent('https', hostname, username, password, disable_certificate_verification = True)

def configure_servermon(agent):
    #
    # Configure server monitor entry for 10.0.42.3
    #
    server_settings = servermon.ServerMonitor.ServerSettings(
        host = '10.0.42.3',
        enabled = True,
        pingInterval = 30,
        retryInterval = 5,
        activationCount = 3,
        failureCount = 5,
        resumeDelay = 180,
        resumeCount = 2,
        powerSettings = servermon.ServerMonitor.ServerPowerSettings(
            enabled = False,
            target = None,
            powerCheck = servermon.ServerMonitor.ServerPowerCheckMethod.TIMER,
            powerThreshold = 0.0,
            timeout = 5,
            shutdownCmd = "echo test",
            username = "admin",
            password = "raritan",
            sshPort = 22)
    )

    print('Creating server monitor entry ...')
    servermon_proxy = servermon.ServerMonitor('/servermon', agent)
    ret, server_id = servermon_proxy.addServer(server_settings)
    if ret == 0:
        print('Server entry successfully added; id = ', server_id)
    elif ret == servermon.ServerMonitor.ERR_DUPLICATE_HOSTNAME:
        print('ServerMonitor.addServer() failed: server entry already exists.')
    else:
        print('ServerMonitor.addServer() failed: ret = ', ret)
        sys.exit(1)

def configure_event(agent):
    #
    # Create event action for power-cycling an outlet
    #
    action = event.Engine.Action(
        id = '', # assigned by addAction()
        name = 'Cycle ISP device',
        isSystem = False,
        type = 'SwitchOutlet',
        arguments = [
            event.KeyValue('Operation', 'cycle'),
            event.KeyValue('OutletIndex', '0')
        ]
    )

    eventengine_proxy = event.Engine('/event_engine', agent)

    # Try to find an existing action with the given name
    existing_action_id = None
    for existing_action in eventengine_proxy.listActions():
        if existing_action.name == action.name:
            print('Found existing action; id = ', existing_action.id)
            existing_action_id = existing_action.id
            break

    if existing_action_id != None:
        print('Updating existing event action ...')
        action.id = action_id = existing_action_id
        ret = eventengine_proxy.modifyAction(action)
        if ret == 0:
            print('Action successfully updated.')
        else:
            print('event.Engine.modifyAction() failed: ret = ', ret)
            sys.exit(1)
    else:
        print('Creating new event action ...')
        ret, action_id = eventengine_proxy.addAction(action)
        if ret == 0:
            print('Action successfully created; id = ', action_id)
        else:
            print('event.Engine.addAction() failed: ret = ', ret)
            sys.exit(1)

    #
    # Create an event rule to invoke action when server becomes unreachable
    #
    rule = event.Engine.Rule(
        id = '', # assigned by addRule()
        name = 'Cycle ISP device',
        isSystem = False,
        isEnabled = True,
        isAutoRearm = True,
        hasMatched = False,
        condition = event.Engine.Condition(
            negate = False,
            operation = event.Engine.Condition.Op.AND,
            matchType = event.Engine.Condition.MatchType.ASSERTED,
            eventId = [ 'ServerMonitor', '10.0.42.3', 'Unreachable' ],
            conditions = []
        ),
        actionIds = [ 'SystemEventLogAction', 'SystemSnmpTrapAction', action_id ],
        arguments = []
    )

    print('Creating new event rule ...')
    ret, rule_id = eventengine_proxy.addRule(rule)
    if ret == 0:
        print('Rule successfully created; id = ', rule_id)
    elif ret == 1:
        print('event.Engine.addRule() failed: rule already exisits.')
    else:
        print('event.Engine.addRule() failed: ret = ', ret)
        sys.exit(1)

def create_user(agent):
    #
    # Create or update user account
    #
    new_userinfo = usermgmt.UserInfo(
        enabled = True,
        locked = False,
        blocked = False,
        needPasswordChange = False,
        auxInfo = usermgmt.AuxInfo(
            fullname = 'John Doe',
            telephone = '555-123-1234',
            eMail = 'John.Doe@example.net'
        ),
        snmpV3Settings = usermgmt.SnmpV3Settings(
            enabled = False,
            secLevel = um.SnmpV3.SecurityLevel.AUTH_PRIV,
            authProtocol = um.SnmpV3.AuthProtocol.SHA1,
            usePasswordAsAuthPassphrase = True,
            haveAuthPassphrase = False,
            authPassphrase = '',
            privProtocol = um.SnmpV3.PrivProtocol.DES,
            useAuthPassphraseAsPrivPassphrase = True,
            havePrivPassphrase = False,
            privPassphrase = ''
        ),
        sshPublicKey = '',
        preferences = usermgmt.Preferences(
            temperatureUnit = usermgmt.TemperatureEnum.DEG_F,
            lengthUnit = usermgmt.LengthEnum.FEET,
            pressureUnit = usermgmt.PressureEnum.PSI
        ),
        roleIds = [ 0 ] # role 0 = Administrator
    )

    new_password = 'secretpassword'

    try:
        print('Trying to update existing account ...')
        user_proxy = usermgmt.User('/auth/user/johndoe', agent)
        ret = user_proxy.updateAccountFull('', new_userinfo)
        if ret == 0:
            print('Account successfully updated.')
        else:
            print('User.updateAccountFull() failed: ret = ', ret)
            sys.exit(1)

        print('Setting account password ...')
        ret = user_proxy.setAccountPassword(new_password)
        if ret == 0:
            print('Password successfully changed.')
        elif ret == usermgmt.User.ERR_PASSWORD_UNCHANGED:
            print('Password unchanged.')
        else:
            print('User.setAccountPassword() failed: ret = ', ret)
            sys.exit(1)
    except raritan.rpc.HttpException:
        print('Account not found; creating new one ...')
        usermgr_proxy = usermgmt.UserManager('/auth/user', agent)
        ret = usermgr_proxy.createAccountFull('johndoe', new_password, new_userinfo)
        if ret == 0:
            print('Account successfully created.')
        else:
            print('UserManager.createAccountFull() failed: ret = ', ret)
            sys.exit(1)

def configure_snmp(agent):
    #
    # Update SNMP settings
    #
    eventengine_proxy = event.Engine('/event_engine', agent)

    for action in eventengine_proxy.listActions():
        if action.id == 'SystemSnmpTrapAction':
            action.arguments = [
                event.KeyValue('SnmpNotfType', 'v2Trap'),
                event.KeyValue('SnmpTrapDest1', 'trap-dest1-hostname:162:community'),
                event.KeyValue('SnmpTrapDest2', 'trap-dest2-hostname:162:community')
                ]
            ret = eventengine_proxy.modifyAction(action)
            if ret == 0:
                print('System SNMP trap action successfully updated.')
            else:
                print('event.Engine.modifyAction() failed: ret = ', ret)
                sys.exit(1)

    snmp_settings = devsettings.Snmp.Configuration(
        v2enable = True,
        v3enable = False,
        readComm = 'readcommunity',
        writeComm = 'private',
        sysContact = 'admin@example.net',
        sysName = 'my system',
        sysLocation = ''
    )

    print('Updating SNMP agent settings ...')
    snmp_proxy = devsettings.Snmp('/snmp', agent)
    ret = snmp_proxy.setConfiguration(snmp_settings)
    if ret == 0:
        print('SNMP agent settings successfully updated.')
    else:
        print('Snmp.setConfiguration() failed: ret = ', ret)
        sys.exit(1)


def configure_outlet(agent):
    #
    # Update outlet 1 settings
    #
    outlet_settings = pdumodel.Outlet.Settings(
        name = 'Device 1',
        startupState = pdumodel.Outlet.StartupState.SS_PDUDEF,
        usePduCycleDelay = False,
        cycleDelay = 30,
        nonCritical = False,
        sequenceDelay = 0
    )

    print('Updating outlet settings ...')
    outlet_proxy = pdumodel.Outlet('/model/outlet/0', agent)
    ret = outlet_proxy.setSettings(outlet_settings)
    if ret == 0:
        print('Outlet settings successfully updated.')
    else:
        print('Outlet.setSettings() failed: ret = ', ret)


def configure_pduname(agent, name):
    #
    # Set PDU name
    #
    print('Getting PDU settings...')
    pdu = pdumodel.Pdu('/model/pdu/0', agent)
    settings = pdu.getSettings()
    print('Successfully retrieved current PDU settings.')
    settings.name = name
    print('Updating PDU settings...')
    ret = pdu.setSettings(settings)
    if ret == 0:
        print('PDU settings successfully updated.')
    else:
        print('Pdu.setSettings() failed: ret = ', ret)
        sys.exit(1)

def configure_certificate(agent, certfilename, keyfilename):
    # read files in binary mode
    cert_file = open(certfilename, "rb")
    key_file = open(keyfilename, "rb")
    # upload
    cert.upload(agent, cert_file.read(), key_file.read())

    print('Activating new certificate...')
    cert_proxy = cert.ServerSSLCert('/server_ssl_cert', agent)
    ret = cert_proxy.installPendingKeyPair()
    if ret == 0:
        print('Successfully installed new certificate.')
    else:
        print('ServerSSLCert.installPendingKeyPair failed: ret = ', ret)
        sys.exit(1)

def configure_inlet_activepower_thresholds(agent):
    #
    # Update active power thresholds for first inlet
    #
    active_power_sensor = sensors.NumericSensor("/model/pdu/0/inlet/0/activePower", agent)
    # Get current thresholds
    thresholds = sensors.NumericSensor.Thresholds(
        upperCriticalActive   = True,
        upperCritical         = 8000.0,
        upperWarningActive    = True,
        upperWarning          = 5000.0,
        lowerWarningActive    = False,
        lowerWarning          = 0.0,
        lowerCriticalActive   = False,
        lowerCritical         = 0.0,
        assertionTimeout      = 0,
        deassertionHysteresis = 0.0)

    print('Updating inlet active power thresholds...')
    ret = active_power_sensor.setThresholds(thresholds)
    if ret == 0:
        print('Inlet active power thresholds successfully updated.')
    else:
        print('setThresholds() failed: ret = ', ret)

def execute_for_each_csv_row(func, filename):
    print("Reading from CSV file: ", filename)
    with open(filename, 'r') as csvfile:
        reader = csv.reader(csvfile, delimiter=',', quotechar='|')
        for row in reader:
            if len(row) != 3:
                print("Row", reader.line_num, "not correctly formatted. Required format: IP,USERNAME,PASSWORD")
                continue
            print("Configuring ip: ", row[0])
            localagent = raritan.rpc.Agent('https', row[0], row[1], row[2], disable_certificate_verification = True)
            func(localagent)

print("Please edit this file and enable/disable the desired functions.")
#configure_servermon(rpcagent)
#configure_event(rpcagent)
#configure_snmp(rpcagent)
#configure_outlet(rpcagent)
#configure_certificate(rpcagent, 'server_ssl_cert_default.pem', 'server_ssl_key_default.pem')
#configure_pduname(rpcagent, 'test name')
#configure_inlet_activepower_thresholds(rpcagent)
#execute_for_each_csv_row(configure_inlet_activepower_thresholds, "test.csv")

