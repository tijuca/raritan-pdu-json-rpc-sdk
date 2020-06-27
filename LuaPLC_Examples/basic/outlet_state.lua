-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Count the power state from all outlets

require "Pdu"

-- Get the root Pdu instance and all outlets
local pdu = pdumodel.Pdu:getDefault()
local outlets = pdu:getOutlets()

-- Define two counter variables for outlets being on or off
local on_count = 0
local off_count = 0

-- Iterate over all outlets
for _, outlet in ipairs(outlets) do
    -- Read the outlet power state
    local state = outlet:getState().powerState

    -- Increment the 'on' or 'off' counter depending on power state
    if state == pdumodel.Outlet.PowerState.PS_ON then
        on_count = on_count + 1
    elseif state == pdumodel.Outlet.PowerState.PS_OFF then
        off_count = off_count + 1
    else
        -- this should never happen
        print("ERROR: Unexpected outlet state: " .. state)
    end
end

print("Outlet power state summary:")
print("  " .. on_count  .. "/" .. #outlets .. " outlets are on.")
print("  " .. off_count .. "/" .. #outlets .. " outlets are off.")
print()
