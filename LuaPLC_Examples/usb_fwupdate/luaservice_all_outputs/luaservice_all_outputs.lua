-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- Prints the output of all scripts to the log file.

require "LuaService"
require "Pdu"

-- get the LuaService Manager
local lsManager = luaservice.Manager:getDefault()

-- get all script names
local scriptNames = lsManager:getScriptNames()

-- get the internal beeper
local pdu = pdumodel.Pdu:getDefault()
local beeper = pdu:getBeeper()

-- iterate through all scripts and get the output
for number, name in ipairs(scriptNames) do
  print("## SCRIPT " .. number .. ": " .. name)
  -- to get the output for first time set nextAddress to zero
  nextAddress = 0
  repeat
    error, startAddress, nextAddress, output, isMoreAvailable = lsManager:getScriptOutput(name, nextAddress)
    -- no newline after writing is complete
    io.write(output)
    io.flush()
  -- repeat this until no more output is available
  until isMoreAvailable == false
  -- empty line (nothing + newline)
  print("")
end

-- to indicate the end a one second beep
beeper:activate(true, "getting output complete", 1000)
print("## no more output")
