// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from testrpc.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.test {
  public class Control : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("test.Control:1.0.0", null);

    public Control(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public Control(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new Control StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new Control(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public class IsTestModeResult {
      public bool _ret_;
    }

    public IsTestModeResult isTestMode() {
      JsonObject _parameters = null;
      var _result = RpcCall("isTestMode", _parameters);
      var _ret = new IsTestModeResult();
      _ret._ret_ = (bool)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest isTestMode(AsyncRpcResponse<IsTestModeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return isTestMode(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest isTestMode(AsyncRpcResponse<IsTestModeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("isTestMode", _parameters,
        _result => {
          try {
            var _ret = new IsTestModeResult();
            _ret._ret_ = (bool)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SetTestModeResult {
    }

    public SetTestModeResult setTestMode(bool isTestModeOn) {
      var _parameters = new LightJson.JsonObject();
      _parameters["isTestModeOn"] = isTestModeOn;

      var _result = RpcCall("setTestMode", _parameters);
      var _ret = new SetTestModeResult();
      return _ret;
    }

    public AsyncRequest setTestMode(bool isTestModeOn, AsyncRpcResponse<SetTestModeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setTestMode(isTestModeOn, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setTestMode(bool isTestModeOn, AsyncRpcResponse<SetTestModeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["isTestModeOn"] = isTestModeOn;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("setTestMode", _parameters,
        _result => {
          try {
            var _ret = new SetTestModeResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}