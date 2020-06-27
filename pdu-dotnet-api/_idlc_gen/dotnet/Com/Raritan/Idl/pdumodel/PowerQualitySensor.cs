// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from PowerQualitySensor.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.pdumodel {
  public class PowerQualitySensor : Com.Raritan.Idl.sensors.StateSensor_3_0_1 {

    static public readonly new TypeInfo typeInfo = new TypeInfo("pdumodel.PowerQualitySensor:1.0.0", null);

    public PowerQualitySensor(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public PowerQualitySensor(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new PowerQualitySensor StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new PowerQualitySensor(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public const int STATE_NORMAL = 0;

    public const int STATE_WARNING = 1;

    public const int STATE_CRITICAL = 2;

  }
}