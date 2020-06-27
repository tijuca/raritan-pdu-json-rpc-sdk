#! /usr/bin/perl -w
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2011 Raritan Inc. All rights reserved.

use strict;
use lib 'pdu-perl-api';

# Device admin account:
my $admin_username = 'admin';
my $admin_password = 'raritan';

# Network configuration parameters:
my $dns1 = '';
my $dns2 = '';
my $domain = '';

# SNMP configuration parameters:
my $snmp_read_community = 'public';
my $snmp_write_community = 'private';

# Disable Certificate Check
my $disable_tls_certificate_check = 1;

# Expected parameters for verification:
my $expected_firmware_version = '2.1.6.5-26030';

use strict;
use Pod::Usage;
use Error qw(:try);
use LWP::UserAgent;
use HTTP::Request::Common;
use FileHandle;

use Raritan::RPC;

use Test::More 'no_plan';

my $test_builder = Test::More->builder;
$test_builder->no_ending(1);

my @pdu_list;
my %pdu_hash;
my $agent = new Raritan::RPC::Agent('', $admin_username, $admin_password, $disable_tls_certificate_check);

sub usage() {
    pod2usage(
	-verbose => 99,
	-sections => "SYNOPSIS|COMMANDS|ARGUMENTS|EXAMPLES",
	-exitval => 1
    );
}

# Device list file format: CSV
# Columns:
#   1 Unit Serial Number
#   2 PDU Name
#   3 IP Address
#   4 Gateway
#   5 Netmask
#   6 Optional comment; line is ignored if "ignore" is included here

sub parse_config($) {
    my ($filename) = @_;
    open F, "<$filename" or die "Can't open config file '$filename'";
    my $lineno = 0;
    while (my $line = <F>) {
	$lineno++;
	chomp($line);
	my @columns = split ',', $line;
	next if (scalar @columns) < 5;

	my $config;
	$config->{'serial'} = shift @columns;

	# valid PX2 serial numbers start with 'P'
	next if !($config->{'serial'} =~ /^P/);

	$config->{'name'} = shift @columns;
	$config->{'ip'} = shift @columns;
	$config->{'gateway'} = shift @columns;
	$config->{'netmask'} = shift @columns;

	my $comment = shift @columns;
	if (!(defined($comment) && $comment =~ /ignore/)) {
	    push @pdu_list, $config;
	    $pdu_hash{$config->{'serial'}} = $config;
	}
    }
    close F;
}

sub all_ips() {
    my @ips = map { $_->{'ip'} } @pdu_list;
    return \@ips;
}

sub sanitize($) {
    my ($name) = @_;
    $name =~ tr/_ ./---/;
    return $name;
}

sub configure_network($$) {
    my ($pdu, $config) = @_;

    my $cfg = {
	'enabled' => 1,
	'autocfg' => Raritan::RPC::net::AutoConfigs::STATIC,
	'ipaddr' => $config->{'ip'},
	'netmask' => $config->{'netmask'},
	'gateway' => $config->{'gateway'},
	'hostname' => sanitize($config->{'name'}),
	'dns_suffixes' => [],
	'override_dns' => 0,
	'dns_ip_1' => $dns1,
	'dns_ip_2' => $dns2,
	'domain_name' => $domain,
    };

    print "Setting network configuration ... ";
    my $net = $agent->createProxy('/net');
    $net->setNetworkConfigIPv4($cfg);
    print "done\n";

    my $new_ip = $config->{'ip'};
    $agent->{'url'} = "https://$new_ip";

    print "Waiting for device to accept new IP address ...";
    my $ok = 0;
    for (my $i = 0; $i < 60; $i++) {
	sleep(1);
	print ".";
	try {
	    $pdu->getMetaData();
	    $ok = 1;
	} catch Error with {
	};
	last if $ok;
    }
    if (!$ok) {
	print " Timeout!\n";
	exit(3);
    }
    print " done\n";
}

sub configure_pdu($$) {
    my ($pdu, $config) = @_;

    print "Setting PDU name ... ";
    my $settings = $pdu->getSettings();
    $settings->{'name'} = $config->{'name'};
    $pdu->setSettings($settings);
    print "done\n";
}

