# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2010 Raritan Inc. All rights reserved.

import raritan.rpc

class Enumeration(object):
    def __init__(self, val):
        assert isinstance(val, int)
        self.val = val

    def encode(self):
        """Encodes enum value to JSON int."""
        json = self.val
        return json
 
    @classmethod
    def decode(cls, json):
        """Decodes JSON int to enum value."""
        assert isinstance(json, int)
        val = json
        assert(val >= 0)
        if (val < len(cls.values)):
            attr = cls.values[val]
            return getattr(cls, attr)
        elif hasattr(cls, '_fallback'):
            return cls._fallback
        raise KeyError(val)

    @classmethod
    def get_elements(cls):
        """Returns enum elements as instances of cls, the respective enum class."""
        return [getattr(cls, v) for v in cls.values]

    def __str__(self):
        return "%s.%s" % (raritan.rpc.TypeInfo.typeBaseName(self.idlType), self.values[self.val])

    def __repr__(self):
        return str(self)

    def __eq__(self, other):
        return self.__class__ == other.__class__ and self.val == other.val

    def __ne__(self, other):
        return not self == other

    def __hash__(self):
        return hash((self.idlType, self.val))
