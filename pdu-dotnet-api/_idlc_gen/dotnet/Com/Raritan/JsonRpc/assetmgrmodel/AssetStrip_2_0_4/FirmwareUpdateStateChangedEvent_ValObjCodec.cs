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
  public class FirmwareUpdateStateChangedEvent_ValObjCodec : Com.Raritan.JsonRpc.idl.Event_ValObjCodec {

    public static new void Register() {
      ValueObjectCodec.RegisterCodec(Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.FirmwareUpdateStateChangedEvent.typeInfo, new FirmwareUpdateStateChangedEvent_ValObjCodec());
    }

    public override void EncodeValObj(LightJson.JsonObject json, ValueObject vo) {
      base.EncodeValObj(json, vo);
      var inst = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.FirmwareUpdateStateChangedEvent)vo;
      json["state"] = (int)inst.state;
    }

    public override ValueObject DecodeValObj(ValueObject vo, LightJson.JsonObject json, Agent agent) {
      Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.FirmwareUpdateStateChangedEvent inst;
      if (vo == null) {
        inst = new Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.FirmwareUpdateStateChangedEvent();
      } else {
        inst = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.FirmwareUpdateStateChangedEvent)vo;
      }
      base.DecodeValObj(vo, json, agent);
      inst.state = (Com.Raritan.Idl.assetmgrmodel.AssetStrip_2_0_4.FirmwareUpdateState)(int)json["state"];
      return inst;
    }

  }
}