sub configure_snmp($) {
    my ($ip) = @_;

    print "Configuring SNMP ...";
    my $snmp = $agent->createProxy('/snmp');
    my $cfg = $snmp->getConfiguration();
    $cfg->{'v2enable'} = 1;
    $cfg->{'v3enable'} = 0;
    $cfg->{'readComm'} = $snmp_read_community;
    $cfg->{'writeComm'} = $snmp_write_community;
    $snmp->setConfiguration($cfg);
    print "done\n";
}

sub configure_single($) {
    my ($ip) = @_;
    print "### Configuring $ip ###\n";
    $agent->{'url'} = "https://$ip";

    try {
	my $pdu = $agent->createProxy('/model/pdu/0');
	my $nameplate = $pdu->getMetaData()->{'nameplate'};
	my $serial = $nameplate->{'serialNumber'};

	print "PDU Model: $nameplate->{'model'}\n";
	print "Serial Number: $serial\n";

	my $config = $pdu_hash{$serial};
	if (!defined($config)) {
	    print STDERR "Serial number not found in config file.\n";
	} else {
	    print "Name: $config->{'name'}\n";
	    print "Hostname: " . sanitize($config->{'name'}) . "\n";
	    print "IP Address: $config->{'ip'}\n";

	    configure_pdu($pdu, $config);
	    configure_snmp($config->{'ip'});
	    configure_network($pdu, $config);
	}
    } catch Error with {
	my $e = shift;
	print $e;
    };
}

sub configure_all() {
    foreach my $config (@pdu_list) {
	print "### Configuring $config->{'ip'} ###\n";
	$agent->{'url'} = "https://$config->{'ip'}";

	try {
	    my $pdu = $agent->createProxy('/model/pdu/0');
	    my $nameplate = $pdu->getMetaData()->{'nameplate'};
	    if ($config->{'serial'} ne $nameplate->{'serialNumber'}) {
		print "Error: Serial number does not match.\n";
		print "  Expected: $config->{'serial'}\n";
		print "  Got:      $nameplate->{'serialNumber'}\n";
		next;
	    }
	    configure_pdu($pdu, $config);
	    configure_snmp($config->{'ip'});
	    print "### Successfully configured $config->{'ip'} ###\n";
	} catch Error with {
	    my $e = shift;
	    print "Error configuring $config->{'ip'}: $e";
	};
    }
}

sub do_configure($) {
    my ($ips) = @_;
    if (scalar @{$ips} == 0) {
	print STDERR "Usage: $0 configure LIST IPS\n";
	exit(1);
    }
    if ($ips->[0] eq 'all') {
	configure_all();
    } else {
	foreach my $ip (@{$ips}) {
	    configure_single($ip);
	}
    }
}

sub verify_pdu($$) {
    my ($pdu, $config) = @_;
    print "Verifying PDU Parameters ...\n";

    my $metadata = $pdu->getMetaData;
    my $nameplate = $metadata->{'nameplate'};
    is ($nameplate->{'serialNumber'}, $config->{'serial'}, 'PDU Serial Number');

    my $firmware_version = $metadata->{'fwRevision'};
    print "Device Firmware Version: $firmware_version\n";
    is ($firmware_version, $expected_firmware_version, 'Firmware Version');

    my $name = $pdu->getSettings()->{'name'};
    is($name, $config->{'name'}, 'PDU Name');
}

sub verify_snmp($) {
    my ($config) = @_;
    print "Verifying SNMP Access ...\n";

    `snmpget -M+. -m+PDU2-MIB -v 2c -c '$snmp_read_community' $config->{'ip'} PDU2-MIB:pduName.1`;
    is($?, 0, "SNMP Read Community");

    `snmpget -M+. -m+PDU2-MIB -v 2c -c '$snmp_write_community' $config->{'ip'} PDU2-MIB:pduName.1`;
    is($?, 0, "SNMP Write Community");
}

