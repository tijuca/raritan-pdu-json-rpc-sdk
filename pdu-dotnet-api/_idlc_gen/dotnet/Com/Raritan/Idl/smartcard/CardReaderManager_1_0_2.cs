// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from CardReaderManager.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.smartcard {
  public class CardReaderManager_1_0_2 : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("smartcard.CardReaderManager:1.0.2", null);

    public CardReaderManager_1_0_2(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public CardReaderManager_1_0_2(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new CardReaderManager_1_0_2 StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new CardReaderManager_1_0_2(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public class CardReaderEvent : Com.Raritan.Idl.idl.Event {
      static public readonly new TypeInfo typeInfo = new TypeInfo("smartcard.CardReaderManager_1_0_2.CardReaderEvent:1.0.0", Com.Raritan.Idl.idl.Event.typeInfo);

      public Com.Raritan.Idl.smartcard.CardReader_1_0_2 cardReader = null;
    }

    public class CardReaderAttachedEvent : Com.Raritan.Idl.smartcard.CardReaderManager_1_0_2.CardReaderEvent {
      static public readonly new TypeInfo typeInfo = new TypeInfo("smartcard.CardReaderManager_1_0_2.CardReaderAttachedEvent:1.0.0", Com.Raritan.Idl.smartcard.CardReaderManager_1_0_2.CardReaderEvent.typeInfo);

    }

    public class CardReaderDetachedEvent : Com.Raritan.Idl.smartcard.CardReaderManager_1_0_2.CardReaderEvent {
      static public readonly new TypeInfo typeInfo = new TypeInfo("smartcard.CardReaderManager_1_0_2.CardReaderDetachedEvent:1.0.0", Com.Raritan.Idl.smartcard.CardReaderManager_1_0_2.CardReaderEvent.typeInfo);

    }

    public class GetCardReadersResult {
      public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.smartcard.CardReader_1_0_2> _ret_;
    }

    public GetCardReadersResult getCardReaders() {
      JsonObject _parameters = null;
      var _result = RpcCall("getCardReaders", _parameters);
      var _ret = new GetCardReadersResult();
      _ret._ret_ = new System.Collections.Generic.List<Com.Raritan.Idl.smartcard.CardReader_1_0_2>(_result["_ret_"].AsJsonArray.Select(
        _value => Com.Raritan.Idl.smartcard.CardReader_1_0_2.StaticCast(ObjectProxy.Decode(_value, agent))));
      return _ret;
    }

    public AsyncRequest getCardReaders(AsyncRpcResponse<GetCardReadersResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getCardReaders(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getCardReaders(AsyncRpcResponse<GetCardReadersResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getCardReaders", _parameters,
        _result => {
          try {
            var _ret = new GetCardReadersResult();
            _ret._ret_ = new System.Collections.Generic.List<Com.Raritan.Idl.smartcard.CardReader_1_0_2>(_result["_ret_"].AsJsonArray.Select(
              _value => Com.Raritan.Idl.smartcard.CardReader_1_0_2.StaticCast(ObjectProxy.Decode(_value, agent))));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetCardReaderByIdResult {
      public Com.Raritan.Idl.smartcard.CardReader_1_0_2 _ret_;
    }

    public GetCardReaderByIdResult getCardReaderById(string readerId) {
      var _parameters = new LightJson.JsonObject();
      _parameters["readerId"] = readerId;

      var _result = RpcCall("getCardReaderById", _parameters);
      var _ret = new GetCardReaderByIdResult();
      _ret._ret_ = Com.Raritan.Idl.smartcard.CardReader_1_0_2.StaticCast(ObjectProxy.Decode(_result["_ret_"], agent));
      return _ret;
    }

    public AsyncRequest getCardReaderById(string readerId, AsyncRpcResponse<GetCardReaderByIdResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getCardReaderById(readerId, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getCardReaderById(string readerId, AsyncRpcResponse<GetCardReaderByIdResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["readerId"] = readerId;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("getCardReaderById", _parameters,
        _result => {
          try {
            var _ret = new GetCardReaderByIdResult();
            _ret._ret_ = Com.Raritan.Idl.smartcard.CardReader_1_0_2.StaticCast(ObjectProxy.Decode(_result["_ret_"], agent));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
