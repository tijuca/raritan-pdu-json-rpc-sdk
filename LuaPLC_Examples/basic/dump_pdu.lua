#! /bin/l4a
-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2015 Raritan Inc. All rights reserved.

require "Pdu"
require "PowerQualitySensor"
require "ResidualCurrentStateSensor"

local function unit_to_symbol(unit)
    local symbols = {}
    symbols[sensors.Sensor.NONE] = ""
    symbols[sensors.Sensor.VOLT] = "V"
    symbols[sensors.Sensor.AMPERE] = "A"
    symbols[sensors.Sensor.WATT] = "W"
    symbols[sensors.Sensor.VOLT_AMP] = "VA"
    symbols[sensors.Sensor.WATT_HOUR] = "Wh"
    symbols[sensors.Sensor.VOLT_AMP_HOUR] = "VAh"
    symbols[sensors.Sensor.HZ] = "Hz"
    symbols[sensors.Sensor.PERCENT] = "%"
    symbols[sensors.Sensor.VOLT_AMP_REACTIVE] = "var"
    symbols[sensors.Sensor.VOLT_AMP_REACTIVE_HOUR] = "varh"
    symbols[sensors.Sensor.DEGREE] = "deg"
    local symbol = symbols[unit]
    if symbol == nil then symbol = "?" end
    return symbol
end

local function dump_nsensor(sensor, name, ind)
    if sensor ~= nil then
	local typespec = sensor:getTypeSpec()
	local reading = sensor:getReading()
	local formatted
	if reading.valid then
	    formatted = string.format("%.2f", reading.value)
	    if typespec.unit ~= sensors.Sensor.NONE then
		formatted = formatted .. " " .. unit_to_symbol(typespec.unit)
	    end
	    local status = reading.status
	    local state = "normal"
	    if status.aboveUpperCritical then
		state = "upper critical"
	    elseif status.aboveUpperWarning then
		state = "upper warning"
	    elseif status.belowLowerCritical then
		state = "lower critical"
	    elseif status.belowLowerWarning then
		state = "lower warning"
	    end
	    formatted = formatted .. " (" .. state .. ")"
	else
	    formatted = "n/a"
	end
	print(ind .. name .. ": " .. formatted)
    end
end

local function dump_ssensor(sensor, name, ind, mapping)
    if sensor ~= nil then
	local state = sensor:getState()
	if not state.available then
	    print(ind .. name .. ": n/a")
	elseif mapping == nil then
	    print(ind .. name .. string.format(": %d", state.value))
	else
	    local mapped = mapping(state.value)
	    if mapped == nil then mapped = "unknown" end
	    print(ind .. name .. string.format(": %s (%d)", mapped, state.value))
	end
    end
end

local function dump_pole(pole, ind)
    -- used for both Pole and DoublePole
    local line = "?"
    if pole.line == pdumodel.PowerLine.L1 then
	line = "L1"
    elseif pole.line == pdumodel.PowerLine.L2 then
	line = "L2"
    elseif pole.line == pdumodel.PowerLine.L3 then
	line = "L3"
    elseif pole.line == pdumodel.PowerLine.NEUTRAL then
	line = "Neutral"
    end
    print(ind .. "Line: " .. line)
    dump_nsensor(pole.voltage, "Voltage (LL)", ind)
    dump_nsensor(pole.voltageLN, "Voltage (LN)", ind)
    dump_nsensor(pole.current, "Current", ind)
    dump_nsensor(pole.peakCurrent, "Peak Current", ind)
    dump_nsensor(pole.activePower, "Active Power", ind)
    dump_nsensor(pole.reactivePower, "Reactive Power", ind)
    dump_nsensor(pole.apparentPower, "Apparent Power", ind)
    dump_nsensor(pole.powerFactor, "Power Factor", ind)
    dump_nsensor(pole.phaseAngle, "Phase Angle", ind)
    dump_nsensor(pole.displacementPowerFactor, "Displacement Power Factor", ind)
    dump_nsensor(pole.activeEnergy, "Active Energy", ind)
    dump_nsensor(pole.apparentEnergy, "Apparent Energy", ind)
end

