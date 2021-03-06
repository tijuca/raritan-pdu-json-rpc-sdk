# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This is an auto-generated file.

#
# Section generated by IdlC from "CascadeManager.idl"
#

import raritan.rpc
from raritan.rpc import Interface, Structure, ValueObject, Enumeration, typecheck, DecodeException
import raritan.rpc.cascading

import raritan.rpc.event

import raritan.rpc.idl


# interface
class CascadeManager(Interface):
    idlType = "cascading.CascadeManager:1.0.0"

    NO_ERROR = 0

    ERR_INVALID_PARAM = 1

    ERR_UNSUPPORTED_ON_MASTER = 2

    ERR_UNSUPPORTED_ON_LINK_UNIT = 3

    ERR_LINK_ID_IN_USE = 4

    ERR_HOST_IN_USE = 5

    ERR_LINK_UNIT_UNREACHABLE = 6

    ERR_LINK_UNIT_ACCESS_DENIED = 7

    ERR_LINK_UNIT_REFUSED = 8

    ERR_UNIT_BUSY = 9

    ERR_NOT_SUPPORTED = 10

    ERR_PASSWORD_CHANGE_REQUIRED = 11

    ERR_PASSWORD_POLICY = 12

    # enumeration
    class Role(Enumeration):
        idlType = "cascading.CascadeManager.Role:1.0.0"
        values = ["STANDALONE", "MASTER", "LINK_UNIT"]

    Role.STANDALONE = Role(0)
    Role.MASTER = Role(1)
    Role.LINK_UNIT = Role(2)

    # enumeration
    class LinkUnitStatus(Enumeration):
        idlType = "cascading.CascadeManager.LinkUnitStatus:1.0.0"
        values = ["UNKNOWN", "OK", "UNREACHABLE", "ACCESS_DENIED", "FIRMWARE_UPDATE"]

    LinkUnitStatus.UNKNOWN = LinkUnitStatus(0)
    LinkUnitStatus.OK = LinkUnitStatus(1)
    LinkUnitStatus.UNREACHABLE = LinkUnitStatus(2)
    LinkUnitStatus.ACCESS_DENIED = LinkUnitStatus(3)
    LinkUnitStatus.FIRMWARE_UPDATE = LinkUnitStatus(4)
    LinkUnitStatus._fallback = LinkUnitStatus.UNKNOWN

    # structure
    class LinkUnit(Structure):
        idlType = "cascading.CascadeManager.LinkUnit:1.0.0"
        elements = ["host", "status"]

        def __init__(self, host, status):
            typecheck.is_string(host, AssertionError)
            typecheck.is_enum(status, raritan.rpc.cascading.CascadeManager.LinkUnitStatus, AssertionError)

            self.host = host
            self.status = status

        @classmethod
        def decode(cls, json, agent):
            obj = cls(
                host = json['host'],
                status = raritan.rpc.cascading.CascadeManager.LinkUnitStatus.decode(json['status']),
            )
            return obj

        def encode(self):
            json = {}
            json['host'] = self.host
            json['status'] = raritan.rpc.cascading.CascadeManager.LinkUnitStatus.encode(self.status)
            return json

    # structure
    class Status(Structure):
        idlType = "cascading.CascadeManager.Status:1.0.0"
        elements = ["role", "master", "linkUnits"]

        def __init__(self, role, master, linkUnits):
            typecheck.is_enum(role, raritan.rpc.cascading.CascadeManager.Role, AssertionError)
            typecheck.is_string(master, AssertionError)

            self.role = role
            self.master = master
            self.linkUnits = linkUnits

        @classmethod
        def decode(cls, json, agent):
            obj = cls(
                role = raritan.rpc.cascading.CascadeManager.Role.decode(json['role']),
                master = json['master'],
                linkUnits = dict([(
                    elem['key'],
                    raritan.rpc.cascading.CascadeManager.LinkUnit.decode(elem['value'], agent))
                    for elem in json['linkUnits']]),
            )
            return obj

        def encode(self):
            json = {}
            json['role'] = raritan.rpc.cascading.CascadeManager.Role.encode(self.role)
            json['master'] = self.master
            json['linkUnits'] = [dict(
                key = k,
                value = raritan.rpc.cascading.CascadeManager.LinkUnit.encode(v))
                for k, v in self.linkUnits.items()]
            return json

    # value object
    class RoleChangedEvent(raritan.rpc.idl.Event):
        idlType = "cascading.CascadeManager.RoleChangedEvent:1.0.0"

        def __init__(self, oldRole, newRole, master, source):
            super(raritan.rpc.cascading.CascadeManager.RoleChangedEvent, self).__init__(source)
            typecheck.is_enum(oldRole, raritan.rpc.cascading.CascadeManager.Role, AssertionError)
            typecheck.is_enum(newRole, raritan.rpc.cascading.CascadeManager.Role, AssertionError)
            typecheck.is_string(master, AssertionError)

            self.oldRole = oldRole
            self.newRole = newRole
            self.master = master

        def encode(self):
            json = super(raritan.rpc.cascading.CascadeManager.RoleChangedEvent, self).encode()
            json['oldRole'] = raritan.rpc.cascading.CascadeManager.Role.encode(self.oldRole)
            json['newRole'] = raritan.rpc.cascading.CascadeManager.Role.encode(self.newRole)
            json['master'] = self.master
            return json

        @classmethod
        def decode(cls, json, agent):
            obj = cls(
                oldRole = raritan.rpc.cascading.CascadeManager.Role.decode(json['oldRole']),
                newRole = raritan.rpc.cascading.CascadeManager.Role.decode(json['newRole']),
                master = json['master'],
                # for idl.Event
                source = Interface.decode(json['source'], agent),
            )
            return obj

        def listElements(self):
            elements = ["oldRole", "newRole", "master"]
            elements = elements + super(raritan.rpc.cascading.CascadeManager.RoleChangedEvent, self).listElements()
            return elements

    # value object
    class LinkUnitAddedEvent(raritan.rpc.event.UserEvent):
        idlType = "cascading.CascadeManager.LinkUnitAddedEvent:1.0.0"

        def __init__(self, linkId, host, actUserName, actIpAddr, source):
            super(raritan.rpc.cascading.CascadeManager.LinkUnitAddedEvent, self).__init__(actUserName, actIpAddr, source)
            typecheck.is_int(linkId, AssertionError)
            typecheck.is_string(host, AssertionError)

            self.linkId = linkId
            self.host = host

        def encode(self):
            json = super(raritan.rpc.cascading.CascadeManager.LinkUnitAddedEvent, self).encode()
            json['linkId'] = self.linkId
            json['host'] = self.host
            return json

        @classmethod
        def decode(cls, json, agent):
            obj = cls(
                linkId = json['linkId'],
                host = json['host'],
                # for event.UserEvent
                actUserName = json['actUserName'],
                actIpAddr = json['actIpAddr'],
                # for idl.Event
                source = Interface.decode(json['source'], agent),
            )
            return obj

        def listElements(self):
            elements = ["linkId", "host"]
            elements = elements + super(raritan.rpc.cascading.CascadeManager.LinkUnitAddedEvent, self).listElements()
            return elements

    # value object
    class LinkUnitReleasedEvent(raritan.rpc.event.UserEvent):
        idlType = "cascading.CascadeManager.LinkUnitReleasedEvent:1.0.0"

        def __init__(self, linkId, host, actUserName, actIpAddr, source):
            super(raritan.rpc.cascading.CascadeManager.LinkUnitReleasedEvent, self).__init__(actUserName, actIpAddr, source)
            typecheck.is_int(linkId, AssertionError)
            typecheck.is_string(host, AssertionError)

            self.linkId = linkId
            self.host = host

        def encode(self):
            json = super(raritan.rpc.cascading.CascadeManager.LinkUnitReleasedEvent, self).encode()
            json['linkId'] = self.linkId
            json['host'] = self.host
            return json

        @classmethod
        def decode(cls, json, agent):
            obj = cls(
                linkId = json['linkId'],
                host = json['host'],
                # for event.UserEvent
                actUserName = json['actUserName'],
                actIpAddr = json['actIpAddr'],
                # for idl.Event
                source = Interface.decode(json['source'], agent),
            )
            return obj

        def listElements(self):
            elements = ["linkId", "host"]
            elements = elements + super(raritan.rpc.cascading.CascadeManager.LinkUnitReleasedEvent, self).listElements()
            return elements

    # value object
    class LinkUnitStatusChangedEvent(raritan.rpc.idl.Event):
        idlType = "cascading.CascadeManager.LinkUnitStatusChangedEvent:1.0.0"

        def __init__(self, linkId, host, oldStatus, newStatus, source):
            super(raritan.rpc.cascading.CascadeManager.LinkUnitStatusChangedEvent, self).__init__(source)
            typecheck.is_int(linkId, AssertionError)
            typecheck.is_string(host, AssertionError)
            typecheck.is_enum(oldStatus, raritan.rpc.cascading.CascadeManager.LinkUnitStatus, AssertionError)
            typecheck.is_enum(newStatus, raritan.rpc.cascading.CascadeManager.LinkUnitStatus, AssertionError)

            self.linkId = linkId
            self.host = host
            self.oldStatus = oldStatus
            self.newStatus = newStatus

        def encode(self):
            json = super(raritan.rpc.cascading.CascadeManager.LinkUnitStatusChangedEvent, self).encode()
            json['linkId'] = self.linkId
            json['host'] = self.host
            json['oldStatus'] = raritan.rpc.cascading.CascadeManager.LinkUnitStatus.encode(self.oldStatus)
            json['newStatus'] = raritan.rpc.cascading.CascadeManager.LinkUnitStatus.encode(self.newStatus)
            return json

        @classmethod
        def decode(cls, json, agent):
            obj = cls(
                linkId = json['linkId'],
                host = json['host'],
                oldStatus = raritan.rpc.cascading.CascadeManager.LinkUnitStatus.decode(json['oldStatus']),
                newStatus = raritan.rpc.cascading.CascadeManager.LinkUnitStatus.decode(json['newStatus']),
                # for idl.Event
                source = Interface.decode(json['source'], agent),
            )
            return obj

        def listElements(self):
            elements = ["linkId", "host", "oldStatus", "newStatus"]
            elements = elements + super(raritan.rpc.cascading.CascadeManager.LinkUnitStatusChangedEvent, self).listElements()
            return elements

    class _getStatus(Interface.Method):
        name = 'getStatus'

        @staticmethod
        def encode():
            args = {}
            return args

        @staticmethod
        def decode(rsp, agent):
            _ret_ = raritan.rpc.cascading.CascadeManager.Status.decode(rsp['_ret_'], agent)
            typecheck.is_struct(_ret_, raritan.rpc.cascading.CascadeManager.Status, DecodeException)
            return _ret_

    class _addLinkUnit(Interface.Method):
        name = 'addLinkUnit'

        @staticmethod
        def encode(linkId, host, login, password, newPassword):
            typecheck.is_int(linkId, AssertionError)
            typecheck.is_string(host, AssertionError)
            typecheck.is_string(login, AssertionError)
            typecheck.is_string(password, AssertionError)
            typecheck.is_string(newPassword, AssertionError)
            args = {}
            args['linkId'] = linkId
            args['host'] = host
            args['login'] = login
            args['password'] = password
            args['newPassword'] = newPassword
            return args

        @staticmethod
        def decode(rsp, agent):
            _ret_ = rsp['_ret_']
            typecheck.is_int(_ret_, DecodeException)
            return _ret_

    class _releaseLinkUnit(Interface.Method):
        name = 'releaseLinkUnit'

        @staticmethod
        def encode(linkId):
            typecheck.is_int(linkId, AssertionError)
            args = {}
            args['linkId'] = linkId
            return args

        @staticmethod
        def decode(rsp, agent):
            _ret_ = rsp['_ret_']
            typecheck.is_int(_ret_, DecodeException)
            return _ret_

    class _requestLink(Interface.Method):
        name = 'requestLink'

        @staticmethod
        def encode(token):
            typecheck.is_string(token, AssertionError)
            args = {}
            args['token'] = token
            return args

        @staticmethod
        def decode(rsp, agent):
            _ret_ = rsp['_ret_']
            typecheck.is_int(_ret_, DecodeException)
            return _ret_

    class _finalizeLink(Interface.Method):
        name = 'finalizeLink'

        @staticmethod
        def encode(token):
            typecheck.is_string(token, AssertionError)
            args = {}
            args['token'] = token
            return args

        @staticmethod
        def decode(rsp, agent):
            return None

    class _unlink(Interface.Method):
        name = 'unlink'

        @staticmethod
        def encode():
            args = {}
            return args

        @staticmethod
        def decode(rsp, agent):
            return None
    def __init__(self, target, agent):
        super(CascadeManager, self).__init__(target, agent)
        self.getStatus = CascadeManager._getStatus(self)
        self.addLinkUnit = CascadeManager._addLinkUnit(self)
        self.releaseLinkUnit = CascadeManager._releaseLinkUnit(self)
        self.requestLink = CascadeManager._requestLink(self)
        self.finalizeLink = CascadeManager._finalizeLink(self)
        self.unlink = CascadeManager._unlink(self)
