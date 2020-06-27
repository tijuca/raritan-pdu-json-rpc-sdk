-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Toggles the third outlet ten times in one-second intervals

require "Pdu"

-- Get root Pdu instance
local pdu = pdumodel.Pdu:getDefault()

-- Get the third outlet; terminate if there is no such outlet
local out3 = pdu:getOutlets()[3]
assert(out3)

-- Get the initial outlet state
local state = out3:getState()
local powerstate = state.powerState
local lastchange = os.date("%c", state.lastPowerStateChange)

print("Initial state:")
print("  Power state: " .. state.powerState)
print("  Last change: " .. lastchange)
print()

for i = 1, 10 do
    -- Wait for one second
    sleep(1)

    -- Toggle outlet power state
    if powerstate == pdumodel.Outlet.PowerState.PS_ON then
        print("Switching outlet OFF ...")
        powerstate = pdumodel.Outlet.PowerState.PS_OFF
    else
        print("Switching outlet ON ...")
        powerstate = pdumodel.Outlet.PowerState.PS_ON
    end
    out3:setPowerState(powerstate)
end
print(" --- done ---")
