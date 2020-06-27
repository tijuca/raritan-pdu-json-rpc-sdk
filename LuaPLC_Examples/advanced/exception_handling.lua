-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- This example shows how to catch exceptions using the Lua builtin
-- function 'pcall()'.

require "Pdu"

-- Acquire the single instance of the Pdu interface
local pdu = pdumodel.Pdu:getDefault()

-- As defined in IDL, Pdu.setSettings() expects a Pdu.Settings structure
-- as parameter. Passing a string instead will result in an exception.
valid_settings = pdu:getSettings()
invalid_settings = "Invalid Settings"

local err, result

-- Example 1: Pass the method to be called, the object and the method
--   parameters as separate arguments to pcall(). Note the missing parentheses
--   after setSettings!

print("Example 1a (valid settings):")
success, result = pcall(pdu.setSettings, pdu, valid_settings)
if success then
    print("- No exception, return code: " .. result)
else
    print("- Exception caught, error message: " .. result)
end

print("Example 1b (invalid settings):")
success, result = pcall(pdu.setSettings, pdu, invalid_settings)
if success then
    print("- No exception, return code: " .. result)
else
    print("- Exception caught, error message: " .. result)
end

-- Example 2: Wrap the method call in an anonymous function. This time the
--   syntax is identical to an unprotected method call.

print("Example 2a (valid settings):")
success, result = pcall(function() return pdu:setSettings(valid_settings) end)
if success then
    print("- No exception, return code: " .. result)
else
    print("- Exception caught, error message: " .. result)
end

print("Example 2b (invalid settings):")
success, result = pcall(function() return pdu:setSettings(invalid_settings) end)
if success then
    print("- No exception, return code: " .. result)
else
    print("- Exception caught, error message: " .. result)
end

-- Example 3: THIS WILL NOT WORK! Example 3a will report an exception even
--   though the actual method call succeeded. Example 3b will fail to catch
--   the exception and terminate the script!

print("Example 3a (incorrect use of pcall):")
success, result = pcall(pdu:setSettings(valid_settings))
if success then
    print("- No exception, return code: " .. result)
else
    print("- Exception caught, error message: " .. result)
end

print("Example 3b (incorrect use of pcall; this will crash the script!):")
success, result = pcall(pdu:setSettings(invalid_settings))
if success then
    print("- No exception, return code: " .. result)
else
    print("- Exception caught, error message: " .. result)
end
