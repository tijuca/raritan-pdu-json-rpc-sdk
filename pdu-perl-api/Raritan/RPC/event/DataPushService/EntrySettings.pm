# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from DataPushService.idl.

use strict;

package Raritan::RPC::event::DataPushService::EntrySettings;


sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'url'} = "$in->{'url'}";
    $encoded->{'allowOffTimeRangeCerts'} = ($in->{'allowOffTimeRangeCerts'}) ? JSON::true : JSON::false;
    $encoded->{'caCertChain'} = "$in->{'caCertChain'}";
    $encoded->{'useAuth'} = ($in->{'useAuth'}) ? JSON::true : JSON::false;
    $encoded->{'username'} = "$in->{'username'}";
    $encoded->{'password'} = "$in->{'password'}";
    $encoded->{'type'} = $in->{'type'};
    $encoded->{'items'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'items'}}; $i0++) {
        $encoded->{'items'}->[$i0] = "$in->{'items'}->[$i0]";
    }
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'url'} = $in->{'url'};
    $decoded->{'allowOffTimeRangeCerts'} = ($in->{'allowOffTimeRangeCerts'}) ? 1 : 0;
    $decoded->{'caCertChain'} = $in->{'caCertChain'};
    $decoded->{'useAuth'} = ($in->{'useAuth'}) ? 1 : 0;
    $decoded->{'username'} = $in->{'username'};
    $decoded->{'password'} = $in->{'password'};
    $decoded->{'type'} = $in->{'type'};
    $decoded->{'items'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'items'}}; $i0++) {
        $decoded->{'items'}->[$i0] = $in->{'items'}->[$i0];
    }
    return $decoded;
}

1;
