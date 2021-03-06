# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from ResidualCurrentStateSensor.idl.

use strict;

package Raritan::RPC::pdumodel::ResidualCurrentStateSensor_2_0_0;

use parent qw(Raritan::RPC::sensors::StateSensor_4_0_0);

use constant typeId => "pdumodel.ResidualCurrentStateSensor:2.0.0";

sub new {
    my ($class, $agent, $rid, $typeId) = @_;
    $typeId = $typeId || Raritan::RPC::pdumodel::ResidualCurrentStateSensor_2_0_0::typeId;
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

Raritan::RPC::Registry::registerProxyClass('pdumodel.ResidualCurrentStateSensor', 2, 0, 0, 'Raritan::RPC::pdumodel::ResidualCurrentStateSensor_2_0_0');
1;
