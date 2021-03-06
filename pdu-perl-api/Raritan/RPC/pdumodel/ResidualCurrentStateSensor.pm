# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from ResidualCurrentStateSensor.idl.

use strict;

package Raritan::RPC::pdumodel::ResidualCurrentStateSensor;

use parent qw(Raritan::RPC::sensors::StateSensor_3_0_1);

use constant typeId => "pdumodel.ResidualCurrentStateSensor:1.0.0";

sub new {
    my ($class, $agent, $rid, $typeId) = @_;
    $typeId = $typeId || Raritan::RPC::pdumodel::ResidualCurrentStateSensor::typeId;
    return $class->SUPER::new($agent, $rid, $typeId);
}

use constant STATE_NORMAL => 0;

use constant STATE_WARNING => 1;

use constant STATE_CRITICAL => 2;

use constant STATE_SELFTEST => 3;

use constant STATE_FAILURE => 4;

sub startSelfTest($) {
    my ($self) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    my $rsp = $agent->json_rpc($self->{'rid'}, 'startSelfTest', $args);
    my $_ret_;
    $_ret_ = $rsp->{'_ret_'};
    return $_ret_;
}

Raritan::RPC::Registry::registerProxyClass('pdumodel.ResidualCurrentStateSensor', 1, 0, 0, 'Raritan::RPC::pdumodel::ResidualCurrentStateSensor');
1;
