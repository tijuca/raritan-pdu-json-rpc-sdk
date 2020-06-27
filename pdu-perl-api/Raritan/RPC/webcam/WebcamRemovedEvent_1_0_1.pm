# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from WebcamManager.idl.

use strict;

package Raritan::RPC::webcam::WebcamRemovedEvent_1_0_1;

use constant typeId => "webcam.WebcamRemovedEvent:1.0.1";
use Raritan::RPC::webcam::WebcamEvent_1_0_1;

sub encode {
    my ($in) = @_;
    my $encoded = Raritan::RPC::webcam::WebcamEvent_1_0_1::encode($in);
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = Raritan::RPC::webcam::WebcamEvent_1_0_1::decode($agent, $in);
    return $decoded;
}

Raritan::RPC::Registry::registerCodecClass('webcam.WebcamRemovedEvent', 1, 0, 1, 'Raritan::RPC::webcam::WebcamRemovedEvent_1_0_1');
1;