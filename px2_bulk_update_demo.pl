#! /usr/bin/perl
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2010 Raritan Inc. All rights reserved.

use strict;
use LWP::UserAgent;
use LWP::Protocol::https; # required for HTTPS support, do not remove!
use HTTP::Request::Common;
use JSON::RPC::Common::Marshal::HTTP;
use Sys::SigAction qw( timeout_call );

my $device_list;
my $filename;
my @pdus;

# stolen from http://stackoverflow.com/questions/73308/true-timeout-on-lwpuseragent-request-method;
# will probably not work in Windows
sub ua_request_with_timeout {
    my ($ua, $req, $timeout) = @_;
    our $res = undef;
    if( timeout_call( $timeout, sub {$res = $ua->request($req);}) ) {
        return HTTP::Response->new( 408 ); #408 is the HTTP timeout
    } else {
        return $res;
    }
}

sub json_request($$$$) {
    my ($request, $url, $user, $password) = @_;

    my $m = JSON::RPC::Common::Marshal::HTTP->new();
    my $req_obj = JSON::RPC::Common::Procedure::Call->inflate($request);
    my $http_request = $m->call_to_post_request($req_obj, uri => new URI($url));
    if (defined($user) && defined($password)) {
	$http_request->authorization_basic($user, $password);
    }

    my $ua = LWP::UserAgent->new();
    my $response = ua_request_with_timeout($ua, $http_request, 10);
    my $respobj = undef;
    if ($response->code == 200) {
	$respobj = $m->response_to_result($response);
    }

    my $resp = {
	'response_code' => $response->code,
	'json_response' => $respobj,
    };
    return $resp;
}

sub get_firmware_version($$$) {
    my ($url, $user, $password) = @_;

    my $request = {
	jsonrpc => "2.0",
	method => "getVersion",
	params => { },
	id => 42,
    };
    my $resp = json_request($request, "$url/firmware", $user, $password);

    if ($resp->{'response_code'} == 200) {
	return $resp->{'json_response'}->{'result'}->{'_ret_'};
    } else {
	return undef;
    }
}

sub all_check_reachable() {
    foreach my $pdu (@pdus) {
	my $url = $pdu->{'url'};
	my $user = $pdu->{'user'};
	my $password = $pdu->{'password'};
	my $version = get_firmware_version($url, $user, $password);
	if (defined $version) {
	    print("$url: Firmware version: $version\n");
	    $pdu->{'status'} = 'ok';
	    $pdu->{'old_version'} = $version;
	} else {
	    print("$url: unreachable\n");
	    $pdu->{'status'} = 'unreachable';
	}
    }
}

sub new_session($$$) {
    my ($url, $user, $password) = @_;

    my $request = {
	jsonrpc => "2.0",
	method => "newSession",
	params => { },
	id => 42,
    };
    my $resp = json_request($request, "$url/session", $user, $password);

    if ($resp->{'response_code'} == 200) {
	return $resp->{'json_response'}->{'result'}->{'token'};
    } else {
	return undef;
    }
}

sub http_upload($$) {
    my ($url, $fname) = @_;

    my $ua = LWP::UserAgent->new();
    my $req = POST $url,
	Content_Type => 'form-data',
	Content => [ submit => 1, upfile => [ $fname ] ];
    my $rsp = ua_request_with_timeout($ua, $req, 60);

    return $rsp->is_success();
}

sub all_upload_image() {
    foreach my $pdu (@pdus) {
	if ($pdu->{'status'} eq 'ok') {
	    my $url = $pdu->{'url'};
	    my $user = $pdu->{'user'};
	    my $password = $pdu->{'password'};
	    my $upload_url = "$url/cgi-bin/fwupload.cgi";
	    my $token = new_session($url, $user, $password);
	    if (defined $token) {
		$upload_url .= "?session=$token";
	    } else {
		print("$url: Failed to open session, trying unauthenticated upload\n");
	    }
	    if (http_upload($upload_url, $filename)) {
		print("$url: Uploaded image\n");
	    } else {
		print("$url: Upload failed\n");
		$pdu->{'status'} = 'upload_failed';
	    }
	}
    }
}

sub trigger_update($$$) {
    my ($url, $user, $password) = @_;

    my $request = {
	jsonrpc => "2.0",
	method => "startUpdate",
	params => { flags => [] },
	id => 42,
    };
    my $resp = json_request($request, "$url/firmware", $user, $password);

    return ($resp->{'response_code'} == 200);
}

sub all_trigger_update() {
    foreach my $pdu (@pdus) {
	if ($pdu->{'status'} eq 'ok') {
	    my $url = $pdu->{'url'};
	    my $user = $pdu->{'user'};
	    my $password = $pdu->{'password'};
	    if (trigger_update($url, $user, $password)) {
		print("$url: Update started\n");
		$pdu->{'status'} = 'update_started';
	    } else {
		print("$url: Failed to start update\n");
		$pdu->{'status'} = 'start_failed';
	    }
	}
    }
}