local function dump_inlet(inlet, ind)
    local metadata = inlet:getMetaData()
    local rating = metadata.rating

    print()
    print(ind .. "Inlet " .. metadata.label .. ":")

    ind = ind .. "  "
    print(ind .. "Plug: " .. metadata.plugType)
    print(string.format(ind .. "Rating: %d-%d V, %d A",
			rating.minVoltage, rating.maxVoltage, rating.current))

    local sensors = inlet:getSensors()
    dump_nsensor(sensors.voltage, "Voltage", ind)
    dump_nsensor(sensors.current, "Current", ind)
    dump_nsensor(sensors.peakCurrent, "Peak Current", ind)
    dump_nsensor(sensors.residualCurrent, "Residual Current", ind)
    dump_nsensor(sensors.activePower, "Active Power", ind)
    dump_nsensor(sensors.apparentPower, "Apparent Power", ind)
    dump_nsensor(sensors.powerFactor, "Power Factor", ind)
    dump_nsensor(sensors.activeEnergy, "Active Energy", ind)
    dump_nsensor(sensors.apparentEnergy, "Apparent Energy", ind)
    dump_nsensor(sensors.unbalancedCurrent, "Unbalanced Current", ind)
    dump_nsensor(sensors.lineFrequency, "Line Frequency", ind)
    dump_nsensor(sensors.phaseAngle, "Phase Angle", ind)

    local map_powerquality = function(value)
	if value == pdumodel.PowerQualitySensor.STATE_NORMAL then return "normal" end
	if value == pdumodel.PowerQualitySensor.STATE_WARNING then return "warning" end
	if value == pdumodel.PowerQualitySensor.STATE_CRITICAL then return "critical" end
    end
    dump_ssensor(sensors.powerQuality, "Power Quality", ind, map_powerquality)

    local map_faultstate = function(value)
	return value == 0 and "OK" or "Fault"
    end
    dump_ssensor(sensors.surgeProtectorStatus, "Surge Protector", ind, map_faultstate)

    local map_rcmstate = function(value)
	if value == pdumodel.ResidualCurrentStateSensor.STATE_NORMAL then return "normal" end
	if value == pdumodel.ResidualCurrentStateSensor.STATE_WARNING then return "warning" end
	if value == pdumodel.ResidualCurrentStateSensor.STATE_CRITICAL then return "critical" end
	if value == pdumodel.ResidualCurrentStateSensor.STATE_SELFTEST then return "self-test" end
	if value == pdumodel.ResidualCurrentStateSensor.STATE_FAILURE then return "failure" end
    end
    dump_ssensor(sensors.residualCurrentStatus, "RCM State", ind, map_rcmstate)

    for i, pole in ipairs(inlet:getPoles()) do
	print(string.format(ind .. "Pole %d:", i))
	dump_pole(pole, ind .. "  ")
    end
end

local function dump_ts(ts, ind)
    local metadata = ts:getMetaData()

    print()
    print(ind .. "Transfer Switch " .. metadata.label .. ":")

    ind = ind .. "  "

    local settings = ts:getSettings()
    print(string.format(ind .. "Preferred Inlet: %d", settings.preferredSource))
    print(ind .. "Automatic Retransfer: " .. (settings.autoRetransfer and "yes" or "no"))
    print(ind .. "Require Phase Sync: " .. (settings.noRetransferIfPhaseFault and "no" or "yes"))
    print(string.format(ind .. "Retransfer Wait time: %d s", settings.autoRetransferWaitTime))

    local sensors = ts:getSensors()
    dump_ssensor(sensors.selectedSource, "Active Inlet", ind)

    local map_opstate = function(value)
	if value == pdumodel.TransferSwitch.OPERATIONAL_STATE_OFF then return "off" end
	if value == pdumodel.TransferSwitch.OPERATIONAL_STATE_NORMAL then return "normal" end
	if value == pdumodel.TransferSwitch.OPERATIONAL_STATE_STANDBY then return "standby" end
	if value == pdumodel.TransferSwitch.OPERATIONAL_STATE_NON_REDUNDANT then return "non-redundant" end
    end
    dump_ssensor(sensors.operationalState, "Operational State", ind, map_opstate)

    dump_nsensor(sensors.sourceVoltagePhaseSyncAngle, "Phase Sync Angle", ind)

    local function map_alarmstate(value)
	return value == 0 and "normal" or "alarmed"
    end
    dump_ssensor(sensors.overloadAlarm, "Overload", ind, map_alarmstate)
    dump_ssensor(sensors.phaseSyncAlarm, "Phase Sync Alarm", ind, map_alarmstate)

    local function map_tsfault(value)
	-- no bitwise operations in LUA :-/
	if value == 0 then return "none" end
	if value % 2 > 0 then return "inlet 1 short" end
	if (value / 2) % 2 > 0 then return "inlet 1 open" end
	if (value / 4) % 2 > 0 then return "inlet 2 short" end
	if (value / 8) % 2 > 0 then return "inlet 2 open" end
    end
    dump_ssensor(sensors.switchFault, "Switch Fault", ind, map_tsfault)
