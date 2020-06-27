-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

-- This examples shows how to use the exit handler.
--
-- The script will exit normally after 10 seconds without calling the
-- exit handler. The function 'ExitHandler' is called in case the
-- script is forcibly stopped.

require "Pdu"

-- To register an exit handler, just define a global function
-- named 'ExitHandler'.
function ExitHandler()
    -- If you use API objects in the exit handler, it's best to create new
    -- ones instead of relying on global objects. The exit handler can be
    -- invoked at any time, and it's not guaranteed that any global objects
    -- are yet/still available.
    local pdu = pdumodel.Pdu:getDefault()
    local beeper = pdu:getBeeper()

    -- Activate the beeper for 100 ms
    beeper:activate(true, "Lua script exit handler called", 100)

    print("The script has been terminated.")
end

for i = 1, 10 do
    print("Running ...")
    sleep(1)
end
print("Exited normally.")