sub verify_single($) {
    my ($config) = @_;
    print "### Verifying $config->{'ip'} ###\n";
    $agent->{'url'} = "https://$config->{'ip'}";

    try {
	my $pdu = $agent->createProxy('/model/pdu/0');
	verify_pdu($pdu, $config);
	verify_snmp($config);
    } catch Error with {
	my $e = shift;
	print $e;
	fail("Exception caught while verifying!");
    };
}

sub do_verify($) {
    my ($ips) = @_;
    if (scalar @{$ips} == 0) {
	print STDERR "Usage: $0 verify LIST IPS\n";
	exit(1);
    }

    my @configs;
    if ($ips->[0] eq 'all') {
	@configs = @pdu_list;
    } else {
	@configs = ();
	foreach my $ip (@{$ips}) {
	    push(@configs, grep { $_->{'ip'} eq $ip } @pdu_list);
	}
    }

    foreach my $config (@configs) {
	verify_single($config);
    }

    my $num_failed = grep !$_->{'ok'}, @{$test_builder->{Test_Results}}[ 0 .. $test_builder->{Curr_Test} - 1 ];
    print "\nSummary: $num_failed test(s) failed\n";
}

sub identify_single($) {
    my ($ip) = @_;
    print "### Identifying $ip ###\n";
    $agent->{'url'} = "https://$ip";

    my $unit = $agent->createProxy('/model/unit');

    try {
	$unit->identify(2);
	sleep(2);
    } catch Error with {
	my $e = shift;
	print $e;
	fail("Exception caught while identifying!");
    };
}

sub do_identify($) {
    my ($ips) = @_;
    if (scalar @{$ips} == 0) {
	print STDERR "Usage: $0 identify LIST IPS\n";
	exit(1);
    }
    if ($ips->[0] eq 'all') {
	$ips = all_ips();
    }
    foreach my $ip (@{$ips}) {
	identify_single($ip);
    }
}

sub reboot_single($) {
    my ($ip) = @_;
    print "### Rebooting $ip ###\n";
    $agent->{'url'} = "https://$ip";

    my $firmware = $agent->createProxy('/firmware');

    try {
	$firmware->reboot();
    } catch Error with {
	my $e = shift;
	print $e;
	fail("Exception caught while rebooting!");
    };
}

sub do_reboot($) {
    my ($ips) = @_;
    if (scalar @{$ips} == 0) {
	print STDERR "Usage: $0 reboot LIST IPS\n";
	exit(1);
    }
    if ($ips->[0] eq 'all') {
	$ips = all_ips();
    }
    foreach my $ip (@{$ips}) {
	reboot_single($ip);
    }
}

sub fetch_fitness_single($$) {
    my ($fh, $ip) = @_;
    print "### Fetching fitness data from $ip ###\n";
    $agent->{'url'} = "https://$ip";

    try {
	my $fitness = $agent->createProxy("/fitness");

	my $dataEntries = $fitness->getDataEntries();
	if (scalar @{$dataEntries} > 0) {
	    print $fh "# ip,-,id,value,maxValue,worstValue,thresholdValue,rawValue,flags\n";
	}
	foreach my $entry (@{$dataEntries}) {
	    printf $fh "%s,data,%s,%d,%d,%d,%d,%d,%d\n",
		    $ip, $entry->{'id'},
		    $entry->{'value'}, $entry->{'maxValue'}, $entry->{'worstValue'},
		    $entry->{'thresholdValue'}, $entry->{'rawValue'}, $entry->{'flags'};
	}

	my ($first, $count);
	$fitness->getErrorLogIndexRange(\$first, \$count);
	my $errorLogEntries = $fitness->getErrorLogEntries($first, $count);
	if (scalar @{$errorLogEntries} > 0) {
	    print $fh "# ip,-,id,value,thresholdValue,rawValue,powerOnHours,timeStampUTC\n";
	}
	foreach my $entry (@{$errorLogEntries}) {
	    printf $fh "%s,error,%s,%d,%d,%d,%d,%d\n",
		    $ip, $entry->{'id'},
		    $entry->{'value'}, $entry->{'thresholdValue'},
		    $entry->{'rawValue'}, $entry->{'powerOnHours'}, $entry->{'timeStampUTC'};
	}
    } catch Error with {
	my $e = shift;
	print $e;
	fail("Exception caught while fetching fitness data!");
    };
}

