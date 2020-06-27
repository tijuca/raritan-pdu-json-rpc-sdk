-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- the enumeration has a wrong value => a failure is thrown by the system 

require "Pdu"

-- get pdu object, outlets vector and 3rd outlet
local pdu = pdumodel.Pdu:getDefault()
local out3 = pdu:getOutlets()[3]

-- create the settings table by hand and a enumeration has a wrong value (out of range)
wrongSettings                  = {}
wrongSettings.name             = "does it work???"
wrongSettings.cycleDelay       = 11
wrongSettings.nonCritical      = false
wrongSettings.startupState     = 4  -- enum 0 .. 3 => 4 is over the enumeration
wrongSettings.usePduCycleDelay = true
wrongSettings.sequenceDelay    = 0

-- failure from the system
err, errMsg = pcall(out3.setSettings, out3, wrongSettings)
if err == false then
  print( "something is wrong ... " .. errMsg)
end
