// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Net.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.net {

  public class LinkStateChangedEvent : Com.Raritan.Idl.idl.Event {
    static public readonly new TypeInfo typeInfo = new TypeInfo("net.LinkStateChangedEvent:1.0.0", Com.Raritan.Idl.idl.Event.typeInfo);

    public string ifName = "";
    public string ifLabel = "";
    public Com.Raritan.Idl.net.InterfaceType ifType = Com.Raritan.Idl.net.InterfaceType.ETHERNET;
    public Com.Raritan.Idl.net.InterfaceOpState ifState = Com.Raritan.Idl.net.InterfaceOpState.NOT_PRESENT;
  }
}