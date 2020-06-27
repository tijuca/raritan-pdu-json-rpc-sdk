// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from ExternalBeeper.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.hmi {
  public class ExternalBeeper : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("hmi.ExternalBeeper:1.0.0", null);

    public ExternalBeeper(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public ExternalBeeper(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new ExternalBeeper StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new ExternalBeeper(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public class AlarmResult {
    }

    public AlarmResult alarm() {
      JsonObject _parameters = null;
      var _result = RpcCall("alarm", _parameters);
      var _ret = new AlarmResult();
      return _ret;
    }

    public AsyncRequest alarm(AsyncRpcResponse<AlarmResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return alarm(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest alarm(AsyncRpcResponse<AlarmResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("alarm", _parameters,
        _result => {
          try {
            var _ret = new AlarmResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class OnResult {
    }

    public OnResult on() {
      JsonObject _parameters = null;
      var _result = RpcCall("on", _parameters);
      var _ret = new OnResult();
      return _ret;
    }

    public AsyncRequest on(AsyncRpcResponse<OnResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return on(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest on(AsyncRpcResponse<OnResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("on", _parameters,
        _result => {
          try {
            var _ret = new OnResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class OffResult {
    }

    public OffResult off() {
      JsonObject _parameters = null;
      var _result = RpcCall("off", _parameters);
      var _ret = new OffResult();
      return _ret;
    }

    public AsyncRequest off(AsyncRpcResponse<OffResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return off(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest off(AsyncRpcResponse<OffResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("off", _parameters,
        _result => {
          try {
            var _ret = new OffResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}