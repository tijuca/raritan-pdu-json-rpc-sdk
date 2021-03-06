// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from EventService.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl._event {
  public class Channel_1_0_1 : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("event.Channel:1.0.1", null);

    public Channel_1_0_1(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public Channel_1_0_1(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new Channel_1_0_1 StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new Channel_1_0_1(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public class DemandEventTypeResult {
    }

    public DemandEventTypeResult demandEventType(TypeInfo type) {
      var _parameters = new LightJson.JsonObject();
      _parameters["type"] = type.ToString();

      var _result = RpcCall("demandEventType", _parameters);
      var _ret = new DemandEventTypeResult();
      return _ret;
    }

    public AsyncRequest demandEventType(TypeInfo type, AsyncRpcResponse<DemandEventTypeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return demandEventType(type, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest demandEventType(TypeInfo type, AsyncRpcResponse<DemandEventTypeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["type"] = type.ToString();
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("demandEventType", _parameters,
        _result => {
          try {
            var _ret = new DemandEventTypeResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class CancelEventTypeResult {
    }

    public CancelEventTypeResult cancelEventType(TypeInfo type) {
      var _parameters = new LightJson.JsonObject();
      _parameters["type"] = type.ToString();

      var _result = RpcCall("cancelEventType", _parameters);
      var _ret = new CancelEventTypeResult();
      return _ret;
    }

    public AsyncRequest cancelEventType(TypeInfo type, AsyncRpcResponse<CancelEventTypeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return cancelEventType(type, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest cancelEventType(TypeInfo type, AsyncRpcResponse<CancelEventTypeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["type"] = type.ToString();
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("cancelEventType", _parameters,
        _result => {
          try {
            var _ret = new CancelEventTypeResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class DemandEventTypesResult {
    }

    public DemandEventTypesResult demandEventTypes(System.Collections.Generic.IEnumerable<TypeInfo> types) {
      var _parameters = new LightJson.JsonObject();
      _parameters["types"] = new JsonArray(types.Select(
        _value => (JsonValue)(_value.ToString())));

      var _result = RpcCall("demandEventTypes", _parameters);
      var _ret = new DemandEventTypesResult();
      return _ret;
    }

    public AsyncRequest demandEventTypes(System.Collections.Generic.IEnumerable<TypeInfo> types, AsyncRpcResponse<DemandEventTypesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return demandEventTypes(types, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest demandEventTypes(System.Collections.Generic.IEnumerable<TypeInfo> types, AsyncRpcResponse<DemandEventTypesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["types"] = new JsonArray(types.Select(
          _value => (JsonValue)(_value.ToString())));
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("demandEventTypes", _parameters,
        _result => {
          try {
            var _ret = new DemandEventTypesResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class CancelEventTypesResult {
    }

    public CancelEventTypesResult cancelEventTypes(System.Collections.Generic.IEnumerable<TypeInfo> types) {
      var _parameters = new LightJson.JsonObject();
      _parameters["types"] = new JsonArray(types.Select(
        _value => (JsonValue)(_value.ToString())));

      var _result = RpcCall("cancelEventTypes", _parameters);
      var _ret = new CancelEventTypesResult();
      return _ret;
    }

    public AsyncRequest cancelEventTypes(System.Collections.Generic.IEnumerable<TypeInfo> types, AsyncRpcResponse<CancelEventTypesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return cancelEventTypes(types, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest cancelEventTypes(System.Collections.Generic.IEnumerable<TypeInfo> types, AsyncRpcResponse<CancelEventTypesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["types"] = new JsonArray(types.Select(
          _value => (JsonValue)(_value.ToString())));
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("cancelEventTypes", _parameters,
        _result => {
          try {
            var _ret = new CancelEventTypesResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class DemandEventResult {
    }

    public DemandEventResult demandEvent(TypeInfo type, ObjectProxy src) {
      var _parameters = new LightJson.JsonObject();
      _parameters["type"] = type.ToString();
      _parameters["src"] = src != null ? src.Encode() : JsonValue.Null;

      var _result = RpcCall("demandEvent", _parameters);
      var _ret = new DemandEventResult();
      return _ret;
    }

    public AsyncRequest demandEvent(TypeInfo type, ObjectProxy src, AsyncRpcResponse<DemandEventResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return demandEvent(type, src, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest demandEvent(TypeInfo type, ObjectProxy src, AsyncRpcResponse<DemandEventResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["type"] = type.ToString();
        _parameters["src"] = src != null ? src.Encode() : JsonValue.Null;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("demandEvent", _parameters,
        _result => {
          try {
            var _ret = new DemandEventResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class CancelEventResult {
    }

    public CancelEventResult cancelEvent(TypeInfo type, ObjectProxy src) {
      var _parameters = new LightJson.JsonObject();
      _parameters["type"] = type.ToString();
      _parameters["src"] = src != null ? src.Encode() : JsonValue.Null;

      var _result = RpcCall("cancelEvent", _parameters);
      var _ret = new CancelEventResult();
      return _ret;
    }

    public AsyncRequest cancelEvent(TypeInfo type, ObjectProxy src, AsyncRpcResponse<CancelEventResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return cancelEvent(type, src, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest cancelEvent(TypeInfo type, ObjectProxy src, AsyncRpcResponse<CancelEventResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["type"] = type.ToString();
        _parameters["src"] = src != null ? src.Encode() : JsonValue.Null;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("cancelEvent", _parameters,
        _result => {
          try {
            var _ret = new CancelEventResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class EventSelect : ICloneable {
      public object Clone() {
        EventSelect copy = new EventSelect();
        copy.type = this.type;
        copy.src = this.src;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["type"] = this.type.ToString();
        json["src"] = this.src != null ? this.src.Encode() : JsonValue.Null;
        return json;
      }

      public static EventSelect Decode(LightJson.JsonObject json, Agent agent) {
        EventSelect inst = new EventSelect();
        inst.type = new TypeInfo(json["type"], null);
        inst.src = ObjectProxy.Decode(json["src"], agent);
        return inst;
      }

      public TypeInfo type = null;
      public ObjectProxy src = null;
    }

    public class DemandEventsResult {
    }

    public DemandEventsResult demandEvents(System.Collections.Generic.IEnumerable<Com.Raritan.Idl._event.Channel_1_0_1.EventSelect> events) {
      var _parameters = new LightJson.JsonObject();
      _parameters["events"] = new JsonArray(events.Select(
        _value => (JsonValue)(_value.Encode())));

      var _result = RpcCall("demandEvents", _parameters);
      var _ret = new DemandEventsResult();
      return _ret;
    }

    public AsyncRequest demandEvents(System.Collections.Generic.IEnumerable<Com.Raritan.Idl._event.Channel_1_0_1.EventSelect> events, AsyncRpcResponse<DemandEventsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return demandEvents(events, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest demandEvents(System.Collections.Generic.IEnumerable<Com.Raritan.Idl._event.Channel_1_0_1.EventSelect> events, AsyncRpcResponse<DemandEventsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["events"] = new JsonArray(events.Select(
          _value => (JsonValue)(_value.Encode())));
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("demandEvents", _parameters,
        _result => {
          try {
            var _ret = new DemandEventsResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class CancelEventsResult {
    }

    public CancelEventsResult cancelEvents(System.Collections.Generic.IEnumerable<Com.Raritan.Idl._event.Channel_1_0_1.EventSelect> events) {
      var _parameters = new LightJson.JsonObject();
      _parameters["events"] = new JsonArray(events.Select(
        _value => (JsonValue)(_value.Encode())));

      var _result = RpcCall("cancelEvents", _parameters);
      var _ret = new CancelEventsResult();
      return _ret;
    }

    public AsyncRequest cancelEvents(System.Collections.Generic.IEnumerable<Com.Raritan.Idl._event.Channel_1_0_1.EventSelect> events, AsyncRpcResponse<CancelEventsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return cancelEvents(events, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest cancelEvents(System.Collections.Generic.IEnumerable<Com.Raritan.Idl._event.Channel_1_0_1.EventSelect> events, AsyncRpcResponse<CancelEventsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["events"] = new JsonArray(events.Select(
          _value => (JsonValue)(_value.Encode())));
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("cancelEvents", _parameters,
        _result => {
          try {
            var _ret = new CancelEventsResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SubscribeResult {
    }

    public SubscribeResult subscribe(Com.Raritan.Idl._event.Consumer consumer) {
      var _parameters = new LightJson.JsonObject();
      _parameters["consumer"] = consumer != null ? consumer.Encode() : JsonValue.Null;

      var _result = RpcCall("subscribe", _parameters);
      var _ret = new SubscribeResult();
      return _ret;
    }

    public AsyncRequest subscribe(Com.Raritan.Idl._event.Consumer consumer, AsyncRpcResponse<SubscribeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return subscribe(consumer, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest subscribe(Com.Raritan.Idl._event.Consumer consumer, AsyncRpcResponse<SubscribeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["consumer"] = consumer != null ? consumer.Encode() : JsonValue.Null;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("subscribe", _parameters,
        _result => {
          try {
            var _ret = new SubscribeResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class UnsubscribeResult {
      public int _ret_;
    }

    public UnsubscribeResult unsubscribe(Com.Raritan.Idl._event.Consumer consumer) {
      var _parameters = new LightJson.JsonObject();
      _parameters["consumer"] = consumer != null ? consumer.Encode() : JsonValue.Null;

      var _result = RpcCall("unsubscribe", _parameters);
      var _ret = new UnsubscribeResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest unsubscribe(Com.Raritan.Idl._event.Consumer consumer, AsyncRpcResponse<UnsubscribeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return unsubscribe(consumer, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest unsubscribe(Com.Raritan.Idl._event.Consumer consumer, AsyncRpcResponse<UnsubscribeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["consumer"] = consumer != null ? consumer.Encode() : JsonValue.Null;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("unsubscribe", _parameters,
        _result => {
          try {
            var _ret = new UnsubscribeResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class PollEventsResult {
      public bool _ret_;
      public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.idl.Event> events;
    }

    public PollEventsResult pollEvents() {
      JsonObject _parameters = null;
      var _result = RpcCall("pollEvents", _parameters);
      var _ret = new PollEventsResult();
      _ret._ret_ = (bool)_result["_ret_"];
      _ret.events = new System.Collections.Generic.List<Com.Raritan.Idl.idl.Event>(_result["events"].AsJsonArray.Select(
        _value => ((Com.Raritan.Idl.idl.Event)ValueObjectCodec.DecodeAs(_value, agent, Com.Raritan.Idl.idl.Event.typeInfo))));
      return _ret;
    }

    public AsyncRequest pollEvents(AsyncRpcResponse<PollEventsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return pollEvents(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest pollEvents(AsyncRpcResponse<PollEventsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("pollEvents", _parameters,
        _result => {
          try {
            var _ret = new PollEventsResult();
            _ret._ret_ = (bool)_result["_ret_"];
            _ret.events = new System.Collections.Generic.List<Com.Raritan.Idl.idl.Event>(_result["events"].AsJsonArray.Select(
              _value => ((Com.Raritan.Idl.idl.Event)ValueObjectCodec.DecodeAs(_value, agent, Com.Raritan.Idl.idl.Event.typeInfo))));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class PollEventsNbResult {
      public bool _ret_;
      public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.idl.Event> events;
    }

    public PollEventsNbResult pollEventsNb() {
      JsonObject _parameters = null;
      var _result = RpcCall("pollEventsNb", _parameters);
      var _ret = new PollEventsNbResult();
      _ret._ret_ = (bool)_result["_ret_"];
      _ret.events = new System.Collections.Generic.List<Com.Raritan.Idl.idl.Event>(_result["events"].AsJsonArray.Select(
        _value => ((Com.Raritan.Idl.idl.Event)ValueObjectCodec.DecodeAs(_value, agent, Com.Raritan.Idl.idl.Event.typeInfo))));
      return _ret;
    }

    public AsyncRequest pollEventsNb(AsyncRpcResponse<PollEventsNbResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return pollEventsNb(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest pollEventsNb(AsyncRpcResponse<PollEventsNbResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("pollEventsNb", _parameters,
        _result => {
          try {
            var _ret = new PollEventsNbResult();
            _ret._ret_ = (bool)_result["_ret_"];
            _ret.events = new System.Collections.Generic.List<Com.Raritan.Idl.idl.Event>(_result["events"].AsJsonArray.Select(
              _value => ((Com.Raritan.Idl.idl.Event)ValueObjectCodec.DecodeAs(_value, agent, Com.Raritan.Idl.idl.Event.typeInfo))));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
