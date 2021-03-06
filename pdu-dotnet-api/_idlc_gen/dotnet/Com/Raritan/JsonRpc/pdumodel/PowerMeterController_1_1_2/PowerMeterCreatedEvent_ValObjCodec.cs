// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from PowerMeterController.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.JsonRpc.pdumodel.PowerMeterController_1_1_2 {
  public class PowerMeterCreatedEvent_ValObjCodec : Com.Raritan.JsonRpc._event.UserEvent_ValObjCodec {

    public static new void Register() {
      ValueObjectCodec.RegisterCodec(Com.Raritan.Idl.pdumodel.PowerMeterController_1_1_2.PowerMeterCreatedEvent.typeInfo, new PowerMeterCreatedEvent_ValObjCodec());
    }

    public override void EncodeValObj(LightJson.JsonObject json, ValueObject vo) {
      base.EncodeValObj(json, vo);
      var inst = (Com.Raritan.Idl.pdumodel.PowerMeterController_1_1_2.PowerMeterCreatedEvent)vo;
      json["powerMeter"] = inst.powerMeter != null ? inst.powerMeter.Encode() : JsonValue.Null;
      json["config"] = inst.config.Encode();
      json["settings"] = inst.settings.Encode();
    }

    public override ValueObject DecodeValObj(ValueObject vo, LightJson.JsonObject json, Agent agent) {
      Com.Raritan.Idl.pdumodel.PowerMeterController_1_1_2.PowerMeterCreatedEvent inst;
      if (vo == null) {
        inst = new Com.Raritan.Idl.pdumodel.PowerMeterController_1_1_2.PowerMeterCreatedEvent();
      } else {
        inst = (Com.Raritan.Idl.pdumodel.PowerMeterController_1_1_2.PowerMeterCreatedEvent)vo;
      }
      base.DecodeValObj(vo, json, agent);
      inst.powerMeter = Com.Raritan.Idl.pdumodel.PowerMeter_1_1_2.StaticCast(ObjectProxy.Decode(json["powerMeter"], agent));
      inst.config = Com.Raritan.Idl.pdumodel.PowerMeter_1_1_2.Config.Decode(json["config"], agent);
      inst.settings = Com.Raritan.Idl.pdumodel.PowerMeter_1_1_2.Settings.Decode(json["settings"], agent);
      return inst;
    }

  }
}