end

local function dump_ocp(ocp, ind)
    local metadata = ocp:getMetaData()

    print()
    print(ind .. "Overcurrent Protector " .. metadata.label .. ":")
    ind = ind .. "  "

    local sensors = ocp:getSensors()
    dump_nsensor(sensors.current, "Current", ind)
    dump_nsensor(sensors.peakCurrent, "Peak Current", ind)

    local map_tripstate = function(value)
	return value == 0 and "open" or "closed"
    end
    dump_ssensor(sensors.trip, "Status", ind, map_tripstate)

    for i, pole in ipairs(ocp:getPoles()) do
	print(string.format(ind .. "Pole %d:", i))
	dump_pole(pole, ind .. "  ")
    end
end

local function dump_outlet(outlet, ind)
    local metadata = outlet:getMetaData()

    print()
    print(ind .. "Outlet " .. metadata.label .. ":")

    ind = ind .. "  "
    print(ind .. "Receptacle: " .. metadata.receptacleType)

    local os = "n/a"
    local state = outlet:getState()
    if state.available then
	if state.switchOnInProgress then
	    os = "switching on"
	elseif state.cycleInProgress then
	    os = "cycling"
	elseif state.powerState == pdumodel.Outlet.PowerState.PS_OFF then
	    os = "off"
	else
	    os = "on"
	end
    end
    print(ind .. "State: " .. os)

    local sensors = outlet:getSensors()
    dump_nsensor(sensors.voltage, "Voltage", ind)
    dump_nsensor(sensors.current, "Current", ind)
    dump_nsensor(sensors.peakCurrent, "Peak Current", ind)
    dump_nsensor(sensors.maximumCurrent, "Maximum Current", ind)
    dump_nsensor(sensors.unbalancedCurrent, "Unbalanced Current", ind)
    dump_nsensor(sensors.activePower, "Active Power", ind)
    dump_nsensor(sensors.apparentPower, "Apparent Power", ind)
    dump_nsensor(sensors.powerFactor, "Power Factor", ind)
    dump_nsensor(sensors.activeEnergy, "Active Energy", ind)
    dump_nsensor(sensors.apparentEnergy, "Apparent Energy", ind)
    dump_nsensor(sensors.phaseAngle, "Phase Angle", ind)
    dump_nsensor(sensors.lineFrequency, "Line Frequency", ind)

    local inlet, ocps, poles = outlet:getIOP()
    for i, pole in ipairs(poles) do
	print(string.format(ind .. "Pole %d:", i))
	dump_pole(pole, ind .. "  ")
    end
end

local function dump_pdu(pdu)
    local metadata = pdu:getMetaData()
    local rating = metadata.nameplate.rating

    print("Model: " .. metadata.nameplate.model)
    print("Serial Number: " .. metadata.nameplate.serialNumber)
    print(string.format("Rating: %s, %s, %s, %s",
			rating.voltage, rating.frequency, rating.current, rating.power))

    for _, inlet in ipairs(pdu:getInlets()) do
	dump_inlet(inlet, "")
    end
    for _, ts in ipairs(pdu:getTransferSwitches()) do
	dump_ts(ts, "")
    end
    for _, ocp in ipairs(pdu:getOverCurrentProtectors()) do
	dump_ocp(ocp, "")
    end
    for _, outlet in ipairs(pdu:getOutlets()) do
	dump_outlet(outlet, "")
    end
end

dump_pdu(pdumodel.Pdu:getDefault())
