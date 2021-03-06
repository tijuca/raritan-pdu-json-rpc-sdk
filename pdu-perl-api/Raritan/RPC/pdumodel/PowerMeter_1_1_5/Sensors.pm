# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from PowerMeter.idl.

use strict;

package Raritan::RPC::pdumodel::PowerMeter_1_1_5::Sensors;


sub encode {
    my ($in) = @_;
    my $encoded = {};
    $encoded->{'voltage'} = Raritan::RPC::ObjectCodec::encode($in->{'voltage'});
    $encoded->{'lineFrequency'} = Raritan::RPC::ObjectCodec::encode($in->{'lineFrequency'});
    $encoded->{'current'} = Raritan::RPC::ObjectCodec::encode($in->{'current'});
    $encoded->{'activePower'} = Raritan::RPC::ObjectCodec::encode($in->{'activePower'});
    $encoded->{'reactivePower'} = Raritan::RPC::ObjectCodec::encode($in->{'reactivePower'});
    $encoded->{'apparentPower'} = Raritan::RPC::ObjectCodec::encode($in->{'apparentPower'});
    $encoded->{'powerFactor'} = Raritan::RPC::ObjectCodec::encode($in->{'powerFactor'});
    $encoded->{'phaseAngle'} = Raritan::RPC::ObjectCodec::encode($in->{'phaseAngle'});
    $encoded->{'displacementPowerFactor'} = Raritan::RPC::ObjectCodec::encode($in->{'displacementPowerFactor'});
    $encoded->{'activeEnergy'} = Raritan::RPC::ObjectCodec::encode($in->{'activeEnergy'});
    $encoded->{'unbalancedCurrent'} = Raritan::RPC::ObjectCodec::encode($in->{'unbalancedCurrent'});
    $encoded->{'crestFactor'} = Raritan::RPC::ObjectCodec::encode($in->{'crestFactor'});
    $encoded->{'powerQuality'} = Raritan::RPC::ObjectCodec::encode($in->{'powerQuality'});
    return $encoded;
}

sub decode {
    my ($agent, $in) = @_;
    my $decoded = {};
    $decoded->{'voltage'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'voltage'}, 'sensors.NumericSensor');
    $decoded->{'lineFrequency'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'lineFrequency'}, 'sensors.NumericSensor');
    $decoded->{'current'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'current'}, 'sensors.NumericSensor');
    $decoded->{'activePower'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'activePower'}, 'sensors.NumericSensor');
    $decoded->{'reactivePower'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'reactivePower'}, 'sensors.NumericSensor');
    $decoded->{'apparentPower'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'apparentPower'}, 'sensors.NumericSensor');
    $decoded->{'powerFactor'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'powerFactor'}, 'sensors.NumericSensor');
    $decoded->{'phaseAngle'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'phaseAngle'}, 'sensors.NumericSensor');
    $decoded->{'displacementPowerFactor'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'displacementPowerFactor'}, 'sensors.NumericSensor');
    $decoded->{'activeEnergy'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'activeEnergy'}, 'sensors.NumericSensor');
    $decoded->{'unbalancedCurrent'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'unbalancedCurrent'}, 'sensors.NumericSensor');
    $decoded->{'crestFactor'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'crestFactor'}, 'sensors.NumericSensor');
    $decoded->{'powerQuality'} = Raritan::RPC::ObjectCodec::decode($agent, $in->{'powerQuality'}, 'sensors.StateSensor');
    return $decoded;
}

1;
