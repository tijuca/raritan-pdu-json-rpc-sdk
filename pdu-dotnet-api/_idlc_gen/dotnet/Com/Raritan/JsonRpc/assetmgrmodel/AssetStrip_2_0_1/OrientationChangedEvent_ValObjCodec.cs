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

namespace Com.Raritan.JsonRpc.assetmgrmodel.AssetStrip_2_0_1 {
  public class OrientationChangedEvent_ValObjCodec : Com.Raritan.JsonRpc.idl.Event_ValObjCodec {

    public static new void Register() {
      ValueObjectCodec.RegisterCodec(Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_1.OrientationChangedEvent.typeInfo, new OrientationChangedEvent_ValObjCodec());
    }

    public override void EncodeValObj(LightJson.JsonObject json, ValueObject vo) {
      base.EncodeValObj(json, vo);
      var inst = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_1.OrientationChangedEvent)vo;
      json["oldOrientation"] = (int)inst.oldOrientation;
      json["newOrientation"] = (int)inst.newOrientation;
    }

    public override ValueObject DecodeValObj(ValueObject vo, LightJson.JsonObject json, Agent agent) {
      Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_1.OrientationChangedEvent inst;
      if (vo == null) {
        inst = new Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_1.OrientationChangedEvent();
      } else {
        inst = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_1.OrientationChangedEvent)vo;
      }
      base.DecodeValObj(vo, json, agent);
      inst.oldOrientation = (Com.Raritan.Idl.assetmgrmodel.AssetStripConfig_1_0_1.Orientation)(int)json["oldOrientation"];
      inst.newOrientation = (Com.Raritan.Idl.assetmgrmodel.AssetStripConfig_1_0_1.Orientation)(int)json["newOrientation"];
      return inst;
    }

  }
}
