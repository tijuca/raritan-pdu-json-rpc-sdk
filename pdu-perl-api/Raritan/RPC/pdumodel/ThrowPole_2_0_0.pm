# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Pole.idl.

use strict;

package Raritan::RPC::pdumodel::ThrowPole_2_0_0;


sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'label'} = "$in->{'label'}";
    $encoded->{'line'} = $in->{'line'};
    $encoded->{'inNodeIds'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'inNodeIds'}}; $i0++) {
        $encoded->{'inNodeIds'}->[$i0] = 1 * $in->{'inNodeIds'}->[$i0];
    }
    $encoded->{'outNodeId'} = 1 * $in->{'outNodeId'};
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'label'} = $in->{'label'};
    $decoded->{'line'} = $in->{'line'};
    $decoded->{'inNodeIds'} = [];
    for (my $i0 = 0; $i0 <= $#{$in->{'inNodeIds'}}; $i0++) {
        $decoded->{'inNodeIds'}->[$i0] = $in->{'inNodeIds'}->[$i0];
    }
    $decoded->{'outNodeId'} = $in->{'outNodeId'};
    return $decoded;
}

1;
