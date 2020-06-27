-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- This script enables or disables event rules.
--
-- Use script arguments to specify the rules to be enabled or disabled. The
-- argument key is the rule name, the value must be either "enable" or
-- "disable" (all lower case).

-- This script can be used to activate or deactivate rules by schedule:
--
-- (1) Deploy this script on your PDU
-- (2) Create a new event action:
--     - Action type: Start/stop Lua script
--     - Operation: Start Script
--     - Script: select this script
--     - Arguments:
--       Key: Name of the rule to be enabled or disabled
--       Value: "enable" or "disable"
-- (3) Create a new scheduled action:
--     - Define a schedule as required
--     - Select the created action

require "EventEngine"

local event_engine = event.Engine:getDefault()

for _, rule in ipairs(event_engine:listRules()) do
    local arg = ARGS[rule.name]
    if arg == "enable" then
	print(os.date("%c") .. ": Enabling rule '" .. rule.name .. "'")
	local rc = event_engine:enableRule(rule.id)
	if rc ~= 0 then
	    print("ERROR: Failure enabling rule, return code: " .. rc)
	end
    elseif arg == "disable" then
	print(os.date("%c") .. ": Disabling rule '" .. rule.name .. "'")
	local rc = event_engine:disableRule(rule.id)
	if rc ~= 0 then
	    print("ERROR: Failure disabling rule, return code: " .. rc)
	end
    end
end
