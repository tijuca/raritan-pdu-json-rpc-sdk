// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Cfg.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.cfg {

  public class KeyValue : ICloneable {
    public object Clone() {
      KeyValue copy = new KeyValue();
      copy.key = this.key;
      copy.value = this.value;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["key"] = this.key;
      json["value"] = this.value;
      return json;
    }

    public static KeyValue Decode(LightJson.JsonObject json, Agent agent) {
      KeyValue inst = new KeyValue();
      inst.key = (string)json["key"];
      inst.value = (string)json["value"];
      return inst;
    }

    public string key = "";
    public string value = "";
  }
}
