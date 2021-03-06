# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from AlertedSensorManager.idl.

use strict;

package Raritan::RPC::sensors::AlertedSensorManager::SensorData;


sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'sensor'} = Raritan::RPC::ObjectCodec::encode($in->{'sensor'});
    $encoded->{'parent'} = Raritan::RPC::ObjectCodec::encode($in->{'parent'});
    $encoded->{'alertState'} = $in->{'alertState'};
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'sensor'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'sensor'}, 'sensors.Sensor');
    $decoded->{'parent'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'parent'}, 'idl.Object');
    $decoded->{'alertState'} = $in->{'alertState'};
    return $decoded;
}

1;
