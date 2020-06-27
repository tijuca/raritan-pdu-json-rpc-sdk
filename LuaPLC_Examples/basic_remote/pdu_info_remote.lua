-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Print some information from the data model of a remote PDU.
-- Note: You will have to change the IP address in the HttpAgent line!

-- load the "Pdu" library and all dependencies
require "Pdu"

-- Create an HTTP agent holding the connection details of a remote device
local http_agent = agent.HttpAgent:new("192.168.99.99", "admin", "raritan")

-- Create a proxy object using the HTTP agent and the well-known URI of
-- the root PDU instance
local pdu = pdumodel.Pdu:newRemote("/model/pdu/0", http_agent)

-- Get and print some information from the PDU nameplate.
--
-- According to Pdu.idl, getNameplate() returns a structure. In Lua, this is
-- represented as a table whose keys are the field names of the structure.
local nameplate = pdu:getNameplate()
print("PDU Manufacturer: " .. nameplate.manufacturer)
print("PDU Model: " .. nameplate.model)
print()

-- Get and print the PDU metadata structure.
--
-- To display a complete table, use the built-in function 'printTable'.
print("PDU metadata:")
printTable(pdu:getMetaData())
print()

-- Iterate through all outlets and print their power state
local outlets = pdu:getOutlets()
for _, outlet in ipairs(outlets) do
    -- Get the outlet label (from the metadata) and power state
    local label = outlet:getMetaData().label
    local state = outlet:getState()

    if state.powerState == pdumodel.Outlet.PowerState.PS_ON then
	print("Outlet " .. label .. ": ON")
    else
	print("Outlet " .. label .. ": OFF")
    end
end
