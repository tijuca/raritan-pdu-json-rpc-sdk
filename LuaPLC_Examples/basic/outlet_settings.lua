-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Shows how to read and modify outlet settings

require "Pdu"

-- Get root Pdu instance
local pdu = pdumodel.Pdu:getDefault()

-- Get the third outlet; terminate if there is no such outlet
local out3 = pdu:getOutlets()[3]
assert(out3)

-- Read and print current outlet settings
local settings = out3:getSettings()
print("Current outlet settings:")
printTable(settings)
print()

-- Modify the settings and write them back
settings.name = "My Outlet"
local rc = out3:setSettings(settings)
if rc == 0 then
    print("Outlet settings successfully changed.")
else
    print("setSettings() failed, return code: " .. rc)
end
print("New outlet settings:")
printTable(out3:getSettings())
print()

-- Create a settings structure from scratch. All fields as defined in
-- Outlet.idl must be present!
new_settings = {
    name             = "My New Outlet",
    startupState     = pdumodel.Outlet.StartupState.SS_ON,
    usePduCycleDelay = false,
    cycleDelay       = 5,
    nonCritical      = true,
    sequenceDelay    = 500
}

-- Write the new settings to the outlet
rc = out3:setSettings(new_settings)
if rc == 0 then
    print("Outlet settings successfully changed.")
else
    print("setSettings() failed, return code: " .. rc)
end
print("New outlet settings:")
printTable(out3:getSettings())
print()
