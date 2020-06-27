-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Uploads a Lua script and set default arguments. After uploading the
-- script starts (with overwritting default arguments).

require "LuaService"

-- the name of the Lua script to upload
local newScriptFileName = "switch_outlets.lua"
-- the name used to reference on LuaService
local newScriptName = "outletSwitcher"

-- set defaultArgs
local defaultArgs = {}
-- sets powerstate of outlet 1, 3, 5 and 7 to off
defaultArgs["offOutlets"] = "1 3 5 7"
-- sets powerstate of outlet 2, 4 and 6 to on
defaultArgs["onOutlets"]  = "2 4 6"

-- newScriptOptions will contain our script options
local newScriptOptions = {}
newScriptOptions.defaultArgs = defaultArgs
newScriptOptions.autoRestart = false
newScriptOptions.autoStart   = false


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

-- open a file and read its content
local newFile   = io.open(newScriptFileName)
local newScript = newFile:read("*a")

-- get the LuaService manager
local lsManager = luaservice.Manager:getDefault()

-- upload the new script
local status = lsManager:setScript(newScriptName, newScript, newScriptOptions)
print("set script " .. newScriptName .. " returns: " .. strFromError(status))

-- start the script with new default args
local defaultArgs = {}
defaultArgs["offOutlets"] = "12 14 16"
defaultArgs["onOutlets"]  = "13 15 17"
local status = lsManager:startScriptWithArgs(newScriptName, defaultArgs)
print("start script " .. newScriptName .. " returns: " .. strFromError(status))
