# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from BulkConfiguration.idl.

use strict;

package Raritan::RPC::bulkcfg::BulkConfiguration;

use parent qw(Raritan::RPC::RemoteObject);

use constant typeId => "bulkcfg.BulkConfiguration:1.0.0";

sub new {
    my ($class, $agent, $rid, $typeId) = @_;
    $typeId = $typeId || Raritan::RPC::bulkcfg::BulkConfiguration::typeId;
    return $class->SUPER::new($agent, $rid, $typeId);
}


sub getStatus($$$) {
    my ($self, $status, $timeStamp) = @_;
    my $agent = $self->{'agent'};
    my $args = {};
    my $rsp = $agent->json_rpc($self->{'rid'}, 'getStatus', $args);
    $$status = $rsp->{'status'};
    $$timeStamp = $rsp->{'timeStamp'};
}

Raritan::RPC::Registry::registerProxyClass('bulkcfg.BulkConfiguration', 1, 0, 0, 'Raritan::RPC::bulkcfg::BulkConfiguration');
1;
