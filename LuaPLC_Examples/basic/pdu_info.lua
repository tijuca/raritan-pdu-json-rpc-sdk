-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Print some information from the PDU data model.

-- load the "Pdu" library and all dependencies
require "Pdu"

-- Acquire the single instance of the Pdu interface
local pdu = pdumodel.Pdu:getDefault()

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
