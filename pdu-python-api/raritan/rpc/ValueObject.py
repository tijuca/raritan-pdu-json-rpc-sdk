# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2013 Raritan Inc. All rights reserved.

import raritan.rpc

class ValueObject(object):
    def __init__(self):
        pass

    @staticmethod
    def decode(json, agent):
        if not json:
            return None
        class_ = raritan.rpc.TypeInfo.decode(json['type'])
        obj = class_.decode(json['value'], agent)
        return obj

    @staticmethod
    def encode(obj):
      json = {}
      json['type'] = obj.idlType
      json['value'] = obj.encode()
      return json

    def listValues(self):
        return [getattr(self, e) for e in self.listElements()]

    def __str__(self):
        elements = self.listElements()
        l = max([len(e) for e in elements])
        pretty = "\n".join([
            raritan.rpc.Utils.indent("* %-*s = %s" % (l, e, raritan.rpc.Utils.rprint(getattr(self, e))), 4) for e in elements
        ])
        return "%s:\n%s" % (raritan.rpc.TypeInfo.typeBaseName(self.idlType), pretty)

    def __eq__(self, other):
        return (other != None and self.idlType == other.idlType and self.listValues() == other.listValues())

    def __hash__(self):
        return hash((self.idlType, tuple(self.listValues())))
