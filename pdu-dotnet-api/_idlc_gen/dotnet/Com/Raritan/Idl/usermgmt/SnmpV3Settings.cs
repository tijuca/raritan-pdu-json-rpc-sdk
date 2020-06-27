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

  public class SnmpV3Settings : ICloneable {
    public object Clone() {
      SnmpV3Settings copy = new SnmpV3Settings();
      copy.enabled = this.enabled;
      copy.secLevel = this.secLevel;
      copy.authProtocol = this.authProtocol;
      copy.usePasswordAsAuthPassphrase = this.usePasswordAsAuthPassphrase;
      copy.haveAuthPassphrase = this.haveAuthPassphrase;
      copy.authPassphrase = this.authPassphrase;
      copy.privProtocol = this.privProtocol;
      copy.useAuthPassphraseAsPrivPassphrase = this.useAuthPassphraseAsPrivPassphrase;
      copy.havePrivPassphrase = this.havePrivPassphrase;
      copy.privPassphrase = this.privPassphrase;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["enabled"] = this.enabled;
      json["secLevel"] = (int)this.secLevel;
      json["authProtocol"] = (int)this.authProtocol;
      json["usePasswordAsAuthPassphrase"] = this.usePasswordAsAuthPassphrase;
      json["haveAuthPassphrase"] = this.haveAuthPassphrase;
      json["authPassphrase"] = this.authPassphrase;
      json["privProtocol"] = (int)this.privProtocol;
      json["useAuthPassphraseAsPrivPassphrase"] = this.useAuthPassphraseAsPrivPassphrase;
      json["havePrivPassphrase"] = this.havePrivPassphrase;
      json["privPassphrase"] = this.privPassphrase;
      return json;
    }

    public static SnmpV3Settings Decode(LightJson.JsonObject json, Agent agent) {
      SnmpV3Settings inst = new SnmpV3Settings();
      inst.enabled = (bool)json["enabled"];
      inst.secLevel = (Com.Raritan.Idl.um.SnmpV3.SecurityLevel)(int)json["secLevel"];
      inst.authProtocol = (Com.Raritan.Idl.um.SnmpV3.AuthProtocol)(int)json["authProtocol"];
      inst.usePasswordAsAuthPassphrase = (bool)json["usePasswordAsAuthPassphrase"];
      inst.haveAuthPassphrase = (bool)json["haveAuthPassphrase"];
      inst.authPassphrase = (string)json["authPassphrase"];
      inst.privProtocol = (Com.Raritan.Idl.um.SnmpV3.PrivProtocol)(int)json["privProtocol"];
      inst.useAuthPassphraseAsPrivPassphrase = (bool)json["useAuthPassphraseAsPrivPassphrase"];
      inst.havePrivPassphrase = (bool)json["havePrivPassphrase"];
      inst.privPassphrase = (string)json["privPassphrase"];
      return inst;
    }

    public bool enabled = false;
    public Com.Raritan.Idl.um.SnmpV3.SecurityLevel secLevel = Com.Raritan.Idl.um.SnmpV3.SecurityLevel.NO_AUTH_NO_PRIV;
    public Com.Raritan.Idl.um.SnmpV3.AuthProtocol authProtocol = Com.Raritan.Idl.um.SnmpV3.AuthProtocol.MD5;
    public bool usePasswordAsAuthPassphrase = false;
    public bool haveAuthPassphrase = false;
    public string authPassphrase = "";
    public Com.Raritan.Idl.um.SnmpV3.PrivProtocol privProtocol = Com.Raritan.Idl.um.SnmpV3.PrivProtocol.DES;
    public bool useAuthPassphraseAsPrivPassphrase = false;
    public bool havePrivPassphrase = false;
    public string privPassphrase = "";
  }
}