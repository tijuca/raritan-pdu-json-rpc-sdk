// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from ResMon.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.res_mon {
  public class ResMon : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("res_mon.ResMon:1.0.0", null);

    public ResMon(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public ResMon(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new ResMon StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new ResMon(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public class GetDataEntriesResult {
      public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.res_mon.Entry> entries;
    }

    public GetDataEntriesResult getDataEntries() {
      JsonObject _parameters = null;
      var _result = RpcCall("getDataEntries", _parameters);
      var _ret = new GetDataEntriesResult();
      _ret.entries = new System.Collections.Generic.List<Com.Raritan.Idl.res_mon.Entry>(_result["entries"].AsJsonArray.Select(
        _value => Com.Raritan.Idl.res_mon.Entry.Decode(_value, agent)));
      return _ret;
    }

    public AsyncRequest getDataEntries(AsyncRpcResponse<GetDataEntriesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getDataEntries(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getDataEntries(AsyncRpcResponse<GetDataEntriesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getDataEntries", _parameters,
        _result => {
          try {
            var _ret = new GetDataEntriesResult();
            _ret.entries = new System.Collections.Generic.List<Com.Raritan.Idl.res_mon.Entry>(_result["entries"].AsJsonArray.Select(
              _value => Com.Raritan.Idl.res_mon.Entry.Decode(_value, agent)));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
