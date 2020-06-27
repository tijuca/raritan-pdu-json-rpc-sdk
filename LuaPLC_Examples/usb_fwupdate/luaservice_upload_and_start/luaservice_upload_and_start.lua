-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Uploads a Lua script. After uploading the new script will be started. You
-- should hear an alarm.

require "LuaService"

-- the name of the Lua script to upload
local newScriptFileName = "beep.lua"
-- the name used to reference on LuaService
local newScriptName = "make_noise"

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

-- upload the new script and start the new script
lsManager:setScript(newScriptName, newScript, newScriptOptions)
lsManager:startScript(newScriptName)
