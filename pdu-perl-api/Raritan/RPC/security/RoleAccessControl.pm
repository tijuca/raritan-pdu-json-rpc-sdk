# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Security.idl.

use strict;

package Raritan::RPC::security::RoleAccessControl;

use Raritan::RPC::security::RoleAccessRule;

sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'enabled'} = ($in->{'enabled'}) ? JSON::true : JSON::false;
    $encoded->{'defaultPolicy'} = $in->{'defaultPolicy'};
    $encoded->{'rules'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'rules'}}; $i0++) {
        $encoded->{'rules'}->[$i0] = Raritan::RPC::security::RoleAccessRule::encode($in->{'rules'}->[$i0]);
    }
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'enabled'} = ($in->{'enabled'}) ? 1 : 0;
    $decoded->{'defaultPolicy'} = $in->{'defaultPolicy'};
    $decoded->{'rules'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'rules'}}; $i0++) {
        $decoded->{'rules'}->[$i0] = Raritan::RPC::security::RoleAccessRule::decode($agent, $in->{'rules'}->[$i0]);
    }
    return $decoded;
}

1;