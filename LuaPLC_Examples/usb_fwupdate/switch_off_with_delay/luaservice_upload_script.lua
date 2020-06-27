-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Uploads a script to switch off outlets after delay. You specify the outlet
-- and delay time at the script arguments.

require "LuaService"
require "Pdu"

-- the name of the Lua script to upload
local newScriptFileName = "switchOff.lua"
-- the name used to reference on LuaService
local newScriptName = "switchOff"

-- newScriptOptions will contain our script options
local newScriptOptions = {}
-- at least there must be a empty table called defaultArgs
newScriptOptions.defaultArgs = {}
newScriptOptions.autoRestart = false
newScriptOptions.autoStart   = false

-- open a file and read its content
local newFile   = io.open(newScriptFileName)
local newScript = newFile:read("*a")

-- get the LuaService manager
local lsManager = luaservice.Manager:getDefault()

-- upload the new script
local errorCode = lsManager:setScript(newScriptName, newScript, newScriptOptions)
print("uploading script '" .. newScriptName .. "' returns with code: " .. errorCode)

-- exit and signal success
local pdu = pdumodel.Pdu:getDefault()
local beeper = pdu:getBeeper()
beeper:activate(true, "finished uploading", 100)
os.exit(0)
