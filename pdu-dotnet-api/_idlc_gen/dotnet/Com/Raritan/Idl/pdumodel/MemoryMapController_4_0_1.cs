// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from MemoryMapController.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.pdumodel {
  public class MemoryMapController_4_0_1 : Com.Raritan.Idl.pdumodel.Controller_4_0_1 {

    static public readonly new TypeInfo typeInfo = new TypeInfo("pdumodel.MemoryMapController:4.0.1", null);

    public MemoryMapController_4_0_1(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public MemoryMapController_4_0_1(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new MemoryMapController_4_0_1 StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new MemoryMapController_4_0_1(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public class ReadMemoryResult {
      public int _ret_;
      public System.Collections.Generic.IEnumerable<byte> memory;
    }

    public ReadMemoryResult readMemory(int address, int size) {
      var _parameters = new LightJson.JsonObject();
      _parameters["address"] = address;
      _parameters["size"] = size;

      var _result = RpcCall("readMemory", _parameters);
      var _ret = new ReadMemoryResult();
      _ret._ret_ = (int)_result["_ret_"];
      _ret.memory = new System.Collections.Generic.List<byte>(_result["memory"].AsJsonArray.Select(
        _value => (byte)_value));
      return _ret;
    }

    public AsyncRequest readMemory(int address, int size, AsyncRpcResponse<ReadMemoryResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return readMemory(address, size, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest readMemory(int address, int size, AsyncRpcResponse<ReadMemoryResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["address"] = address;
        _parameters["size"] = size;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("readMemory", _parameters,
        _result => {
          try {
            var _ret = new ReadMemoryResult();
            _ret._ret_ = (int)_result["_ret_"];
            _ret.memory = new System.Collections.Generic.List<byte>(_result["memory"].AsJsonArray.Select(
              _value => (byte)_value));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class WriteMemoryResult {
      public int _ret_;
    }

    public WriteMemoryResult writeMemory(int address, System.Collections.Generic.IEnumerable<byte> memory) {
      var _parameters = new LightJson.JsonObject();
      _parameters["address"] = address;
      _parameters["memory"] = new JsonArray(memory.Select(
        _value => (JsonValue)(_value)));

      var _result = RpcCall("writeMemory", _parameters);
      var _ret = new WriteMemoryResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest writeMemory(int address, System.Collections.Generic.IEnumerable<byte> memory, AsyncRpcResponse<WriteMemoryResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return writeMemory(address, memory, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest writeMemory(int address, System.Collections.Generic.IEnumerable<byte> memory, AsyncRpcResponse<WriteMemoryResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["address"] = address;
        _parameters["memory"] = new JsonArray(memory.Select(
          _value => (JsonValue)(_value)));
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("writeMemory", _parameters,
        _result => {
          try {
            var _ret = new WriteMemoryResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
