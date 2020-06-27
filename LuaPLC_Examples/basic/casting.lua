-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Casting Example
-- Sometimes you need to cast objects. This example demonstrates how to use the
-- cast method. The generated output is in the --[[ ... ]]-- statement.

require "Pdu"

-- get oulet 1
local pdu     = pdumodel.Pdu:getDefault()
local outlets = pdu:getOutlets()
local out1    = outlets[1]

print("state from outlet 1")
printTable(out1:getState())
print()
--[[
state from outlet 1
cycleInProgress: false
available: true
lastPowerStateChange: 1468230437
ledState:    -->   table: 0x528678
  green: false
  red: false
  blinking: false
switchOnInProgress: false
powerState: 0
]]--

print("get parent from outlet 1")
parent = out1:getParents()
print("parent vector length" .. #parent)
print("parent type: ")
printTable(parent[1]:getTypeInfo())
print()
--[[
get parent from outlet 1
parent vector length1
parent type: 
1: pdumodel.OverCurrentProtector
2: pdumodel.EDevice
3: idl.Object
]]--

-- cast to OverCurrentProtector
print("cast parent to over current protector and print metadata")
local currentProtector = pdumodel.OverCurrentProtector:cast(parent[1])

print("table metadata")
printTable(currentProtector:getMetaData())
print()
--[[
cast parent to over current protector and print metadata
table metadata
label: C1
namePlate:    -->   table: 0x508210
  rating:    -->   table: 0x508420
    frequency: 
    power: 
    voltage: 250V
    current: 20A
  model: FAZ-C20/1-NA
  serialNumber: <not set>
  manufacturer: Moeller
  imageFileURL: 
  partNumber: 
type: 0
rating:    -->   table: 0x5081c8
  minVoltage: 0
  maxVoltage: 0
  current: 20
  maxTripCnt: 1000
]]--

child = out1:getChildren()
print("outlet 1: children vector length " .. #child)
print()
--[[
outlet 1: children vector length 0
]]--
