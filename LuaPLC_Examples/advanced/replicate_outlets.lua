-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- This script will replicate any outlet state changes to a remote PDU.

require "Pdu"
require "EventService"

-- Change remote PDU IP address and credentials here
local http_agent = agent.HttpAgent:new("192.168.99.99", "admin", "raritan")

local function debug(...)
    -- Uncomment to enable debug output:
    -- print("DEBUG:", (...))
end

-- Collect outlet state sensors so we can identify the outlet index by the
-- source of received events
local outletidx_by_statesensor = {}

local outlets = pdumodel.Pdu.getDefault():getOutlets()
for i = 1, #outlets do
    state_sensor = outlets[i]:getSensors().outletState
    if state_sensor then
        outletidx_by_statesensor[state_sensor:getId()] = i
    end
end

-- Create an event channel and sign up for state sensor change events
debug("Creating event channel ...")
local event_service = event.Service:getDefault()
local channel = event_service:createChannel()
channel:demandEventType(sensors.StateSensor.StateChangedEvent.TYPE_ID)

print(os.date("%c") .. ": Script started")
while true do
    debug("Waiting for events ...")
    -- Wait for new events; this will block until new events are available
    local more, events = channel:pollEvents()

    for _, event in ipairs(events) do
        -- We do not expect anything other than sensor state change events.
        assert(event.TYPE_ID == sensors.StateSensor.StateChangedEvent.TYPE_ID)

        -- Check whether the event originated from an outlet state sensor
        local idx = outletidx_by_statesensor[event.source:getId()]
        if idx == nil then
            debug("Ignoring event from unknown state sensor " .. event.source:getId())
        elseif not event.newState.available then
            print(os.date("%c") .. ": Outlet " .. idx .. " became unavailable!")
        else
            debug("Got event for outlet " .. idx .. ", new state = " .. event.newState.value)

            -- URI of corresponding remote outlet; JSON-RPC outlet indices are 0-based!
            local remote_uri = "/model/outlet/" .. (idx - 1)

            -- Replicate state change to remote outlet
            local remote_outlet = pdumodel.Outlet:newRemote(remote_uri, http_agent)
            if event.newState.value == 0 then
                print(os.date("%c") .. ": Switching OFF " .. remote_uri)
                remote_outlet:setPowerState(pdumodel.Outlet.PowerState.PS_OFF)
            else
                print(os.date("%c") .. ": Switching ON " .. remote_uri)
                remote_outlet:setPowerState(pdumodel.Outlet.PowerState.PS_ON)
            end
        end
    end
end
