# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from StorageManager.idl.

use strict;

package Raritan::RPC::webcam::StorageManager::StorageSettings;


sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'type'} = $in->{'type'};
    $encoded->{'capacity'} = 1 * $in->{'capacity'};
    $encoded->{'server'} = "$in->{'server'}";
    $encoded->{'username'} = "$in->{'username'}";
    $encoded->{'password'} = "$in->{'password'}";
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'type'} = $in->{'type'};
    $decoded->{'capacity'} = $in->{'capacity'};
    $decoded->{'server'} = $in->{'server'};
    $decoded->{'username'} = $in->{'username'};
    $decoded->{'password'} = $in->{'password'};
    return $decoded;
}

1;