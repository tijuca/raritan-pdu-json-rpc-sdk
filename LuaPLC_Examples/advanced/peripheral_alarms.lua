-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Enables load shedding if any temperature reading is above the upper
-- warning threshold.
--
-- This script should be set up to be triggered by an event rule for:
-- Peripheral Device Slot/<Any Slot>/Numeric Sensor/Temperature/<Any sub-event>

require "Pdu"

local pdu = pdumodel.Pdu:getDefault()

-- Iterate over peripheral devices, count alarmed temperature sensors
local temp_alarms = 0
local slots = pdu:getPeripheralDeviceManager():getDeviceSlots()

for _, slot in ipairs(slots) do
    local device = slot:getDevice()
    if device ~= nil and
       device.deviceID.type.readingtype == sensors.Sensor.NUMERIC and
       device.deviceID.type.type == sensors.Sensor.TEMPERATURE then
        local nsensor = sensors.NumericSensor:cast(device.device)
        local status = nsensor:getReading().status
        if status.aboveUpperWarning or status.aboveUpperCritical then
            temp_alarms = temp_alarms + 1
        end
    end
end

-- Enable or disable load shedding depending on number of alarmed sensors
local load_shedding_active = pdu:isLoadSheddingActive()
if temp_alarms == 0 and load_shedding_active then
    print(os.date("%c") .. ": Disabling load shedding")
    pdu:setLoadSheddingActive(false)
elseif temp_alarms > 0 and not load_shedding_active then
    print(os.date("%c") .. ": Enabling load shedding")
    pdu:setLoadSheddingActive(true)
end
