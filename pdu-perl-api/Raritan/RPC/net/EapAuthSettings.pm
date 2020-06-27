# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Net.idl.

use strict;

package Raritan::RPC::net::EapAuthSettings;


sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'identity'} = "$in->{'identity'}";
    $encoded->{'password'} = "$in->{'password'}";
    $encoded->{'clearPassword'} = ($in->{'clearPassword'}) ? JSON::true : JSON::false;
    $encoded->{'outerMethod'} = $in->{'outerMethod'};
    $encoded->{'innerMethod'} = $in->{'innerMethod'};
    $encoded->{'caCertChain'} = "$in->{'caCertChain'}";
    $encoded->{'forceTrustedCert'} = ($in->{'forceTrustedCert'}) ? JSON::true : JSON::false;
    $encoded->{'allowOffTimeRangeCerts'} = ($in->{'allowOffTimeRangeCerts'}) ? JSON::true : JSON::false;
    $encoded->{'allowNotYetValidCertsIfTimeBeforeBuild'} = ($in->{'allowNotYetValidCertsIfTimeBeforeBuild'}) ? JSON::true : JSON::false;
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'identity'} = $in->{'identity'};
    $decoded->{'password'} = $in->{'password'};
    $decoded->{'clearPassword'} = ($in->{'clearPassword'}) ? 1 : 0;
    $decoded->{'outerMethod'} = $in->{'outerMethod'};
    $decoded->{'innerMethod'} = $in->{'innerMethod'};
    $decoded->{'caCertChain'} = $in->{'caCertChain'};
    $decoded->{'forceTrustedCert'} = ($in->{'forceTrustedCert'}) ? 1 : 0;
    $decoded->{'allowOffTimeRangeCerts'} = ($in->{'allowOffTimeRangeCerts'}) ? 1 : 0;
    $decoded->{'allowNotYetValidCertsIfTimeBeforeBuild'} = ($in->{'allowNotYetValidCertsIfTimeBeforeBuild'}) ? 1 : 0;
    return $decoded;
}

1;