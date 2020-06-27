// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Switch.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.JsonRpc.sensors.Switch_2_0_4 {
  public class SwitchEvent_ValObjCodec : Com.Raritan.JsonRpc._event.UserEvent_ValObjCodec {

    public static new void Register() {
      ValueObjectCodec.RegisterCodec(Com.Raritan.Idl.sensors.Switch_2_0_4.SwitchEvent.typeInfo, new SwitchEvent_ValObjCodec());
    }

    public override void EncodeValObj(LightJson.JsonObject json, ValueObject vo) {
      base.EncodeValObj(json, vo);
      var inst = (Com.Raritan.Idl.sensors.Switch_2_0_4.SwitchEvent)vo;
      json["targetState"] = inst.targetState;
    }

    public override ValueObject DecodeValObj(ValueObject vo, LightJson.JsonObject json, Agent agent) {
      Com.Raritan.Idl.sensors.Switch_2_0_4.SwitchEvent inst;
      if (vo == null) {
        inst = new Com.Raritan.Idl.sensors.Switch_2_0_4.SwitchEvent();
      } else {
        inst = (Com.Raritan.Idl.sensors.Switch_2_0_4.SwitchEvent)vo;
      }
      base.DecodeValObj(vo, json, agent);
      inst.targetState = (int)json["targetState"];
      return inst;
    }

  }
}