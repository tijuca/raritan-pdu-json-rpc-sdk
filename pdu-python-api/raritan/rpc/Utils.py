# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2013 Raritan Inc. All rights reserved.

# Avoid name clash with raritan.rpc.sys
from __future__ import absolute_import
import sys

class Utils(object):

    @staticmethod
    def indent(string, indent):
        return "\n".join([(" " * indent) + l for l in string.splitlines()])

    @staticmethod
    def rprint(object):
        if sys.version_info.major < 3:
            if isinstance(object, basestring):
                return '"' + str(object) + '"'
        else:
            if isinstance(object, str):
                return '"' + str(object) + '"'

        if isinstance(object, list):
            if len(object) == 0:
                return '[]'
            else:
                return '[\n' + ',\n'.join(Utils.indent(Utils.rprint(x), 4) for x in object) + '\n]'
        elif isinstance(object, dict):
            if len(object) == 0:
                return '{}'
            else:
                return '{\n' + "\n".join(Utils.indent("%s = %s" % (x, Utils.rprint(object[x])), 4) for x in sorted(object)) + '\n}'
        return str(object)