sub do_fetch_fitness($) {
    my ($args) = @_;
    if (scalar @{$args} < 2) {
	print STDERR "Usage: $0 fetch_fitness LIST OUTFILE IPS\n";
	exit(1);
    }

    my $filename = shift(@{$args});

    my @pdus;
    if ($args->[0] eq 'all') {
	$args = all_ips();
    }
    my $fh = FileHandle->new(">$filename") or die "Can't open output file '$filename'";
    foreach my $ip (@{$args}) {
	fetch_fitness_single($fh, $ip);
    }
}

sub all_check_reachable($) {
    my ($pdus) = @_;
    foreach my $pdu (@{$pdus}) {
	try {
	    $pdu->{'firmware'} = $pdu->{'agent'}->createProxy('/firmware');
	    $pdu->{'old_version'} = $pdu->{'firmware'}->getVersion();
	    $pdu->{'status'} = 'ok';
	    print "$pdu->{'ip'}: Firmware version: $pdu->{'old_version'}\n";
	} catch Error with {
	    my $e = shift;
	    print $e;
	    print "$pdu->{'ip'}: Unreachable\n";
	    $pdu->{'status'} = 'unreachable';
	};
    }
}

sub all_upload_image($$) {
    my ($pdus, $image) = @_;
    foreach my $pdu (grep { $_->{'status'} eq 'ok' } @{$pdus}) {
	my $session;
	my $token;
	my $sessionmgr = $pdu->{'agent'}->createProxy('/session');
	$sessionmgr->newSession(\$session, \$token);
	my $url = "https://$pdu->{'ip'}/cgi-bin/fwupload.cgi?session=$token";

	my $ssl_opts = $disable_tls_certificate_check ? {
	    verify_hostname => 0,
	    SSL_verify_mode => IO::Socket::SSL::SSL_VERIFY_NONE
	} : undef;
	my $ua = LWP::UserAgent->new( timeout => 60, ssl_opts => $ssl_opts );
	my $req = POST $url,
		    Content_Type => 'form-data',
		    Content => [ submit => 1, upfile => [ $image ] ];
	my $rsp = $ua->request($req);
	if ($rsp->is_success()) {
	    print "$pdu->{'ip'}: Uploaded firmware image\n";
	} else {
	    print "$pdu->{'ip'}: Upload failed\n";
	    $pdu->{'status'} = 'upload_failed';
	}
    }
}

sub all_trigger_update($) {
    my ($pdus) = @_;
    foreach my $pdu (grep { $_->{'status'} eq 'ok' } @{$pdus}) {
	try {
	    $pdu->{'firmware'}->startUpdate([]);
	    print "$pdu->{'ip'}: Update started\n";
	    $pdu->{'status'} = 'update_started';
	} catch Error with {
	    my $e = shift;
	    print $e;
	    print "$pdu->{'ip'}: Failed to start update\n";
	    $pdu->{'status'} = 'start_failed';
	};
    }
}

sub all_wait_complete($) {
    my ($pdus) = @_;
    my $pending;
    my $deadline = time() + 600; # Wait for up to 10 minutes

    do {
	$pending = 0;
	foreach my $pdu (@{$pdus}) {
	    if ($pdu->{'status'} eq 'update_started' ||
		$pdu->{'status'} eq 'update_succeeded') {
		try {
		    my $state = $pdu->{'fwupdate_status'}->getStatus()->{'state'};
		    if ($state eq 'NONE') {
			print "$pdu->{'ip'}: Update complete\n";
			$pdu->{'status'} = 'complete';
		    } elsif ($state eq 'FAIL') {
			print "$pdu->{'ip'}: Update failed\n";
			$pdu->{'status'} = 'update_failed';
		    } elsif ($state eq 'SUCCESS') {
			if ($pdu->{'status'} ne 'update_succeeded') {
			    print "$pdu->{'ip'}: Update succeeded, device will now reboot ...\n";
			    $pdu->{'status'} = 'update_succeeded';
			}
			$pending++;
		    } else {
			$pending++;
		    }
		} catch Error with {
		    # No problem, timeouts are expected when device reboots
		    # after update.
		    $pending++;
		};
	    }
	}
	sleep(2);
    } while ($pending > 0 && time() <= $deadline);

    if ($pending > 0) {
	print "TIMEOUT: $pending devices pending!\n";
    }
}

