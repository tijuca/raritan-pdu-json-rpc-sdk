-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Remove all installed scripts from pdu.

require "LuaService"
require "Pdu"

-- get the LuaService Manager
local lsManager = luaservice.Manager:getDefault()

-- get all script names
local scriptNames = lsManager:getScriptNames()

-- get the internal beeper
local pdu = pdumodel.Pdu:getDefault()
local beeper = pdu:getBeeper()

-- helper function to convert an error number to text
function strFromError(_e)
  local Mgr = luaservice.Manager
  if _e ==  Mgr.NO_ERROR then
    return "no error"
  elseif _e ==  Mgr.ERR_INVALID_NAME then
    return "err invalid name"
  elseif _e ==  Mgr.ERR_NO_SUCH_SCRIPT then
    return "err no such script"
  elseif _e ==  Mgr.ERR_MAX_SCRIPT_NUMBERS_EXCEEDED then
    return "err max script numbers exceeded"
  elseif _e ==  Mgr.ERR_MAX_SCRIPT_SIZE_EXCEEDED then
    return "err max script size exceeded"
  elseif _e ==  Mgr.ERR_MAX_ALL_SCRIPT_SIZE_EXCEEDED then
    return "err max all script size exceeded"
    elseif _e ==  Mgr.ERR_NOT_TERMINATED then
    return "err not terminated"
  elseif _e ==  Mgr.ERR_NOT_RUNNING then
    return "err not running"
  elseif _e ==  Mgr.ERR_INVALID_ADDR then
    return "err invalid address"
  else
    return "unknown"
  end
end

-- function to delete script
function deleteScriptWithName(_name)
  local error = lsManager:deleteScript(_name)
  print("Deleting script: " .. _name .. " returns: " .. strFromError(error))
  -- if the script was running...
  if error == luaservice.Manager.ERR_NOT_TERMINATED then
    -- ...then terminate it, wait a short time and...
    local error = lsManager:terminateScript(_name)
    sleep(0.5)
    -- ...call this function again (recursion)
    print("Terminating script:" .. _name .. " returns: " .. strFromError(error))
    deleteScriptWithName(_name)
  end
end

-- iterate through all scripts and get the output
for number, name in ipairs(scriptNames) do
  deleteScriptWithName(name)
end

-- to indicate the end a short beep
beeper:activate(true, "all scripts removed", 100)
