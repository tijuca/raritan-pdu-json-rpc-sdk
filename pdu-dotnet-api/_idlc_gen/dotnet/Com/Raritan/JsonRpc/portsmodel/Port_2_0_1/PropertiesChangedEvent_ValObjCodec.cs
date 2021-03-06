// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Port.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.JsonRpc.portsmodel.Port_2_0_1 {
  public class PropertiesChangedEvent_ValObjCodec : Com.Raritan.JsonRpc.idl.Event_ValObjCodec {

    public static new void Register() {
      ValueObjectCodec.RegisterCodec(Com.Raritan.Idl.portsmodel.Port_2_0_1.PropertiesChangedEvent.typeInfo, new PropertiesChangedEvent_ValObjCodec());
    }

    public override void EncodeValObj(LightJson.JsonObject json, ValueObject vo) {
      base.EncodeValObj(json, vo);
      var inst = (Com.Raritan.Idl.portsmodel.Port_2_0_1.PropertiesChangedEvent)vo;
      json["oldProperties"] = inst.oldProperties.Encode();
      json["newProperties"] = inst.newProperties.Encode();
    }

    public override ValueObject DecodeValObj(ValueObject vo, LightJson.JsonObject json, Agent agent) {
      Com.Raritan.Idl.portsmodel.Port_2_0_1.PropertiesChangedEvent inst;
      if (vo == null) {
        inst = new Com.Raritan.Idl.portsmodel.Port_2_0_1.PropertiesChangedEvent();
      } else {
        inst = (Com.Raritan.Idl.portsmodel.Port_2_0_1.PropertiesChangedEvent)vo;
      }
      base.DecodeValObj(vo, json, agent);
      inst.oldProperties = Com.Raritan.Idl.portsmodel.Port_2_0_1.Properties.Decode(json["oldProperties"], agent);
      inst.newProperties = Com.Raritan.Idl.portsmodel.Port_2_0_1.Properties.Decode(json["newProperties"], agent);
      return inst;
    }

  }
}
