# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from HardwareHealth.idl.

use strict;

package Raritan::RPC::fitness::HardwareHealth_1_0_1;

use parent qw(Raritan::RPC::RemoteObject);

use constant typeId => "fitness.HardwareHealth:1.0.1";

sub new {
    my ($class, $agent, $rid, $typeId) = @_;
    $typeId = $typeId || Raritan::RPC::fitness::HardwareHealth_1_0_1::typeId;
    return $class->SUPER::new($agent, $rid, $typeId);
}

use constant FAILURE_TYPE_NETWORK_DEVICE_NOT_DETECTED => 1;

use constant FAILURE_TYPE_I2C_BUS_STUCK => 2;

use constant FAILURE_TYPE_SLAVE_CTRL_NOT_REACHABLE => 3;

use constant FAILURE_TYPE_SLAVE_CTRL_MALFUNCTION => 4;

use constant FAILURE_TYPE_OUTLET_POWER_STATE_INCONSISTENT => 5;

use Raritan::RPC::fitness::HardwareHealth_1_0_1::Failure;

sub getFailures($) {
    my ($self) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    my $rsp = $agent->json_rpc($self->{'rid'}, 'getFailures', $args);
    my $_ret_;
    $_ret_ = [];
    for (my $i0 = 0; $i0 <= $#{$rsp->{'_ret_'}}; $i0++) {
        $_ret_->[$i0] = Raritan::RPC::fitness::HardwareHealth_1_0_1::Failure::decode($agent, $rsp->{'_ret_'}->[$i0]);
    }
    return $_ret_;
}

Raritan::RPC::Registry::registerProxyClass('fitness.HardwareHealth', 1, 0, 1, 'Raritan::RPC::fitness::HardwareHealth_1_0_1');
1;
