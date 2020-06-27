// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Pole.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.pdumodel {

  public class DoublePole_1_0_1 : ICloneable {
    public object Clone() {
      DoublePole_1_0_1 copy = new DoublePole_1_0_1();
      copy.label = this.label;
      copy.line = this.line;
      copy.inNodeId = this.inNodeId;
      copy.outNodeId = this.outNodeId;
      copy.voltage = this.voltage;
      copy.voltageLN = this.voltageLN;
      copy.current = this.current;
      copy.peakCurrent = this.peakCurrent;
      copy.activePower = this.activePower;
      copy.apparentPower = this.apparentPower;
      copy.powerFactor = this.powerFactor;
      copy.activeEnergy = this.activeEnergy;
      copy.apparentEnergy = this.apparentEnergy;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["label"] = this.label;
      json["line"] = (int)this.line;
      json["inNodeId"] = this.inNodeId;
      json["outNodeId"] = this.outNodeId;
      json["voltage"] = this.voltage != null ? this.voltage.Encode() : JsonValue.Null;
      json["voltageLN"] = this.voltageLN != null ? this.voltageLN.Encode() : JsonValue.Null;
      json["current"] = this.current != null ? this.current.Encode() : JsonValue.Null;
      json["peakCurrent"] = this.peakCurrent != null ? this.peakCurrent.Encode() : JsonValue.Null;
      json["activePower"] = this.activePower != null ? this.activePower.Encode() : JsonValue.Null;
      json["apparentPower"] = this.apparentPower != null ? this.apparentPower.Encode() : JsonValue.Null;
      json["powerFactor"] = this.powerFactor != null ? this.powerFactor.Encode() : JsonValue.Null;
      json["activeEnergy"] = this.activeEnergy != null ? this.activeEnergy.Encode() : JsonValue.Null;
      json["apparentEnergy"] = this.apparentEnergy != null ? this.apparentEnergy.Encode() : JsonValue.Null;
      return json;
    }

    public static DoublePole_1_0_1 Decode(LightJson.JsonObject json, Agent agent) {
      DoublePole_1_0_1 inst = new DoublePole_1_0_1();
      inst.label = (string)json["label"];
      inst.line = (Com.Raritan.Idl.pdumodel.PowerLine)(int)json["line"];
      inst.inNodeId = (int)json["inNodeId"];
      inst.outNodeId = (int)json["outNodeId"];
      inst.voltage = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["voltage"], agent));
      inst.voltageLN = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["voltageLN"], agent));
      inst.current = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["current"], agent));
      inst.peakCurrent = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["peakCurrent"], agent));
      inst.activePower = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["activePower"], agent));
      inst.apparentPower = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["apparentPower"], agent));
      inst.powerFactor = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["powerFactor"], agent));
      inst.activeEnergy = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["activeEnergy"], agent));
      inst.apparentEnergy = Com.Raritan.Idl.sensors.NumericSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["apparentEnergy"], agent));
      return inst;
    }

    public string label = "";
    public Com.Raritan.Idl.pdumodel.PowerLine line = Com.Raritan.Idl.pdumodel.PowerLine.L1;
    public int inNodeId = 0;
    public int outNodeId = 0;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 voltage = null;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 voltageLN = null;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 current = null;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 peakCurrent = null;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 activePower = null;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 apparentPower = null;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 powerFactor = null;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 activeEnergy = null;
    public Com.Raritan.Idl.sensors.NumericSensor_3_0_1 apparentEnergy = null;
  }
}