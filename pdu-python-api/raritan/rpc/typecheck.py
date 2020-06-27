# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2010 Raritan Inc. All rights reserved.

from __future__ import absolute_import
import sys

import raritan.rpc
from raritan.rpc import Interface, Structure, Enumeration, ValueObject
import inspect

#
# simple types
#
def is_bool(x, exception):
    if not isinstance(x, bool):
        raise exception("'%s' is not a bool" % x)

def _is_int(x):
    if sys.version_info.major < 3:
        return isinstance(x, (int, long))
    else:
        return isinstance(x, int)

def _is_float(x):
    return _is_int(x) or isinstance(x, float)

def is_byte(x, exception):
    if not _is_int(x) or int(x) < 0 or int(x) > 255:
        raise exception("'%s' is not a byte" % x)

def is_int(x, exception):
    if not _is_int(x):
        raise exception("'%s' is not an integer" % x)

def is_long(x, exception):
    if not _is_int(x):
        raise exception("'%s' is not a long" % x)

def is_float(x, exception):
    if not _is_float(x):
        raise exception("'%s' is not a float" % x)

def is_double(x, exception):
    if not _is_float(x):
        raise exception("'%s' is not a double" % x)

def is_string(x, exception):
    if sys.version_info.major < 3:
        if not isinstance(x, basestring):
            raise exception("'%s' is not a string" % x)
    else:
        if not isinstance(x, str):
            raise exception("'%s' is not a string" % x)

def is_time(x, exception):
    if not isinstance(x, raritan.rpc.Time):
        raise exception("'%s' is not a Time" % x)

def is_remote_obj(x, exception):
    is_interface(x, Interface, exception)

def is_typeinfo(x, exception):
    if not inspect.isclass(x) or (not issubclass(x, Interface) and not issubclass(x, ValueObject)):
        raise exception("'%s' is not a TypeInfo" % x)

#
# complex types
#
def is_interface(x, cls, exception):
    # allow to pass None as interface
    if x is None:
        return True

    if not isinstance(x, Interface):
        raise exception("'%s' is not an interface reference" % (x))
    is_class(x, cls, exception)

def is_struct(x, cls, exception):
    if not isinstance(x, Structure):
        raise exception("'%s' is not a structure" % (x))
    is_class(x, cls, exception)

def is_enum(x, cls, exception):
    if not isinstance(x, Enumeration):
        raise exception("'%s' is not an enumeration" % (x))
    is_class(x, cls, exception)

def is_valobj(x, cls, exception):
    if x == None:
    	return True # ValueObject may be null / 'None'
    if not isinstance(x, ValueObject):
        raise exception("'%s' is not a value object" % (x))
    is_class(x, cls, exception)

#
# helpers
#
def is_class(x, cls, exception):
    if not isinstance(x, cls):
        raise exception("'%s' is not a %s" % (x, cls))
