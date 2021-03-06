# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from ServerMonitor.idl.

use strict;

package Raritan::RPC::servermon::ServerMonitor_2_0_1::ServerPowerSettings;


sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'enabled'} = ($in->{'enabled'}) ? JSON::true : JSON::false;
    $encoded->{'target'} = Raritan::RPC::ObjectCodec::encode($in->{'target'});
    $encoded->{'powerCheck'} = $in->{'powerCheck'};
    $encoded->{'powerThreshold'} = 1 * $in->{'powerThreshold'};
    $encoded->{'timeout'} = 1 * $in->{'timeout'};
    $encoded->{'shutdownCmd'} = "$in->{'shutdownCmd'}";
    $encoded->{'username'} = "$in->{'username'}";
    $encoded->{'password'} = "$in->{'password'}";
    $encoded->{'sshPort'} = 1 * $in->{'sshPort'};
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'enabled'} = ($in->{'enabled'}) ? 1 : 0;
    $decoded->{'target'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'target'}, 'idl.Object');
    $decoded->{'powerCheck'} = $in->{'powerCheck'};
    $decoded->{'powerThreshold'} = $in->{'powerThreshold'};
    $decoded->{'timeout'} = $in->{'timeout'};
    $decoded->{'shutdownCmd'} = $in->{'shutdownCmd'};
    $decoded->{'username'} = $in->{'username'};
    $decoded->{'password'} = $in->{'password'};
    $decoded->{'sshPort'} = $in->{'sshPort'};
    return $decoded;
}

1;
