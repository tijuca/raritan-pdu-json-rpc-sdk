# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from OutletGroupManager.idl.

use strict;

package Raritan::RPC::pdumodel::OutletGroupManager_1_0_1;

use parent qw(Raritan::RPC::RemoteObject);

use constant typeId => "pdumodel.OutletGroupManager:1.0.1";

sub new {
    my ($class, $agent, $rid, $typeId) = @_;
    $typeId = $typeId || Raritan::RPC::pdumodel::OutletGroupManager_1_0_1::typeId;
    return $class->SUPER::new($agent, $rid, $typeId);
}

use constant ERR_INVALID_ARGUMENT => 1;

use constant ERR_NO_SUCH_ID => 2;

use constant ERR_MAX_GROUP_COUNT_REACHED => 3;


sub createGroup($$$$) {
    my ($self, $name, $members, $group) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    $args->{'name'} = "$name";
    $args->{'members'} = [];
    for (my $i0 = 0; $i0 <= $#{$members}; $i0++) {
        $args->{'members'}->[$i0] = Raritan::RPC::ObjectCodec::encode($members->[$i0]);
    }
    my $rsp = $agent->json_rpc($self->{'rid'}, 'createGroup', $args);
    $$group = Raritan::RPC::ObjectCodec::decode($agent, $rsp->{'group'}, 'pdumodel.OutletGroup');
    my $_ret_;
    $_ret_ = $rsp->{'_ret_'};
    return $_ret_;
}


sub getAllGroups($) {
    my ($self) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    my $rsp = $agent->json_rpc($self->{'rid'}, 'getAllGroups', $args);
    my $_ret_;
    $_ret_ = {};
    for (my $i0 = 0; $i0 <= $#{$rsp->{'_ret_'}}; $i0++) {
        my $key0 = $rsp->{'_ret_'}->[$i0]->{'key'};
        my $value0 = Raritan::RPC::ObjectCodec::decode($agent, $rsp->{'_ret_'}->[$i0]->{'value'}, 'pdumodel.OutletGroup');
        $_ret_->{$key0} = $value0;
    }
    return $_ret_;
}


sub getGroup($$$) {
    my ($self, $id, $group) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    $args->{'id'} = 1 * $id;
    my $rsp = $agent->json_rpc($self->{'rid'}, 'getGroup', $args);
    $$group = Raritan::RPC::ObjectCodec::decode($agent, $rsp->{'group'}, 'pdumodel.OutletGroup');
    my $_ret_;
    $_ret_ = $rsp->{'_ret_'};
    return $_ret_;
}

sub deleteGroup($$) {
    my ($self, $id) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    $args->{'id'} = 1 * $id;
    my $rsp = $agent->json_rpc($self->{'rid'}, 'deleteGroup', $args);
    my $_ret_;
    $_ret_ = $rsp->{'_ret_'};
    return $_ret_;
}

Raritan::RPC::Registry::registerProxyClass('pdumodel.OutletGroupManager', 1, 0, 1, 'Raritan::RPC::pdumodel::OutletGroupManager_1_0_1');
1;
