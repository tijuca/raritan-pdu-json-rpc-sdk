# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from ExternalSensorSlot.idl.

use strict;

package Raritan::RPC::sensors::ExternalSensorSlot_3_0_0::Settings;

use Raritan::RPC::sensors::ExternalSensorSlot_3_0_0::Location;

sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'name'} = "$in->{'name'}";
    $encoded->{'description'} = "$in->{'description'}";
    $encoded->{'location'} = Raritan::RPC::sensors::ExternalSensorSlot_3_0_0::Location::encode($in->{'location'});
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'name'} = $in->{'name'};
    $decoded->{'description'} = $in->{'description'};
    $decoded->{'location'} = Raritan::RPC::sensors::ExternalSensorSlot_3_0_0::Location::decode($agent, $in->{'location'});
    return $decoded;
}

1;
