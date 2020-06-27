# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2012 Raritan Inc. All rights reserved.

package Raritan::RPC::Agent;

use strict;
use warnings;

use LWP::Protocol::https; # required for HTTPS support, do not remove!
use LWP::UserAgent;
use HTTP::Request::Common;
use JSON;
use JSON::RPC::Common::Marshal::HTTP;
use Error qw(:try);
use Data::Dumper;

use Raritan::RPC::RemoteObject;
use Raritan::RPC::Exception;

sub new {
    my ($class, $url, $user, $password, $disable_cert_eval) = @_;
    my $self = {
	url => $url,
	user => $user,
	password => $password,
	disable_cert_eval => (defined($disable_cert_eval) ? $disable_cert_eval : 0),
	token => undef,
	timeout_s => 60,
	request_id => 1,
	verbose => 0,
    };
    bless $self, $class;
    return $self;
}

sub set_verbose {
    my ($self, $v) = @_;
    $self->{'verbose'} = $v;
}

sub set_auth_basic {
    my ($self, $user, $password) = @_;
    $self->{'user'} = $user;
    $self->{'password'} = $password;
    $self->{'token'} = undef;
}

sub set_auth_token {
    my ($self, $token) = @_;
    $self->{'user'} = undef;
    $self->{'password'} = undef;
    $self->{'token'} = $token;
}

sub json_request {
    my ($self, $rid, $request) = @_;
    my $url = "$self->{'url'}/$rid";

    my $m = JSON::RPC::Common::Marshal::HTTP->new();
    my $req_obj = JSON::RPC::Common::Procedure::Call->inflate($request);
    my $http_request = $m->call_to_post_request($req_obj, uri => new URI($url));
    if (defined($self->{'user'}) && defined($self->{'password'})) {
	$http_request->authorization_basic($self->{'user'}, $self->{'password'});
    } elsif (defined($self->{'token'})) {
	$http_request->header('X-SessionToken' => $self->{'token'});
    }

    my $ssl_opts = $self->{'disable_cert_eval'} ? {
	verify_hostname => 0,
	SSL_verify_mode => IO::Socket::SSL::SSL_VERIFY_NONE
    } : undef;
    my $ua = LWP::UserAgent->new( timeout => $self->{'timeout_s'}, ssl_opts => $ssl_opts );
    if ($self->{'verbose'} == 1) {
	printf("\n### %s -> %s\n", $url, $request->{'method'});
	printf("### JSON-RPC Req: %s", Dumper($request));
	#printf("### curl -d '%s' -u %s:%s %s\n",
	#       $http_request->content, $self->{'user'}, 
	#       $self->{'password'}, $http_request->uri);
    }

    my $response = $ua->request($http_request);
    
    my $respobj = undef;
    if ($response->code == 200) {
	$respobj = $m->response_to_result($response);
	if ($self->{'verbose'} == 1) {
	    printf("### JSON-RPC Rsp: %s\n", Dumper($respobj));
	}
    }

    my $resp = {
	'response_code' => $response->code,
	'response_message' => $response->message,
	'json_response' => $respobj,
    };

    if ($response->code == 302) {
	$resp->{'location'} = $response->header('location');
    }

    return $resp;
}

sub json_rpc {
    my ($self, $rid, $method, $params) = @_;

    my $id = $self->{'request_id'};
    $self->{'request_id'} = $id + 1;

    my $request = {
	jsonrpc => "2.0",
	method => $method,
	params => $params,
	id => $id,
    };
    my $resp = $self->json_request($rid, $request);

    if ($resp->{'response_code'} == 302) {
	# handle HTTP-to-HTTPS redirection
	my $location = $resp->{'location'};
	my $baselen = length($location) - length($rid);
	if (substr($location, $baselen) eq $rid) {
	    $self->{'url'} = substr($location, 0, $baselen);
	    $resp = $self->json_request($rid, $request);
	}
    }

    if ($resp->{'response_code'} != 200) {
	if ($resp->{'response_code'} == 500 and $resp->{'response_message'} =~ /certificate verify failed/) {
	    printf("==================================================================\n");
	    printf(" SSL certificate verification failed!\n");
	    printf("\n");
	    printf(" When connecting to a device without valid SSL certificate,\n");
	    printf(" set the 4th parameter of Raritan::RPC::Agent() to 1.\n");
	    printf("==================================================================\n");
	}
	throw Raritan::RPC::HttpException("HTTP Error $resp->{'response_code'} ($resp->{'response_message'})");
    }

    my $json_response = $resp->{'json_response'};

    if (!$json_response->isa("JSON::RPC::Common::Procedure::Return::Version_2_0")) {
	throw Raritan::RPC::JsonRpcSyntaxException("Response is not a valid JSON object");
    } elsif (defined $json_response->{'error'}) {
	my $msg = sprintf("JSON RPC Error Response (%d: %s)",
	               $json_response->{'error'}->{'code'},
		       $json_response->{'error'}->{'message'});
	throw Raritan::RPC::JsonRpcErrorException($msg);
    }

    my $deflated = $json_response->deflate;
    return $deflated->{'result'};
}

sub timeout {
    my ($self, $secs) = @_;
    my $orig_tmout = $self->{'timeout_s'};
    if (defined($secs)) {
	$self->{'timeout_s'} = $secs;
    }
    return $orig_tmout;
}

#
# Create a proxy for the given resource id.
#
# Queries the type info of the given resource id and tries to instantiate a
# proxy for the most-derived supported type. If the remote object does not
# support the getTypeInfo method and a $basetype argument is present a proxy
# for the specified class with version 1.0.0 is created.
#
sub createProxy($$;$) {
    my ($self, $rid, $basetype) = @_;

    my $typeinfo;
    try {
	my $obj = new Raritan::RPC::RemoteObject($self, $rid, undef);
	$typeinfo = $obj->getTypeInfo();
    } catch Raritan::RPC::JsonRpcErrorException with {
	# Remote object does not support getTypeInfo. This is probably
	# a pre-versioning server, so assume version 1.0.0 of the
	# specified type.
	if (!defined $basetype) {
	    throw Raritan::RPC::JsonRpcErrorException("Failed to get type information for $rid");
	}
	Raritan::RPC::debug("getTypeInfo() failed for $rid; assuming $basetype:1.0.0\n");
	$typeinfo = { 'id' => "$basetype:1.0.0", 'parent' => undef };
    };

    # Look for a compatible proxy, starting with the most-derived type id.
    my $ti = $typeinfo;
    while (defined $ti) {
	my $class = Raritan::RPC::Registry::findProxyClass($ti->{'id'});
	if (defined $class) {
	    return $class->new($self, $rid);
	}
	$ti = $ti->{'parent'};
    }
    throw Raritan::RPC::JsonRpcErrorException("No proxy found for $typeinfo->{'id'}");
}

1;
