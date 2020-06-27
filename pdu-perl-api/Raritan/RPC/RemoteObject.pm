# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2012 Raritan Inc. All rights reserved.

package Raritan::RPC::RemoteObject;

use strict;
use warnings;

use Raritan::RPC::Exception;

###############################################################################
# members
#

sub new {
    my ($class, $agent, $rid, $type) = @_;
    my $self = {
	'agent' => $agent,
	'rid' => $rid,
	'typeId' => $type,  # IDL type
    };
    bless $self, $class;
    return $self;
}

sub getTypeInfo($) {
    my ($self) = @_;
    my $rsp = $self->{'agent'}->json_rpc($self->{'rid'}, 'getTypeInfo', {});
    return $rsp->{'_ret_'};
}

###############################################################################
# static helpers
#

# Extracts fields from json_rpc response and returns them together with err msg.
# 
# @param rsp         response as returned by json_rpc
# @param fieldNames  ref to an array containing list of field names to extract
#		       - "_ret_" is special for the IDL func return value
#		       - out params are named as in IDL
# @return  Tuple (<retval_0>, ..., <retval_n>, <error message>).  If any is
#          undefined, element is replaced by undef. Elem count stays the same.
sub retvalsAndErr($$) {
    my ($rsp, $fieldNames) = @_;

    my $result = $rsp->{'result'};
    my @retvals = ();
    foreach my $f (@$fieldNames) {
	push @retvals, $result ? $result->{$f} : undef;
    }
    my $errmsg = ($rsp->{'json_response'})
		    ? (($rsp->{'json_response'}->deflate_error)
			? $rsp->{'json_response'}->deflate_error->{'message'}
			: undef)
		    : "No json_response.";
    return (@retvals, $errmsg);
}

###############################################################################
# exports
#

our @ISA    = qw(Exporter);
our @EXPORT = qw(retvalsAndErr);

1;
