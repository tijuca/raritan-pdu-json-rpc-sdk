# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from InternalBeeper.idl.

use strict;

package Raritan::RPC::hmi::InternalBeeper_2_0_1::MuteChangedEvent;

use constant typeId => "hmi.InternalBeeper_2_0_1.MuteChangedEvent:1.0.0";
use Raritan::RPC::event::UserEvent;

sub encode {
    my ($in) = @_;
    my $encoded = Raritan::RPC::event::UserEvent::encode($in);
    $encoded->{'muted'} = ($in->{'muted'}) ? JSON::true : JSON::false;
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = Raritan::RPC::event::UserEvent::decode($agent, $in);
    $decoded->{'muted'} = ($in->{'muted'}) ? 1 : 0;
    return $decoded;
}

Raritan::RPC::Registry::registerCodecClass('hmi.InternalBeeper_2_0_1.MuteChangedEvent', 1, 0, 0, 'Raritan::RPC::hmi::InternalBeeper_2_0_1::MuteChangedEvent');
1;