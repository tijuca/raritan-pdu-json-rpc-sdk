# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from DateTime.idl.

use strict;

package Raritan::RPC::datetime::DateTime::ZoneInfo;

sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'id'} = 1 * $in->{'id'};
    $encoded->{'name'} = "$in->{'name'}";
    $encoded->{'hasDSTInfo'} = ($in->{'hasDSTInfo'}) ? JSON::true : JSON::false;
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'id'} = $in->{'id'};
    $decoded->{'name'} = $in->{'name'};
    $decoded->{'hasDSTInfo'} = ($in->{'hasDSTInfo'}) ? 1 : 0;
    return $decoded;
}

1;
