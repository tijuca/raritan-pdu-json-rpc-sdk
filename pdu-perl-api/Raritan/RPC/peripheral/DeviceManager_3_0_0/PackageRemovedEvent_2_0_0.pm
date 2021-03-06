# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from PeripheralDeviceManager.idl.

use strict;

package Raritan::RPC::peripheral::DeviceManager_3_0_0::PackageRemovedEvent_2_0_0;

use constant typeId => "peripheral.DeviceManager_3_0_0.PackageRemovedEvent:2.0.0";
use Raritan::RPC::peripheral::DeviceManager_3_0_0::PackageEvent_2_0_0;

sub encode {
    my ($in) = @_;
    my $encoded = Raritan::RPC::peripheral::DeviceManager_3_0_0::PackageEvent_2_0_0::encode($in);
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = Raritan::RPC::peripheral::DeviceManager_3_0_0::PackageEvent_2_0_0::decode($agent, $in);
    return $decoded;
}

Raritan::RPC::Registry::registerCodecClass('peripheral.DeviceManager_3_0_0.PackageRemovedEvent', 2, 0, 0, 'Raritan::RPC::peripheral::DeviceManager_3_0_0::PackageRemovedEvent_2_0_0');
1;
