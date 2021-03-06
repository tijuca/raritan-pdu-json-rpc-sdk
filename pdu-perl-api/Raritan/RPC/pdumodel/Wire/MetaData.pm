# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Wire.idl.

use strict;

package Raritan::RPC::pdumodel::Wire::MetaData;

sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'label'} = "$in->{'label'}";
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'label'} = $in->{'label'};
    return $decoded;
}

1;
