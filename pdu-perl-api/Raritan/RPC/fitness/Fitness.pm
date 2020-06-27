# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Fitness.idl.

use strict;

package Raritan::RPC::fitness::Fitness;

use parent qw(Raritan::RPC::RemoteObject);

use constant typeId => "fitness.Fitness:1.0.0";

sub new {
    my ($class, $agent, $rid, $typeId) = @_;
    $typeId = $typeId || Raritan::RPC::fitness::Fitness::typeId;
    return $class->SUPER::new($agent, $rid, $typeId);
}

use constant FLAG_VALUE_INVALID => 0x1;

use constant FLAG_VALUE_OLD => 0x2;

use constant FLAG_ENTRY_CRITICAL => 0x4;

use Raritan::RPC::fitness::Fitness::DataEntry;

sub getDataEntries($) {
    my ($self) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    my $rsp = $agent->json_rpc($self->{'rid'}, 'getDataEntries', $args);
    my $_ret_;
    $_ret_ = [];
    for (my $i0 = 0; $i0 <= $#{$rsp->{'_ret_'}}; $i0++) {
        $_ret_->[$i0] = Raritan::RPC::fitness::Fitness::DataEntry::decode($agent, $rsp->{'_ret_'}->[$i0]);
    }
    return $_ret_;
}

sub getErrorLogIndexRange($$$) {
    my ($self, $firstIndex, $entryCount) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    my $rsp = $agent->json_rpc($self->{'rid'}, 'getErrorLogIndexRange', $args);
    $$firstIndex = $rsp->{'firstIndex'};
    $$entryCount = $rsp->{'entryCount'};
}

use Raritan::RPC::fitness::Fitness::ErrorLogEntry;

sub getErrorLogEntries($$$) {
    my ($self, $startIndex, $count) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    $args->{'startIndex'} = 1 * $startIndex;
    $args->{'count'} = 1 * $count;
    my $rsp = $agent->json_rpc($self->{'rid'}, 'getErrorLogEntries', $args);
    my $_ret_;
    $_ret_ = [];
    for (my $i0 = 0; $i0 <= $#{$rsp->{'_ret_'}}; $i0++) {
        $_ret_->[$i0] = Raritan::RPC::fitness::Fitness::ErrorLogEntry::decode($agent, $rsp->{'_ret_'}->[$i0]);
    }
    return $_ret_;
}

Raritan::RPC::Registry::registerProxyClass('fitness.Fitness', 1, 0, 0, 'Raritan::RPC::fitness::Fitness');
1;