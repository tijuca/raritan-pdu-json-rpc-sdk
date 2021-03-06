// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from PeripheralDeviceManager.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.JsonRpc.peripheral.DeviceManager_2_0_1 {
  public class DeviceAddedEvent_ValObjCodec : Com.Raritan.JsonRpc.peripheral.DeviceManager_2_0_1.DeviceEvent_ValObjCodec {

    public static new void Register() {
      ValueObjectCodec.RegisterCodec(Com.Raritan.Idl.peripheral.DeviceManager_2_0_1.DeviceAddedEvent.typeInfo, new DeviceAddedEvent_ValObjCodec());
    }

    public override void EncodeValObj(LightJson.JsonObject json, ValueObject vo) {
      base.EncodeValObj(json, vo);
      var inst = (Com.Raritan.Idl.peripheral.DeviceManager_2_0_1.DeviceAddedEvent)vo;
    }

    public override ValueObject DecodeValObj(ValueObject vo, LightJson.JsonObject json, Agent agent) {
      Com.Raritan.Idl.peripheral.DeviceManager_2_0_1.DeviceAddedEvent inst;
      if (vo == null) {
        inst = new Com.Raritan.Idl.peripheral.DeviceManager_2_0_1.DeviceAddedEvent();
      } else {
        inst = (Com.Raritan.Idl.peripheral.DeviceManager_2_0_1.DeviceAddedEvent)vo;
      }
      base.DecodeValObj(vo, json, agent);
      return inst;
    }

  }
}
