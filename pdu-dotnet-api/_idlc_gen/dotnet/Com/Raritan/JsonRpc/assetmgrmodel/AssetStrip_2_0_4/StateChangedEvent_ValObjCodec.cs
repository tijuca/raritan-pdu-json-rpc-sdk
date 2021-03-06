// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from AssetStrip.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.JsonRpc.assetmgrmodel.AssetStrip_2_0_4 {
  public class StateChangedEvent_ValObjCodec : Com.Raritan.JsonRpc.idl.Event_ValObjCodec {

    public static new void Register() {
      ValueObjectCodec.RegisterCodec(Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.StateChangedEvent.typeInfo, new StateChangedEvent_ValObjCodec());
    }

    public override void EncodeValObj(LightJson.JsonObject json, ValueObject vo) {
      base.EncodeValObj(json, vo);
      var inst = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.StateChangedEvent)vo;
      json["oldState"] = (int)inst.oldState;
      json["newState"] = (int)inst.newState;
      json["deviceInfo"] = inst.deviceInfo.Encode();
    }

    public override ValueObject DecodeValObj(ValueObject vo, LightJson.JsonObject json, Agent agent) {
      Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.StateChangedEvent inst;
      if (vo == null) {
        inst = new Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.StateChangedEvent();
      } else {
        inst = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.StateChangedEvent)vo;
      }
      base.DecodeValObj(vo, json, agent);
      inst.oldState = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.State)(int)json["oldState"];
      inst.newState = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.State)(int)json["newState"];
      inst.deviceInfo = Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.DeviceInfo.Decode(json["deviceInfo"], agent);
      return inst;
    }

  }
}
