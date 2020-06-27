# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2012 Raritan Inc. All rights reserved.

package Raritan::RPC::ValObjCodec;

use strict;
use warnings;

use Raritan::RPC::Registry;
use Raritan::RPC::Exception;

sub encode($) {
    my ($valobj) = @_;
    if (!defined $valobj || !defined $valobj->{'_typeId'}) {
	return undef;
    }

    my $codec_class = Raritan::RPC::Registry::findCodecClass($valobj->{'_typeId'});
    if (!defined $codec_class) {
	throw Raritan::RPC::JsonRpcErrorException("No codec found for $valobj->{'_typeId'}");
    }

    my $encoder = $codec_class->can('encode');
    return $encoder->($valobj);
}

sub decode($$$) {
    my ($agent, $ref, $classname) = @_;
    if (!defined $ref) {
	return undef;
    }

    # try to find a compatible codec for the type ID reported by the server
    my $codec_class = Raritan::RPC::Registry::findCodecClass($ref->{'type'});

    if (!defined $codec_class) {
	# no compatible codec found; use class from the IDL signature
	$codec_class = Raritan::RPC::Registry::findCodecClass($classname);
    }

    if (!defined $codec_class) {
	throw Raritan::RPC::JsonRpcErrorException("No codec found for $ref->{'type'}");
    }

    my $decoder = $codec_class->can('decode');
    my $decoded = $decoder->($agent, $ref->{'value'});
    $decoded->{'_typeId'} = $ref->{'type'};
    return $decoded;
}

1;
