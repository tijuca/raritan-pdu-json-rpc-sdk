// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from PeripheralDeviceSlot.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.peripheral {

  public class Address_2_0_0 : ICloneable {
    public object Clone() {
      Address_2_0_0 copy = new Address_2_0_0();
      copy.position = this.position;
      copy.type = this.type;
      copy.isActuator = this.isActuator;
      copy.channel = this.channel;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["position"] = new JsonArray(this.position.Select(
        _value => (JsonValue)(_value.Encode())));
      json["type"] = this.type.Encode();
      json["isActuator"] = this.isActuator;
      json["channel"] = this.channel;
      return json;
    }

    public static Address_2_0_0 Decode(LightJson.JsonObject json, Agent agent) {
      Address_2_0_0 inst = new Address_2_0_0();
      inst.position = new System.Collections.Generic.List<Com.Raritan.Idl.peripheral.PosElement>(json["position"].AsJsonArray.Select(
        _value => Com.Raritan.Idl.peripheral.PosElement.Decode(_value, agent)));
      inst.type = Com.Raritan.Idl.sensors.Sensor_4_0_0.TypeSpec.Decode(json["type"], agent);
      inst.isActuator = (bool)json["isActuator"];
      inst.channel = (int)json["channel"];
      return inst;
    }

    public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.peripheral.PosElement> position = new System.Collections.Generic.List<Com.Raritan.Idl.peripheral.PosElement>();
    public Com.Raritan.Idl.sensors.Sensor_4_0_0.TypeSpec type = new Com.Raritan.Idl.sensors.Sensor_4_0_0.TypeSpec();
    public bool isActuator = false;
    public int channel = 0;
  }
}
