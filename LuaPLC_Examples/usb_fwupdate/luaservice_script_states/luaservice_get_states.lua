-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Iterates through all uploaded Lua scripts and prints it actually state out.
-- The output will be written back to usb stick

require "LuaService"

-- get the LuaService Manager
local lsManager = luaservice.Manager:getDefault()

-- get all script names
local scriptNames = lsManager:getScriptNames()

-- helper functions to convert script state to a string
function strFromState(_s)
  local ExecState = luaservice.ScriptState.ExecState
  if _s == ExecState.STAT_NEW then
    return "new"
  elseif _s == ExecState.STAT_RUNNING then
    return "running"
  elseif _s == ExecState.STAT_TERMINATED then
    return "terminated"
  elseif _s == ExecState.STAT_RESTARTING then
    return "restarting"
  else
    return "unkown"
  end
end

-- iterate through all scripts, get the state and print the state out
for k,v in ipairs(scriptNames) do
  local err, state = lsManager:getScriptState(v)
  print(k .. ": " .. v .. " - state: " .. strFromState(state.execState))
end