sub all_get_new_version($) {
    my ($pdus) = @_;
    foreach my $pdu (grep { $_->{'status'} eq 'complete' } @{$pdus}) {
	try {
	    $pdu->{'new_version'} = $pdu->{'firmware'}->getVersion();
	} catch Error with {
	    my $e = shift;
	    print $e;
	    print "$pdu->{'ip'}: Unreachable after successful update\n";
	    $pdu->{'status'} = 'unreachable2';
	};
    }
}

sub all_print_status($) {
    my ($pdus) = @_;
    print "==== Update Summary ====\n";
    foreach my $pdu (@{$pdus}) {
	my $ip = $pdu->{'ip'};
	my $status = $pdu->{'status'};
	if ($status eq 'complete') {
	    my $old = $pdu->{'old_version'};
	    my $new = $pdu->{'new_version'};
	    print("$ip: Successfully upgraded from $old to $new.\n");
	} elsif ($status eq 'unreachable') {
	    print("$ip: ERROR: Could not connect to device\n");
	} elsif ($status eq 'upload_failed') {
	    print("$ip: ERROR: Failed to upload image\n");
	} elsif ($status eq 'update_failed') {
	    print("$ip: ERROR: Firmware update failed\n");
	} elsif ($status eq 'update_started') {
	    print("$ip: ERROR: Timeout during update\n");
	} elsif ($status eq 'update_succeeded') {
	    print("$ip: ERROR: Device did not reboot after update\n");
	} elsif ($status eq 'unreachable2') {
	    print("$ip: ERROR: Device unreachable after successful update\n");
	} else {
	    print("$ip: ERROR: $status\n");
	}
    }
}

sub do_update($) {
    my ($args) = @_;
    if (scalar @{$args} < 2) {
	print STDERR "Usage: $0 update LIST IMAGE IPS\n";
	exit(1);
    }

    my $image = shift(@{$args});

    my @pdus;
    if ($args->[0] eq 'all') {
	$args = all_ips();
    }
    foreach my $ip (@{$args}) {
	my $agent = new Raritan::RPC::Agent("https://$ip", $admin_username, $admin_password, $disable_tls_certificate_check);
	my $fwupdate_status = new Raritan::RPC::firmware::FirmwareUpdateStatus($agent, '/cgi-bin/fwupdate_progress.cgi');
	my $pdu = {
	    'agent' => $agent,
	    'ip' => $ip,
	    'fwupdate_status' => $fwupdate_status,
	};
	push(@pdus, $pdu);
    }

    all_check_reachable(\@pdus);
    all_upload_image(\@pdus, $image);
    all_trigger_update(\@pdus);
    all_wait_complete(\@pdus);
    all_get_new_version(\@pdus);
    all_print_status(\@pdus);
}

