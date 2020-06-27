-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- shows how to handle failure by example

require "Pdu"

-- get pdu object
local pdu = pdumodel.Pdu:getDefault()
-- get outlets vector and get outlet 99
local outlets = pdu:getOutlets()
-- index is not valid / not in range => nil will returned
local out99   = outlets[99]

-- test if outlet 99 exists
if out99 == nil then
	print("outlet 99 is nil - does not exist")
else
	print("outlet 99 is not nil - exists")
end

-- To test if out99 is not nil assert() is used here. If assert fails execution
-- of the script will stop.
assert(out99)

print("this message will not print if assert() fails")
