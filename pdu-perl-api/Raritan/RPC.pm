# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2012 Raritan Inc. All rights reserved.

package Raritan::RPC;

use strict;

use Raritan::RPC::Agent;
use Raritan::RPC::Registry;
use Raritan::RPC::RemoteObject;
use Raritan::RPC::ObjectCodec;
use Raritan::RPC::ValObjCodec;
use Raritan::RPC::Exception;

# Load and register all proxy classes
use Raritan::RPC::All;

our $Debug = 0;

sub debug {
    if ($Debug) {
	print @_;
    }
}

1;
