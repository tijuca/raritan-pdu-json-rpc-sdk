# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from BulkConfiguration.idl.

use strict;

package Raritan::RPC::bulkcfg::BulkConfiguration_1_0_1::Filter;

sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'name'} = "$in->{'name'}";
    $encoded->{'displayName'} = "$in->{'displayName'}";
    $encoded->{'noOverride'} = ($in->{'noOverride'}) ? JSON::true : JSON::false;
    $encoded->{'bulkOnly'} = ($in->{'bulkOnly'}) ? JSON::true : JSON::false;
    $encoded->{'ruleSpecs'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'ruleSpecs'}}; $i0++) {
        $encoded->{'ruleSpecs'}->[$i0] = "$in->{'ruleSpecs'}->[$i0]";
    }
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'name'} = $in->{'name'};
    $decoded->{'displayName'} = $in->{'displayName'};
    $decoded->{'noOverride'} = ($in->{'noOverride'}) ? 1 : 0;
    $decoded->{'bulkOnly'} = ($in->{'bulkOnly'}) ? 1 : 0;
    $decoded->{'ruleSpecs'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'ruleSpecs'}}; $i0++) {
        $decoded->{'ruleSpecs'}->[$i0] = $in->{'ruleSpecs'}->[$i0];
    }
    return $decoded;
}

1;