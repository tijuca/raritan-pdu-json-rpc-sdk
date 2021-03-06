# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from OutletGroup.idl.

use strict;

package Raritan::RPC::pdumodel::OutletGroup_1_0_1::Settings;


sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'name'} = "$in->{'name'}";
    $encoded->{'members'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'members'}}; $i0++) {
        $encoded->{'members'}->[$i0] = Raritan::RPC::ObjectCodec::encode($in->{'members'}->[$i0]);
    }
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'name'} = $in->{'name'};
    $decoded->{'members'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'members'}}; $i0++) {
        $decoded->{'members'}->[$i0] = Raritan::RPC::ObjectCodec::decode($agent, $in->{'members'}->[$i0], 'pdumodel.Outlet');
    }
    return $decoded;
}

1;
