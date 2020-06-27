-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- This example code shows how to use events. 'Upper Warning' of sensor 'Temperatur 1'
-- will be tracked and processed. Outlet 8 will be disabled if Upper Warning is true
-- and will be disabled if 'Upper Warning' is false.

require "Pdu"
require "EventService"

-- get outlet 8 from pdu
local agent = agent.HttpAgent:new("192.168.7.15", "admin", "raritan")
local pdu     = pdumodel.Pdu:newRemote("model/pdu/0", agent)
local outlets = pdu:getOutlets()
local o8 = outlets[8]
print("switch on outlet 8")
o8:setPowerState(pdumodel.Outlet.PowerState.PS_ON)
sleep(1) -- just wait one second 

-- get sensor with name 'Temperature 1'
local dm = pdu:getPeripheralDeviceManager()
local dses = dm:getDeviceSlots()
local dsIdx  = 0
for i=1,#dses do
    print(i .. ": " .. dses[i]:getSettings().name)
    if dses[i]:getSettings().name == "Temperature 1" then
        dsIdx = i
    end
end

if dsIdx == 0 then
    print("sensor not found")
    os.exit(1)
else
    print("found sensor at position: " .. dsIdx)
end

ds = dses[dsIdx]
local temp1 = sensors.NumericSensor:cast( ds:getDevice().device )
print()

-- print thresholds
local t = temp1:getThresholds()
print("upper critical is active: " .. tostring(t.upperCriticalActive) .. " : " .. t.upperCritical)
print("upper warning is active:  " .. tostring(t.upperWarningActive ) .. " : " .. t.upperWarning )
print("lower warning is active:  " .. tostring(t.lowerWarningActive ) .. " : " .. t.lowerWarning )
print("lower critical is active: " .. tostring(t.lowerCriticalActive) .. " : " .. t.lowerCritical)


-- get event service and create a new channel
local service = event.Service:newRemote("/eventservice", agent)
local channel = service:createChannel()

-- subscribe events -> all events for "Temperature 1"
channel:demandEvent(idl.Event.TYPE_ID, temp1)

print()
print("- - -   get events   - - -")
print()

-- forever loop
while true do
    -- pollEvents() blocks if there is no event
    local moreEvents, events = channel:pollEvents() -- moreEvents not used
    print("number of events: " .. #events)
    
    -- process events if there are events
    if #events > 0 then
        for i=1,#events do
            print(i .. " Type: " .. events[i].TYPE_ID)

            -- Reading Changed Event
            if events[i].TYPE_ID == sensors.NumericSensor.ReadingChangedEvent.TYPE_ID then
                local time = os.date("*t", events[i].newReading.timestamp)
                print("value: " .. events[i].newReading.value
                    .. " is valid: " .. tostring(events[i].newReading.valid)
                    .. " time: " .. time.hour .. ":" .. time.min .. ":" .. time.sec)

                local o = events[i].source
                print("id " .. o:getId())

            -- State Changed Event
            elseif  events[i].TYPE_ID == sensors.NumericSensor.StateChangedEvent.TYPE_ID then
                local l1 = events[i].newReading.status.aboveUpperCritical
                local l2 = events[i].newReading.status.aboveUpperWarning
                local l3 = events[i].newReading.status.belowLowerWarning
                local l4 = events[i].newReading.status.belowLowerCritical
                local time = os.date("*t", events[i].newReading.timestamp)
                print("aboveUpperCritical: " .. tostring(l1))
                print("aboveUpperWarning:  " .. tostring(l2))
                print("belowLowerWarning:  " .. tostring(l3))
                print("belowLowerCritical: " .. tostring(l4))
                print(" is valid: " .. tostring(events[i].newReading.valid)
                    .. " time: " .. time.hour .. ":" .. time.min .. ":" .. time.sec)

                -- aboveUpperWarning
                if l2 == true then
                    o8:setPowerState(pdumodel.Outlet.PowerState.PS_OFF)
                    print("switched off outlet 8")
                else
                    o8:setPowerState(pdumodel.Outlet.PowerState.PS_ON)
                    print("switched on outlet 8")
                end
            end
        end

    else
        print("no event catched")
        local reading = temp1:getReading()
        print("value: " .. reading.value)
        print("aboveUpperWarning: " .. tostring( reading.status.aboveUpperWarning ))
    end

    print()
end
