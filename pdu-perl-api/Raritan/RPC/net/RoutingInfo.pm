# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Net.idl.

use strict;

package Raritan::RPC::net::RoutingInfo;

use Raritan::RPC::net::IpRoute;
use Raritan::RPC::net::IpRoute;

sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'ipv4Routes'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'ipv4Routes'}}; $i0++) {
        $encoded->{'ipv4Routes'}->[$i0] = Raritan::RPC::net::IpRoute::encode($in->{'ipv4Routes'}->[$i0]);
    }
    $encoded->{'ipv6Routes'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'ipv6Routes'}}; $i0++) {
        $encoded->{'ipv6Routes'}->[$i0] = Raritan::RPC::net::IpRoute::encode($in->{'ipv6Routes'}->[$i0]);
    }
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'ipv4Routes'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'ipv4Routes'}}; $i0++) {
        $decoded->{'ipv4Routes'}->[$i0] = Raritan::RPC::net::IpRoute::decode($agent, $in->{'ipv4Routes'}->[$i0]);
    }
    $decoded->{'ipv6Routes'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'ipv6Routes'}}; $i0++) {
        $decoded->{'ipv6Routes'}->[$i0] = Raritan::RPC::net::IpRoute::decode($agent, $in->{'ipv6Routes'}->[$i0]);
    }
    return $decoded;
}

1;
