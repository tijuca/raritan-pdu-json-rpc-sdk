# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Cfg.idl.

use strict;

package Raritan::RPC::cfg::Cfg_1_0_1;

use parent qw(Raritan::RPC::RemoteObject);

use constant typeId => "cfg.Cfg:1.0.1";

sub new {
    my ($class, $agent, $rid, $typeId) = @_;
    $typeId = $typeId || Raritan::RPC::cfg::Cfg_1_0_1::typeId;
    return $class->SUPER::new($agent, $rid, $typeId);
}

use constant ERR_INVALID_KEY => 1;

use constant ERR_INVALID_VALUE => 2;

use constant ERR_NOT_ALLOWED_IN_FIPS_MODE => 3;

sub getValues($$$) {
    my ($self, $values, $keys) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    $args->{'keys'} = [];
    for (my $i0 = 0; $i0 <= $#{$keys}; $i0++) {
        $args->{'keys'}->[$i0] = "$keys->[$i0]";
    }
    my $rsp = $agent->json_rpc($self->{'rid'}, 'getValues', $args);
    $$values = [];
    for (my $i0 = 0; $i0 <= $#{$rsp->{'values'}}; $i0++) {
        $$values->[$i0] = $rsp->{'values'}->[$i0];
    }
    my $_ret_;
    $_ret_ = $rsp->{'_ret_'};
    return $_ret_;
}

use Raritan::RPC::cfg::KeyValue;

sub setValues($$) {
    my ($self, $keyvaluepairs) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    $args->{'keyvaluepairs'} = [];
    for (my $i0 = 0; $i0 <= $#{$keyvaluepairs}; $i0++) {
        $args->{'keyvaluepairs'}->[$i0] = Raritan::RPC::cfg::KeyValue::encode($keyvaluepairs->[$i0]);
    }
    my $rsp = $agent->json_rpc($self->{'rid'}, 'setValues', $args);
    my $_ret_;
    $_ret_ = $rsp->{'_ret_'};
    return $_ret_;
}

Raritan::RPC::Registry::registerProxyClass('cfg.Cfg', 1, 0, 1, 'Raritan::RPC::cfg::Cfg_1_0_1');
1;
