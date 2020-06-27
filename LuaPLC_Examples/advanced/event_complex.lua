-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2015 Raritan Inc. All rights reserved.

-- switching outlets o12, o13 and o14 on/off dependent on temperature t1, t2 and t3 and
-- current and state from outlet o8
--
-- decision matrix
--
-- 0 := false
-- 1 := true
-- X := false | true
--
-- init => t1 = 0, t2 = 0, t3 = 0, o8c = 0, o8s = 0
-- t1 temperature > 35 °C => t1 = 1
-- t1 temperature < 30 °C => t1 = 0
-- t2 temperature > 40 °C => t2 = 1
-- t2 temperature < 35 °C => t2 = 0
-- t3 temperature > 45 °C => t3 = 1
-- t3 temperature < 20 °C => t3 = 0
-- o8 current >= 0.1 A => o8c = 1
-- o8 current <  0.1 A => 08c = 0
-- o8 state == on  => o8s = 1
-- o8 state == off => o8s = 0
--
-- t1  t2  t3  o8c o8s | o12 o13 o14
-- --------------------+------------
--  0   0   0   0   0  |  0   0   1
--  1   0   0   X   X  |  1   0   0
--  0   0   1   X   X  |  0   1   0
--  1   0   1   X   X  |  1   1   0
--  0   1   0   0   1  |  0   0   1
--  0   1   0   1   1  |  1   0   1
-- other states        |  0   0   0
--

require "Pdu"
require "EventService"


-- print info function
function printInfo(...)
  -- set true/false to print out information or not
  if true then
	  print("INFO: ", (...))
  end
end


local function getTemperatureDevice(devName)
  local p  = pdumodel.Pdu:getDefault()
  local dm = p:getPeripheralDeviceManager()
  local ds = dm:getDeviceSlots()
  for i=1,#ds do
    if ds[i]:getSettings().name == devName then
      dev = sensors.NumericSensor:cast( ds[i]:getDevice().device )
      break
    end
  end
  return dev
end


local eval = {}
eval.t1  = false
eval.t2  = false
eval.t3  = false
eval.o8c = false
eval.o8s = false


function evaluateNumericSensor(sens)
  if sens.newReading.valid == true then
   local src = sensors.NumericSensor:cast(sens.source)
   printInfo("reading value " .. sens.newReading.value .. " from sensor " .. src:getId())
   -- devT1
    if src:equals(devT1) then
      if sens.newReading.value > 35.0 then
        eval.t1 = true
      elseif sens.newReading.value < 30.0 then
        eval.t1 = false
      end

    -- devT2
    elseif src:equals(devT2) then
			if sens.newReading.value > 40.0 then
				eval.t2 = true
			elseif sens.newReading.value < 35.0 then
				eval.t2 = false
			end

    -- devT3
    elseif src:equals(devT3) then
      if sens.newReading.value > 45.0 then
				eval.t3 = true
			elseif sens.newReading.value < 20.0 then
				eval.t3 = false
			end

    -- devO8C
    elseif src:equals(devO8C) then
	    if sens.newReading.value >= 0.1 then
				eval.o8c = true
			elseif sens.newReading.value < 0.1 then
				eval.o8c = false
			end

    else
      do  return  end
    end

	  processEval()

  end
end


function evaluateStateSensor(sens)
	printInfo("reading state " .. sens.newState.value .. " from sensor " .. sens.source:getId())
  if sens.newState.available == true then

   local src = sensors.StateSensor:cast(sens.source)
   -- devO8S
    if src:equals(devO8S) then
      if sens.newState.value == 0 then
        eval.o8s = false
      else
        eval.o8s = true
      end

    else
      do  return  end
    end

	  processEval()

  end
end


function processEval()
  printInfo("processing ...")
  printInfo("t1\tt2\tt3\to8c\to8s")
  printInfo(tostring(eval.t1) .. "\t" .. tostring(eval.t2) .. "\t" .. tostring(eval.t3) .. "\t" ..  tostring(eval.o8c) .. "\t" .. tostring(eval.o8s))
-- t1  t2  t3  o8c o8s | o12 o13 o14
-- --------------------+------------
--  0   0   0   0   0  |          1
--  0   1   0   X   1  |          1
  if eval.t1 == false and eval.t2 == false and eval.t3 == false and eval.o8c == false and eval.o8s == false
  or eval.t1 == false and eval.t2 == true  and eval.t3 == false and                       eval.o8s == true  then
	  printInfo("out 14 switch on")
	  out14:setPowerState(pwState.PS_ON)
  else
    printInfo("out 14 switch off")
    out14:setPowerState(pwState.PS_OFF)
  end

--  X   0   1   X   X  |      1
  if eval.t2 == false and eval.t3 == true  then
	  printInfo("out 13 switch on")
	  out13:setPowerState(pwState.PS_ON)
  else
    printInfo("out 13 switch off")
    out13:setPowerState(pwState.PS_OFF)
  end

--  1   0   X   X   X  |  1
--  0   1   0   1   1  |  1
  if eval.t1 == false and eval.t2 == true  and eval.t3 == false and eval.o8c == true  and eval.o8s == true
  or eval.t1 == true  and eval.t2 == false then
	  printInfo("out 12 switch on")
	  out12:setPowerState(pwState.PS_ON)
  else
    printInfo("out 12 switch off")
    out12:setPowerState(pwState.PS_OFF)
  end

end


local pdu     = pdumodel.Pdu:getDefault()
local outlets = pdu:getOutlets()
local out8    = outlets[8]
out12   = outlets[12]
out13   = outlets[13]
out14   = outlets[14]
pwState = pdumodel.Outlet.PowerState
devT1   = getTemperatureDevice("Temperature 1")
devT2   = getTemperatureDevice("Temperature 2")
devT3   = getTemperatureDevice("Temperature 3")
local o8Sens  = out8:getSensors()
devO8C  = o8Sens.current
devO8S  = o8Sens.outletState
local outlets = nil
local o8Sens  = nil
local out8    = nil
local pdu     = nil

assert(devT1)
assert(devT2)
assert(devT3)
assert(devO8C)
assert(devO8S)

local evtSrv = event.Service:getDefault()
local ch     = evtSrv:createChannel()
ch:demandEvent(sensors.NumericSensor.ReadingChangedEvent.TYPE_ID, devT1 )
ch:demandEvent(sensors.NumericSensor.ReadingChangedEvent.TYPE_ID, devT2 )
ch:demandEvent(sensors.NumericSensor.ReadingChangedEvent.TYPE_ID, devT3 )
ch:demandEvent(sensors.NumericSensor.ReadingChangedEvent.TYPE_ID, devO8C)
ch:demandEvent(sensors.StateSensor.StateChangedEvent.TYPE_ID    , devO8S)

printInfo("- - -   get events - - -")

while true do
  local more, vec = ch:pollEvents() -- more will be ignored, pollEvents() is blocking
  local len = #vec
  printInfo("received " .. len .. " events")
  if len > 0 then
    for i=1,len do
	    printInfo("got data for " .. vec[i].source:getId() .. " (" .. vec[i].TYPE_ID .. ")")

      if vec[i].TYPE_ID == sensors.NumericSensor.ReadingChangedEvent.TYPE_ID then
        evaluateNumericSensor(vec[i])

      elseif vec[i].TYPE_ID == sensors.StateSensor.StateChangedEvent.TYPE_ID then
	      evaluateStateSensor(vec[i])
      end
  end

  end
end
