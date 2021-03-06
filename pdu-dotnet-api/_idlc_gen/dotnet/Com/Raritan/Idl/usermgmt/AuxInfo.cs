// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from User.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.usermgmt {

  public class AuxInfo : ICloneable {
    public object Clone() {
      AuxInfo copy = new AuxInfo();
      copy.fullname = this.fullname;
      copy.telephone = this.telephone;
      copy.eMail = this.eMail;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["fullname"] = this.fullname;
      json["telephone"] = this.telephone;
      json["eMail"] = this.eMail;
      return json;
    }

    public static AuxInfo Decode(LightJson.JsonObject json, Agent agent) {
      AuxInfo inst = new AuxInfo();
      inst.fullname = (string)json["fullname"];
      inst.telephone = (string)json["telephone"];
      inst.eMail = (string)json["eMail"];
      return inst;
    }

    public string fullname = "";
    public string telephone = "";
    public string eMail = "";
  }
}
