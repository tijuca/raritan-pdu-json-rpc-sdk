-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

require "Pdu"

local pdu = pdumodel.Pdu:getDefault()
local outlets = pdu:getOutlets()

local offOutletsParam = ARGS['offOutlets']
local onOutletsParam = ARGS['onOutlets']

-- prints out the error message an stops the script with an error code of 1
function exitWithErrorMessage(errorMessage)
  print("ERROR: " .. errorMessage)
  os.exit(1)
end

-- splits a list like "1 3 7 10" to a table of "1", "3", "7", "10"
function split(inputString)
  return string.gmatch(inputString, "%S+")
end

-- switch one outlet
function switchOutlet(number, powerstate)
  outlet = outlets[number]
  if outlet ~= nil then
    -- if there is an outlet switch it to powerstate
    local status = outlet:setPowerState(powerstate)
    -- break script if there is an failure
    if status == 1 then
      exitWithErrorMessage("outlet ist not switchable")
    elseif status == 2 then
      exitWithErrorMessage("switching is not possible due to load shedding")
    elseif status == 3 then
      exitWithErrorMessage("outlet is disabled")
    else
      print("switched outlet: " .. number)
    end
  else
    exitWithErrorMessage('cannot find outlet with number ' .. number)    
  end
end

-- switch a group of outlets
function switchOutletGroup(outletGroup, powerstate)
  -- if outletGroup is a string go further
  if type(outletGroup) == "string" then
    -- for all outlets
    local switchOutlets = split(outletGroup)
    for swOut in switchOutlets do
      -- convert the string to a number
      local number = tonumber(swOut)
      if number ~= nil then
        switchOutlet(number, powerstate)
      else
        exitWithErrorMessage('cannot convert ' .. swOut .. " to number")
      end
    end
  -- else do nothing
  else
    print("note: not a string")
  end
end

switchOutletGroup(offOutletsParam, pdumodel.Outlet.PowerState.PS_OFF)
switchOutletGroup(onOutletsParam, pdumodel.Outlet.PowerState.PS_ON)
