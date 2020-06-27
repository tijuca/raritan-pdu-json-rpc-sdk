#!/usr/bin/perl -w
# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2015 Raritan Inc. All rights reserved.

use strict;
use warnings;

use lib 'pdu-perl-api';
use Raritan::RPC;

my $ip = $ARGV[0] || "10.0.42.2";
my $user = $ARGV[1] || "admin";
my $pw = $ARGV[2] || "raritan";

my $agent = new Raritan::RPC::Agent("https://" . $ip, $user, $pw, 1); # 1 disables cert check

my $pdu = $agent->createProxy("/model/pdu/0");
my $firmware_proxy = $agent->createProxy("/firmware");

my $inlets = $pdu->getInlets();
my $ocps = $pdu->getOverCurrentProtectors();
my $outlets = $pdu->getOutlets();

print "PDU: $ip \n";
print "Firmware version: " . $firmware_proxy->getVersion() . "\n";

print "Number of inlets: " . scalar(@$inlets) . "\n";
print "Number of over current protectors: " . scalar(@$ocps) . "\n";
print "Number of outlets: " . scalar(@$outlets) . "\n";

my $outlet = $outlets->[0];
my $outlet_sensors = $outlet->getSensors();
my $outlet_metadata = $outlet->getMetaData();
my $outlet_settings = $outlet->getSettings();

print "Outlet " . $outlet_metadata->{label} . ":\n";
print "  Name: " . (($outlet_settings->{name}) ? $outlet_settings->{name} : "(none)") . "\n";
print "  Switchable: " . (($outlet_metadata->{isSwitchable}) ? "yes" : "no") . "\n";

if ($outlet_sensors->{voltage}) {
    my $sensor_reading = $outlet_sensors->{voltage}->getReading();
    print "  Voltage: " . (($sensor_reading->{valid}) ? $sensor_reading->{value} : "n/a") . " V\n";
}

if ($outlet_sensors->{current}) {
    my $sensor_reading = $outlet_sensors->{current}->getReading();
    print "  Current: " . (($sensor_reading->{valid}) ? $sensor_reading->{value} : "n/a") . " A\n";
}

if ($outlet_metadata->{isSwitchable}) {
    my $outlet_state_sensor = $outlet_sensors->{outletState};
    my $outlet_state = $outlet_state_sensor->getState();
    if ($outlet_state->{available}) {
        print "  Status: " . (($outlet_state->{value} == Raritan::RPC::sensors::Sensor::OnOffState::ON) ? "on" : "off") . "\n";
    }
    print "  Turning outlet off...\n";
    $outlet->setPowerState(Raritan::RPC::pdumodel::Outlet::PowerState::PS_OFF);
    print "  Sleeping 4 seconds...\n";
    sleep(4);
    print "  Turning outlet on...\n";
    $outlet->setPowerState(Raritan::RPC::pdumodel::Outlet::PowerState::PS_ON);
    $outlet_state = $outlet_state_sensor->getState();
    if ($outlet_state->{available}) {
        print "  Status: " . (($outlet_state->{value} == Raritan::RPC::sensors::Sensor::OnOffState::ON) ? "on" : "off") . "\n";
    }
}
