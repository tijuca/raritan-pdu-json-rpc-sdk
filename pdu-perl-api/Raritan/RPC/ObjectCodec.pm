# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2012 Raritan Inc. All rights reserved.

package Raritan::RPC::ObjectCodec;

use strict;
use warnings;

use Raritan::RPC::Registry;
use Raritan::RPC::Exception;

sub encode($) {
    my ($obj) = @_;
    if ($obj) {
	return {
	    'rid' => $obj->{'rid'},
	    'type' => $obj->{'typeId'}
	};
    } else {
	return undef;
    }
}

sub decode($$$) {
    my ($agent, $ref, $classname) = @_;
    if (!defined $ref) {
	return undef;
    }

    # try to find a compatible proxy for the type ID reported by the server
    my $proxy_class = Raritan::RPC::Registry::findProxyClass($ref->{'type'});

    if (!defined $proxy_class) {
	# no compatible proxy found; use class from the IDL signature
	if ($classname eq 'idl.Object') {
	    $proxy_class = 'Raritan::RPC::RemoteObject';
	} else {
	    $proxy_class = Raritan::RPC::Registry::findProxyClass($classname);
	}
    }

    if (!defined $proxy_class) {
	throw Raritan::RPC::JsonRpcErrorException("No proxy found for $ref->{'type'}");
    }
    return $proxy_class->new($agent, $ref->{'rid'});
}

1;