sub tz_sort($$) {
    my $tz1Name = shift;
    my $tz2Name = shift;

    my $result;
    if ($tz1Name =~ /^\((UTC|GMT)/ && $tz1Name =~ /^\((UTC|GMT)/) {
	my $tz1Offset = 0;
	my $tz2Offset = 0;
	my $tz1Char4 = substr($tz1Name, 4, 1);
	my $tz2Char4 = substr($tz2Name, 4, 1);
	if ($tz1Char4 ne ")") {
	    $tz1Offset = substr($tz1Name, 5, 2) . substr($tz1Name, 8, 2);
	    if ($tz1Char4 eq "-") {
		$tz1Offset = -$tz1Offset;
	    }
	}
	if ($tz2Char4 ne ")") {
	    $tz2Offset = substr($tz2Name, 5, 2) . substr($tz2Name, 8, 2);
	    if ($tz2Char4 eq "-") {
		$tz2Offset = -$tz2Offset;
	    }
	}
	if ($tz1Offset == $tz2Offset) {
	    if ($tz1Offset < 0) {
		$result = $tz1Name cmp $tz2Name;
	    } else {
		$result = $tz1Name cmp $tz2Name;
	    }	
	} else {
	    $result = ($tz1Offset - $tz2Offset) < 0 ? -1 : 1;
	}
    } else {
	$result = $tz1Name cmp $tz2Name;
    }
    return $result;
}

sub do_print_timezones($) {
    my ($args) = @_;
    if (scalar @{$args} != 1) {
	print STDERR "Usage: $0 print_timezones IP\n";
	exit(1);
    }

    my $ip = $args->[0];
    print "### Getting timezone data for $ip ###\n";
    $agent->{'url'} = "https://$ip";

    my $datetime = $agent->createProxy("/datetime");
    my $zoneinfos;
    my %zoneinfo_by_name;

    try {
	$datetime->getZoneInfos(\$zoneinfos, 0);
	foreach my $zoneinfo (@{$zoneinfos}) {
	    $zoneinfo_by_name{$zoneinfo->{"name"}} = $zoneinfo;
	}
	foreach my $zone_name (sort tz_sort keys %zoneinfo_by_name) {
	    my $zoneinfo = $zoneinfo_by_name{$zone_name};
	    print "id: " . $zoneinfo->{"id"} . "\tname: " . $zoneinfo->{"name"} . "\n";
	}
    } catch Error with {
	my $e = shift;
	print $e;
	fail("Exception caught while getting timezone data!");
	return;
    };
}

sub set_timezone_single($$) {
    my ($ip, $tzid) = @_;
    print "### Setting timezone for $ip ###\n";
    $agent->{'url'} = "https://$ip";

    my $datetime = $agent->createProxy("/datetime");
    my $zoneinfos;
    my $found = 0;

    try {
	$datetime->getZoneInfos(\$zoneinfos, 0);
	foreach my $zoneinfo (@{$zoneinfos}) {
	    if ($tzid == $zoneinfo->{"id"}) {
		$found = 1;
		my $dateTimeCfg;
		try {
		    $datetime->getCfg(\$dateTimeCfg);
		} catch Error with {
		    my $e = shift;
		    print $e;
		    fail("Exception caught while getting date/time cfg!");
		    return;
		};
		$dateTimeCfg->{"zoneCfg"}->{"id"} = $tzid;
		$dateTimeCfg->{"zoneCfg"}->{"name"} = "";
		try {
		    if ($datetime->setCfg($dateTimeCfg) != 0) {
			fail("Setting date/time cfg failed!");
			return;
		    }
		} catch Error with {
		    my $e = shift;
		    print $e;
		    fail("Exception caught while setting date/time cfg!");
		    return;
		}
	    }
	}
	if (!$found) {
	    fail("Timezone id $tzid invalid!");
	    return;
	}
    } catch Error with {
	my $e = shift;
	print $e;
	fail("Exception caught while getting timezone data!");
    };
}

sub do_set_timezone($) {
    my ($args) = @_;
    if (scalar @{$args} < 2) {
	print STDERR "Usage: $0 set_timezone LIST TZID IPS\n";
	exit(1);
    }

    my $tzid = shift(@{$args});

    my ($ips) = @_;
    if ($ips->[0] eq 'all') {
	$ips = all_ips();
    }
    foreach my $ip (@{$ips}) {
	set_timezone_single($ip, $tzid);
    }
}

my $command = shift;
my $pdu_list;
if ($command ne 'print_timezones') {
    $pdu_list = shift;
}
my @args = @ARGV;

if (!defined($command) || !defined($pdu_list)) {
    if (defined($command) && ($command ne 'print_timezones')) {
	usage();
	exit(1);
    }
}

if (defined($pdu_list)) {
    parse_config($pdu_list);
}

if ($command eq 'configure') {
    do_configure(\@args);
} elsif ($command eq 'verify') {
    do_verify(\@args);
} elsif ($command eq 'identify') {
    do_identify(\@args);
} elsif ($command eq 'reboot') {
    do_reboot(\@args);
} elsif ($command eq 'fetch_fitness') {
    do_fetch_fitness(\@args);
} elsif ($command eq 'update') {
    do_update(\@args);
} elsif ($command eq 'print_timezones') {
    do_print_timezones(\@args);
} elsif ($command eq 'set_timezone') {
    do_set_timezone(\@args);
} else {
    print STDERR "Unknown command: $command\n";
    usage();
    exit(1);
}

__END__

=head1 SYNOPSIS

px2_configure.pl COMMAND [ARGUMENTS]

=head1 DESCRIPTION

PX2 deployment and configuration utility.

=head1 COMMANDS

=over

=item B<configure LIST IPS>

Configure some or all devices (network settings, unit name and SNMP
settings).

The behavior of this command is different depending on whether concrete IP
addresses or the 'all' keyword is specified:

=over 2

=item * If IP addresses are specified the program will contact each device,
read its serial number and search for a matching entry in the device list. It
will then change the device's network settings to the values from the device
list. The initial IP addresses do not have to match the values from the device
list, they might as well be dynamically assigned by a DHCP server.

=item * If the 'all' keyword is specified the IP addresses are taken from the
device list. The devices' network settings are expected to be properly
configured at this point, so the network configuration part will be skipped.

=back

=item B<verify LIST IPS>

Verify device settings and SNMP access.

=item B<identify LIST IPS>

Identify some or all devices (show a distinctive string for two seconds in
each unit's display).

=item B<reboot LIST IPS>

Reboot some or all devices.

=item B<fetch_fitness LIST OUTFILE IPS>

Fetch fitness data from some or all devices and write them to a file in
CSV format.

=item B<update LIST IMAGE IPS>

Device firmware update. The firmware image will be sequentially uploaded to
each device, then all devices will be instructed to perform the firmware
update in parallel.

=item B<print_timezones IP>

Print list of supported timezones with its IDs.

=item B<set_timezone LIST TZID IPS>

Set the timezone of all devices.

=back

=head1 ARGUMENTS

=over

=item B<LIST>

Device list in CSV format.

=item B<IPS>

Device IP addresses; use 'all' to run on all IPs in the CSV file.

=item B<IP>

Device IP address.

=item B<OUTFILE>

Output file name.

=item B<IMAGE>

Firmware image file.

=item B<TZID>

Timezone ID as printed by the "print_timezones" command.

=back

=head1 EXAMPLES

=over

=item B<px2_configure.pl configure devices.csv 192.168.2.3>

Connect to the device at 192.168.2.3, read its serial number and look it up in
the device list file named F<devices.csv>. Configure the device's network, PDU
name and SNMP settings.

=item B<px2_configure.pl verify devices.csv all>

Verify the network settings, PDU name and SNMP access for all devices in the
list file.

=item B<px2_configure.pl update devices.csv pdu-px2-020106-26030.bin all>

Update all devices to firmware version 2.1.6.

=back

=head1 DEVICE LIST FORMAT

The device list file contains a list of PX2 devices in CSV format. It could
e.g. be exported from an inventory spreadsheet. Each device line contains five
or six fields, seperated by commas:

=over

=item 1. Unit Serial Number

=item 2. PDU Name

=item 3. IP Address

=item 4. Gateway IP Address

=item 5. Netmask

=item 6. Optional comment; the line is ignored if "ignore" is included here

=back

Lines with less than five fields are silently ignored. Likewise, lines that do
not contain a valid serial number (i.e. a string starting with 'P') are
disregarded.

=head1 REQUIREMENTS

The following Perl modules are required to run this script:

=over 2

=item * Raritan PX2 RPC bindings (distributed along with this script)

=item * L<libwww-perl|http://search.cpan.org/dist/libwww-perl/>

=item * L<LWP::Protocol::https|http://search.cpan.org/dist/LWP-Protocol-https/>

=item * L<JSON|http://search.cpan.org/dist/JSON/>

=item * L<JSON-RPC-Common|http://search.cpan.org/dist/JSON-RPC-Common/>

=item * L<perl-Error|http://search.cpan.org/dist/Error/>

=item * L<Test-Simple|http://search.cpan.org/dist/Test-Simple/>

=back

Additionally, the B<verify> command uses the I<snmpget> utility to verify
SNMP access.

=cut
