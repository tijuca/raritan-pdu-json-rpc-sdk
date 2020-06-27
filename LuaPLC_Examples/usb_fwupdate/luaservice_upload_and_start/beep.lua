-- SPDX-License-Identifier: BSD-3-Clause
--
-- Copyright 2016 Raritan Inc. All rights reserved.

require "Pdu"

-- beeps three times

local pdu = pdumodel.Pdu:getDefault()

local beeper = pdu:getBeeper()
beeper:activate(true, "make noise 100", 100)

sleep(1)

local beeper = pdu:getBeeper()
beeper:activate(true, "make noise 200", 200)

sleep(2)

local beeper = pdu:getBeeper()
beeper:activate(true, "make noise 400", 400)

