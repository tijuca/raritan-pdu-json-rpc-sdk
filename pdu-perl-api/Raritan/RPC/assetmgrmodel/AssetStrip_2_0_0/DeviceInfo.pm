# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from AssetStrip.idl.

use strict;

package Raritan::RPC::assetmgrmodel::AssetStrip_2_0_0::DeviceInfo;

sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'deviceId'} = 1 * $in->{'deviceId'};
    $encoded->{'hardwareId'} = 1 * $in->{'hardwareId'};
    $encoded->{'protocolVersion'} = 1 * $in->{'protocolVersion'};
    $encoded->{'bootVersion'} = 1 * $in->{'bootVersion'};
    $encoded->{'appVersion'} = 1 * $in->{'appVersion'};
    $encoded->{'orientationSensAvailable'} = ($in->{'orientationSensAvailable'}) ? JSON::true : JSON::false;
    $encoded->{'isCascadable'} = ($in->{'isCascadable'}) ? JSON::true : JSON::false;
    $encoded->{'rackUnitCountConfigurable'} = ($in->{'rackUnitCountConfigurable'}) ? JSON::true : JSON::false;
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'deviceId'} = $in->{'deviceId'};
    $decoded->{'hardwareId'} = $in->{'hardwareId'};
    $decoded->{'protocolVersion'} = $in->{'protocolVersion'};
    $decoded->{'bootVersion'} = $in->{'bootVersion'};
    $decoded->{'appVersion'} = $in->{'appVersion'};
    $decoded->{'orientationSensAvailable'} = ($in->{'orientationSensAvailable'}) ? 1 : 0;
    $decoded->{'isCascadable'} = ($in->{'isCascadable'}) ? 1 : 0;
    $decoded->{'rackUnitCountConfigurable'} = ($in->{'rackUnitCountConfigurable'}) ? 1 : 0;
    return $decoded;
}

1;
