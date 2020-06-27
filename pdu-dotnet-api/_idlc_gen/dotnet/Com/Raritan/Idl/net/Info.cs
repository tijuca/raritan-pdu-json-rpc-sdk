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

  public class Info : ICloneable {
    public object Clone() {
      Info copy = new Info();
      copy.common = this.common;
      copy.ifMap = this.ifMap;
      copy.ethMap = this.ethMap;
      copy.wlanMap = this.wlanMap;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["common"] = this.common.Encode();
      json["ifMap"] = new JsonArray(this.ifMap.Select(_entry => (JsonValue) new JsonObject(new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, LightJson.JsonValue>> {
        new System.Collections.Generic.KeyValuePair<string, JsonValue>("key", _entry.Key),
        new System.Collections.Generic.KeyValuePair<string, JsonValue>("value", _entry.Value.Encode())
      })));
      json["ethMap"] = new JsonArray(this.ethMap.Select(_entry => (JsonValue) new JsonObject(new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, LightJson.JsonValue>> {
        new System.Collections.Generic.KeyValuePair<string, JsonValue>("key", _entry.Key),
        new System.Collections.Generic.KeyValuePair<string, JsonValue>("value", _entry.Value.Encode())
      })));
      json["wlanMap"] = new JsonArray(this.wlanMap.Select(_entry => (JsonValue) new JsonObject(new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, LightJson.JsonValue>> {
        new System.Collections.Generic.KeyValuePair<string, JsonValue>("key", _entry.Key),
        new System.Collections.Generic.KeyValuePair<string, JsonValue>("value", _entry.Value.Encode())
      })));
      return json;
    }

    public static Info Decode(LightJson.JsonObject json, Agent agent) {
      Info inst = new Info();
      inst.common = Com.Raritan.Idl.net.CommonInfo.Decode(json["common"], agent);
      inst.ifMap = DictionaryHelper.Create(json["ifMap"].AsJsonArray.Select(
        _value => new System.Collections.Generic.KeyValuePair<string, Com.Raritan.Idl.net.InterfaceInfo>(_value["key"], Com.Raritan.Idl.net.InterfaceInfo.Decode(_value["value"], agent))));
      inst.ethMap = DictionaryHelper.Create(json["ethMap"].AsJsonArray.Select(
        _value => new System.Collections.Generic.KeyValuePair<string, Com.Raritan.Idl.net.EthInfo>(_value["key"], Com.Raritan.Idl.net.EthInfo.Decode(_value["value"], agent))));
      inst.wlanMap = DictionaryHelper.Create(json["wlanMap"].AsJsonArray.Select(
        _value => new System.Collections.Generic.KeyValuePair<string, Com.Raritan.Idl.net.WlanInfo>(_value["key"], Com.Raritan.Idl.net.WlanInfo.Decode(_value["value"], agent))));
      return inst;
    }

    public Com.Raritan.Idl.net.CommonInfo common = new Com.Raritan.Idl.net.CommonInfo();
    public System.Collections.Generic.IDictionary<string, Com.Raritan.Idl.net.InterfaceInfo> ifMap = new System.Collections.Generic.Dictionary<string, Com.Raritan.Idl.net.InterfaceInfo>();
    public System.Collections.Generic.IDictionary<string, Com.Raritan.Idl.net.EthInfo> ethMap = new System.Collections.Generic.Dictionary<string, Com.Raritan.Idl.net.EthInfo>();
    public System.Collections.Generic.IDictionary<string, Com.Raritan.Idl.net.WlanInfo> wlanMap = new System.Collections.Generic.Dictionary<string, Com.Raritan.Idl.net.WlanInfo>();
  }
}