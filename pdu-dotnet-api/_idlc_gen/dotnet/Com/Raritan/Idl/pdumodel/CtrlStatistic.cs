// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Controller.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.pdumodel {

  public class CtrlStatistic : ICloneable {
    public object Clone() {
      CtrlStatistic copy = new CtrlStatistic();
      copy.masterCSumErrCnt = this.masterCSumErrCnt;
      copy.slaveCSumErrCnt = this.slaveCSumErrCnt;
      copy.timeoutCnt = this.timeoutCnt;
      copy.resetCnt = this.resetCnt;
      copy.emResetCnt = this.emResetCnt;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["masterCSumErrCnt"] = this.masterCSumErrCnt;
      json["slaveCSumErrCnt"] = this.slaveCSumErrCnt;
      json["timeoutCnt"] = this.timeoutCnt;
      json["resetCnt"] = this.resetCnt;
      json["emResetCnt"] = this.emResetCnt;
      return json;
    }

    public static CtrlStatistic Decode(LightJson.JsonObject json, Agent agent) {
      CtrlStatistic inst = new CtrlStatistic();
      inst.masterCSumErrCnt = (int)json["masterCSumErrCnt"];
      inst.slaveCSumErrCnt = (int)json["slaveCSumErrCnt"];
      inst.timeoutCnt = (int)json["timeoutCnt"];
      inst.resetCnt = (int)json["resetCnt"];
      inst.emResetCnt = (int)json["emResetCnt"];
      return inst;
    }

    public int masterCSumErrCnt = 0;
    public int slaveCSumErrCnt = 0;
    public int timeoutCnt = 0;
    public int resetCnt = 0;
    public int emResetCnt = 0;
  }
}