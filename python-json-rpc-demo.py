#!/usr/bin/python3
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2015 Raritan Inc. All rights reserved.

import sys, time

sys.path.append("pdu-python-api")
from raritan.rpc import Agent, pdumodel, firmware

ip = "10.0.42.2"
user = "admin"
pw = "raritan"

try:
    ip = sys.argv[1]
    user = sys.argv[2]
    pw = sys.argv[3]
except IndexError:
    pass # use defaults

agent = Agent("https", ip, user, pw, disable_certificate_verification=True)
pdu = pdumodel.Pdu("/model/pdu/0", agent)
firmware_proxy = firmware.Firmware("/firmware", agent)

inlets = pdu.getInlets()
ocps = pdu.getOverCurrentProtectors()
outlets = pdu.getOutlets()

print ("PDU: %s" % (ip))
print ("Firmware version: %s" % (firmware_proxy.getVersion()))
print ("Number of inlets: %d" % (len(inlets)))
print ("Number of over current protectors: %d" % (len(ocps)))
print ("Number of outlets: %d" % (len(outlets)))

outlet = outlets[0]
outlet_sensors = outlet.getSensors()
outlet_metadata = outlet.getMetaData()
outlet_settings = outlet.getSettings()

print ("Outlet %s:" % (format(outlet_metadata.label)))
print ("  Name: %s" % (outlet_settings.name if outlet_settings.name != "" else "(none)"))
print ("  Switchable: %s" % ("yes" if outlet_metadata.isSwitchable else "no"))

if outlet_sensors.voltage:
    sensor_reading = outlet_sensors.voltage.getReading()
    print ("  Voltage: %s" % (("%d V" % (sensor_reading.value)) if sensor_reading.valid else "n/a"))

if outlet_sensors.current:
    sensor_reading = outlet_sensors.current.getReading()
    print ("  Current: %s" % (("%d A" % (sensor_reading.value)) if sensor_reading.valid else "n/a"))

if outlet_metadata.isSwitchable:
    outlet_state_sensor = outlet_sensors.outletState
    outlet_state = outlet_state_sensor.getState()
    if outlet_state.available:
        print ("  Status :%s" % ("on" if outlet_state.value == outlet_state_sensor.OnOffState.ON.val else "off"))
    print ("  Turning outlet off...")
    outlet.setPowerState(outlet.PowerState.PS_OFF)
    print ("  Sleeping 4 seconds...")
    time.sleep(4)
    print ("  Turning outlet on...")
    outlet.setPowerState(outlet.PowerState.PS_ON)
    outlet_state = outlet_state_sensor.getState()
    if outlet_state.available:
        print ("  Status :%s" % ("on" if outlet_state.value == outlet_state_sensor.OnOffState.ON.val else "off"))