sub get_update_state($) {
    my ($url) = @_;
    my $progress_url = "$url/cgi-bin/fwupdate_progress.cgi";
    my $request = {
	jsonrpc => "2.0",
	method => "getStatus",
	params => { },
	id => 42,
    };
    my $resp = json_request($request, $progress_url, undef, undef);
    if ($resp->{'response_code'} == 200) {
	if (defined ($resp->{'json_response'}->{'result'}->{'_ret_'})) {
	    return $resp->{'json_response'}->{'result'}->{'_ret_'}->{'state'};
	} else {
	    return $resp->{'json_response'}->{'result'}->{'state'};
	}
    } else {
	return undef;
    }
}

sub all_wait_complete() {
    my $pending;
    my $deadline = time() + 600;
    do {
	$pending = 0;
	foreach my $pdu (@pdus) {
	    my $url = $pdu->{'url'};
	    if ($pdu->{'status'} eq 'update_started' ||
		$pdu->{'status'} eq 'update_succeeded') {
		my $state = get_update_state($url);
		if ($state eq 'NONE') {
		    print("$url: Update complete\n");
		    $pdu->{'status'} = 'complete';
		} elsif ($state eq 'FAIL') {
		    print("$url: Update failed\n");
		    $pdu->{'status'} = 'update_failed';
		} elsif ($state eq 'SUCCESS') {
		    if ($pdu->{'status'} ne 'update_succeeded') {
			print("$url: Update succeeded, rebooting ...\n");
			$pdu->{'status'} = 'update_succeeded';
		    }
		    $pending++;
		} else {
		    $pending++;
		}
	    }
	}
	sleep(2);
    } while ($pending > 0 && time() <= $deadline);
    if ($pending > 0) {
	print("TIMEOUT: $pending devices pending!\n");
    }
}

sub all_get_new_version() {
    foreach my $pdu (@pdus) {
	if ($pdu->{'status'} eq 'complete') {
	    my $url = $pdu->{'url'};
	    my $user = $pdu->{'user'};
	    my $password = $pdu->{'password'};
	    my $version = get_firmware_version($url, $user, $password);
	    if (defined $version) {
		$pdu->{'new_version'} = $version;
	    } else {
		print("$url: unreachable after update\n");
		$pdu->{'status'} = 'unreachable2';
	    }
	}
    }
}

sub all_print_status() {
    foreach my $pdu (@pdus) {
	my $url = $pdu->{'url'};
	my $status = $pdu->{'status'};
	if ($status eq "complete") {
	    my $old = $pdu->{'old_version'};
	    my $new = $pdu->{'new_version'};
	    print("$url: Successfully upgraded from $old to $new.\n");
	} elsif ($status eq "unreachable") {
	    print("$url: ERROR: Could not connect to device\n");
	} elsif ($status eq "upload_failed") {
	    print("$url: ERROR: Failed to upload image\n");
	} elsif ($status eq "update_failed") {
	    print("$url: ERROR: Firmware update failed\n");
	} elsif ($status eq "update_started") {
	    print("$url: ERROR: Timeout during update\n");
	} elsif ($status eq "update_succeeded") {
	    print("$url: ERROR: Device did not reboot after update\n");
	} elsif ($status eq "unreachable2") {
	    print("$url: ERROR: Device unreachable after update\n");
	} else {
	    print("$url: ERROR: $status\n");
	}
    }
}

$device_list = shift;
$filename = shift;

if (!defined($device_list) || !defined($filename)) {
    print("Usage: $0 DEVICE_LIST IMAGE_FILE\n");
    print("\n");
    print("  DEVICE_LIST: List of devices to upgrade. The file must contain one\n");
    print("               line per device. Each line must follow the syntax:\n");
    print("\n");
    print("               <URL>,<USERNAME>,<PASSWORD>\n");
    print("\n");
    print("  IMAGE_FILE:  Firmware image file.\n");
    exit 1;
}

open F, "<$device_list" or die "Can't open device list file";
my $lineno = 0;
while (my $line = <F>) {
    $lineno++;
    chomp($line);
    next if $line eq "";
    my ($url, $user, $password) = split ',', $line;
    if (defined($url) && defined($user) && defined($password)) {
	print("URL: '$url', User: '$user', Password: '$password'\n");
	push @pdus, { url => $url, user => $user, password => $password };
    } else {
	print "$device_list: Syntax error in line $lineno\n";
	exit 1;
    }
}

all_check_reachable();
all_upload_image();
all_trigger_update();
all_wait_complete();
all_get_new_version();
all_print_status();
