// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from LhxSensor.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.JsonRpc.lhxmodel.Sensor_4_0_1 {
  public class ReadingChangedEvent_ValObjCodec : Com.Raritan.JsonRpc.idl.Event_ValObjCodec {

    public static new void Register() {
      ValueObjectCodec.RegisterCodec(Com.Raritan.Idl.lhxmodel.Sensor_4_0_1.ReadingChangedEvent.typeInfo, new ReadingChangedEvent_ValObjCodec());
    }

    public override void EncodeValObj(LightJson.JsonObject json, ValueObject vo) {
      base.EncodeValObj(json, vo);
      var inst = (Com.Raritan.Idl.lhxmodel.Sensor_4_0_1.ReadingChangedEvent)vo;
      json["newReading"] = inst.newReading.Encode();
    }

    public override ValueObject DecodeValObj(ValueObject vo, LightJson.JsonObject json, Agent agent) {
      Com.Raritan.Idl.lhxmodel.Sensor_4_0_1.ReadingChangedEvent inst;
      if (vo == null) {
        inst = new Com.Raritan.Idl.lhxmodel.Sensor_4_0_1.ReadingChangedEvent();
      } else {
        inst = (Com.Raritan.Idl.lhxmodel.Sensor_4_0_1.ReadingChangedEvent)vo;
      }
      base.DecodeValObj(vo, json, agent);
      inst.newReading = Com.Raritan.Idl.lhxmodel.Sensor_4_0_1.Reading.Decode(json["newReading"], agent);
      return inst;
    }

  }
}
