// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from SessionManager.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.session {

  public class Session : ICloneable {
    public object Clone() {
      Session copy = new Session();
      copy.token = this.token;
      copy.username = this.username;
      copy.remoteIp = this.remoteIp;
      copy.clientType = this.clientType;
      copy.creationTime = this.creationTime;
      copy.timeout = this.timeout;
      copy.idle = this.idle;
      copy.userIdle = this.userIdle;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["token"] = this.token;
      json["username"] = this.username;
      json["remoteIp"] = this.remoteIp;
      json["clientType"] = this.clientType;
      json["creationTime"] = this.creationTime.Ticks;
      json["timeout"] = this.timeout;
      json["idle"] = this.idle;
      json["userIdle"] = this.userIdle;
      return json;
    }

    public static Session Decode(LightJson.JsonObject json, Agent agent) {
      Session inst = new Session();
      inst.token = (string)json["token"];
      inst.username = (string)json["username"];
      inst.remoteIp = (string)json["remoteIp"];
      inst.clientType = (string)json["clientType"];
      inst.creationTime = new System.DateTime(json["creationTime"]);
      inst.timeout = (int)json["timeout"];
      inst.idle = (int)json["idle"];
      inst.userIdle = (int)json["userIdle"];
      return inst;
    }

    public string token = "";
    public string username = "";
    public string remoteIp = "";
    public string clientType = "";
    public System.DateTime creationTime = new System.DateTime(0);
    public int timeout = 0;
    public int idle = 0;
    public int userIdle = 0;
  }
}
