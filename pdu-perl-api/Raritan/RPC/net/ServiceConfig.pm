# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Net.idl.

use strict;

package Raritan::RPC::net::ServiceConfig;

sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'service'} = "$in->{'service'}";
    $encoded->{'enable'} = ($in->{'enable'}) ? JSON::true : JSON::false;
    $encoded->{'port'} = 1 * $in->{'port'};
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'service'} = $in->{'service'};
    $decoded->{'enable'} = ($in->{'enable'}) ? 1 : 0;
    $decoded->{'port'} = $in->{'port'};
    return $decoded;
}

1;
