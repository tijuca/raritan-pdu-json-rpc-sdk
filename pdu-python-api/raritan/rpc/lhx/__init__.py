# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This is an auto-generated file.

#
# Section generated by IdlC from "LhxSupport.idl"
#

import raritan.rpc
from raritan.rpc import Interface, Structure, ValueObject, Enumeration, typecheck, DecodeException

# interface
class Support(Interface):
    idlType = "lhx.Support:1.0.0"

    class _setEnabled(Interface.Method):
        name = 'setEnabled'

        @staticmethod
        def encode(enabled):
            typecheck.is_bool(enabled, AssertionError)
            args = {}
            args['enabled'] = enabled
            return args

        @staticmethod
        def decode(rsp, agent):
            return None

    class _isEnabled(Interface.Method):
        name = 'isEnabled'

        @staticmethod
        def encode():
            args = {}
            return args

        @staticmethod
        def decode(rsp, agent):
            _ret_ = rsp['_ret_']
            typecheck.is_bool(_ret_, DecodeException)
            return _ret_
    def __init__(self, target, agent):
        super(Support, self).__init__(target, agent)
        self.setEnabled = Support._setEnabled(self)
        self.isEnabled = Support._isEnabled(self)
