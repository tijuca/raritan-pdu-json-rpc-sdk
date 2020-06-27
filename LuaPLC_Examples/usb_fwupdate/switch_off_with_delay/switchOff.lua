-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

require 'Pdu'

-- switch an outlet off
-- argument "outlet" can be the number/label or the name of the outlet
-- argument "delay" is the delay in seconds (1 .. 120, default 60). This arguments is optional

-- two curl examples to start the script from the terminal
--
-- curl -skd '{ "jsonrpc": "2.0", "method": "startScriptWithArgs", "params": { "name": "switchOff", "arguments": [{"key":"outlet", "value": "10"}, {"key":"delay", "value":"10"}] }, "id": 12 }' https://admin:raritan@192.168.7.167/luaservice
--
-- curl -skd '{ "jsonrpc": "2.0", "method": "startScriptWithArgs", "params": { "name": "switchOff", "arguments": [{"key":"outlet", "value": "myoutlet"}, {"key":"delay", "value":"10"}] }, "id": 13 }' https://admin:raritan@192.168.7.167/luaservice

-- some globals
pdu = pdumodel.Pdu:getDefault()
outlets = pdu:getOutlets()
beeper = pdu:getBeeper()
outlet = nil
delay = 60 -- default timeout

paramOutlet = ARGS["outlet"]
paramDelay = ARGS["delay"]


-- function: exit and signal an error
function exitWithErrorMessage(errMsg)
  print(errMsg)
  beeper:activate(true, errMsg, 500)
  sleep(1)
  beeper:activate(true, "- - - exit with failure", 500)
  os.exit(1)
end

-- function: find outlet by name
function findOutletByName(name)
  for position=1,#outlets do
  	local settings = outlets[position]:getSettings()
    if name == settings.name then
      return outlets[position]
    end
  end
  return nil
end


-- find the outlet
local number = tonumber(paramOutlet)
if number ~= nil then
  outlet = outlets[number]
elseif tostring(paramOutlet ~= nil) then
  outlet = findOutletByName(paramOutlet)
end

if (outlet == nil) then
  if paramOutlet == nil then
  	paramOutlet = "nil"
  end
  exitWithErrorMessage("outlet with identifier '" .. paramOutlet .. "' not found")
end

-- default delay or user/parameter defined delay
number = tonumber(paramDelay)
if number ~= nil and number > 0 and number < 120 then
  delay = number  
end

print("switch off outlet '" .. paramOutlet .. "' after '" .. delay .. "' seconds")

-- delay
sleep(delay)

-- switch outlet off
local status = outlet:setPowerState(pdumodel.Outlet.PowerState.PS_OFF)

-- returned setPowerState a failure?
if status == 1 then
  exitWithErrorMessage("outlet ist not switchable")
elseif status == 2 then
  exitWithErrorMessage("switching is not possible due to load shedding")
elseif status == 3 then
  exitWithErrorMessage("outlet is disabled")
end

-- exit and signal success
beeper:activate(true, "finished switch off", 100)
os.exit(0)
