-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2019 Raritan Inc. All rights reserved.

-- Switch the third outlet of the first link PDU

require "Pdu"

-- Create an HTTP agent for the link unit
local http_agent = agent.HttpAgent:getForLinkUnit(2)
assert(http_agent)

-- Create a proxy object using the HTTP agent and the well-known URI of
-- the root PDU instance
local pdu = pdumodel.Pdu:newRemote("/model/pdu/0", http_agent)

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

if powerstate == pdumodel.Outlet.PowerState.PS_ON then
    print("Switching outlet OFF ...")
    powerstate = pdumodel.Outlet.PowerState.PS_OFF
else
    print("Switching outlet ON ...")
    powerstate = pdumodel.Outlet.PowerState.PS_ON
end

-- Toggle outlet power state
out3:setPowerState(powerstate)

print(" --- done ---")
