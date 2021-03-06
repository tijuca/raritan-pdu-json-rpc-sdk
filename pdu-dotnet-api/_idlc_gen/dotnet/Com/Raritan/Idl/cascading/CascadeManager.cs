// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from CascadeManager.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.cascading {
  public class CascadeManager : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("cascading.CascadeManager:1.0.0", null);

    public CascadeManager(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public CascadeManager(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new CascadeManager StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new CascadeManager(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public const int NO_ERROR = 0;

    public const int ERR_INVALID_PARAM = 1;

    public const int ERR_UNSUPPORTED_ON_MASTER = 2;

    public const int ERR_UNSUPPORTED_ON_LINK_UNIT = 3;

    public const int ERR_LINK_ID_IN_USE = 4;

    public const int ERR_HOST_IN_USE = 5;

    public const int ERR_LINK_UNIT_UNREACHABLE = 6;

    public const int ERR_LINK_UNIT_ACCESS_DENIED = 7;

    public const int ERR_LINK_UNIT_REFUSED = 8;

    public const int ERR_UNIT_BUSY = 9;

    public const int ERR_NOT_SUPPORTED = 10;

    public const int ERR_PASSWORD_CHANGE_REQUIRED = 11;

    public const int ERR_PASSWORD_POLICY = 12;

    public enum Role {
      STANDALONE,
      MASTER,
      LINK_UNIT,
    }

    public enum LinkUnitStatus {
      UNKNOWN,
      OK,
      UNREACHABLE,
      ACCESS_DENIED,
      FIRMWARE_UPDATE,
    }

    public class LinkUnit : ICloneable {
      public object Clone() {
        LinkUnit copy = new LinkUnit();
        copy.host = this.host;
        copy.status = this.status;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["host"] = this.host;
        json["status"] = (int)this.status;
        return json;
      }

      public static LinkUnit Decode(LightJson.JsonObject json, Agent agent) {
        LinkUnit inst = new LinkUnit();
        inst.host = (string)json["host"];
        inst.status = (Com.Raritan.Idl.cascading.CascadeManager.LinkUnitStatus)(int)json["status"];
        return inst;
      }

      public string host = "";
      public Com.Raritan.Idl.cascading.CascadeManager.LinkUnitStatus status = Com.Raritan.Idl.cascading.CascadeManager.LinkUnitStatus.UNKNOWN;
    }

    public class Status : ICloneable {
      public object Clone() {
        Status copy = new Status();
        copy.role = this.role;
        copy.master = this.master;
        copy.linkUnits = this.linkUnits;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["role"] = (int)this.role;
        json["master"] = this.master;
        json["linkUnits"] = new JsonArray(this.linkUnits.Select(_entry => (JsonValue) new JsonObject(new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, LightJson.JsonValue>> {
          new System.Collections.Generic.KeyValuePair<string, JsonValue>("key", _entry.Key),
          new System.Collections.Generic.KeyValuePair<string, JsonValue>("value", _entry.Value.Encode())
        })));
        return json;
      }

      public static Status Decode(LightJson.JsonObject json, Agent agent) {
        Status inst = new Status();
        inst.role = (Com.Raritan.Idl.cascading.CascadeManager.Role)(int)json["role"];
        inst.master = (string)json["master"];
        inst.linkUnits = DictionaryHelper.Create(json["linkUnits"].AsJsonArray.Select(
          _value => new System.Collections.Generic.KeyValuePair<int, Com.Raritan.Idl.cascading.CascadeManager.LinkUnit>(_value["key"], Com.Raritan.Idl.cascading.CascadeManager.LinkUnit.Decode(_value["value"], agent))));
        return inst;
      }

      public Com.Raritan.Idl.cascading.CascadeManager.Role role = Com.Raritan.Idl.cascading.CascadeManager.Role.STANDALONE;
      public string master = "";
      public System.Collections.Generic.IDictionary<int, Com.Raritan.Idl.cascading.CascadeManager.LinkUnit> linkUnits = new System.Collections.Generic.Dictionary<int, Com.Raritan.Idl.cascading.CascadeManager.LinkUnit>();
    }

    public class RoleChangedEvent : Com.Raritan.Idl.idl.Event {
      static public readonly new TypeInfo typeInfo = new TypeInfo("cascading.CascadeManager.RoleChangedEvent:1.0.0", Com.Raritan.Idl.idl.Event.typeInfo);

      public Com.Raritan.Idl.cascading.CascadeManager.Role oldRole = Com.Raritan.Idl.cascading.CascadeManager.Role.STANDALONE;
      public Com.Raritan.Idl.cascading.CascadeManager.Role newRole = Com.Raritan.Idl.cascading.CascadeManager.Role.STANDALONE;
      public string master = "";
    }

    public class LinkUnitAddedEvent : Com.Raritan.Idl._event.UserEvent {
      static public readonly new TypeInfo typeInfo = new TypeInfo("cascading.CascadeManager.LinkUnitAddedEvent:1.0.0", Com.Raritan.Idl._event.UserEvent.typeInfo);

      public int linkId = 0;
      public string host = "";
    }

    public class LinkUnitReleasedEvent : Com.Raritan.Idl._event.UserEvent {
      static public readonly new TypeInfo typeInfo = new TypeInfo("cascading.CascadeManager.LinkUnitReleasedEvent:1.0.0", Com.Raritan.Idl._event.UserEvent.typeInfo);

      public int linkId = 0;
      public string host = "";
    }

    public class LinkUnitStatusChangedEvent : Com.Raritan.Idl.idl.Event {
      static public readonly new TypeInfo typeInfo = new TypeInfo("cascading.CascadeManager.LinkUnitStatusChangedEvent:1.0.0", Com.Raritan.Idl.idl.Event.typeInfo);

      public int linkId = 0;
      public string host = "";
      public Com.Raritan.Idl.cascading.CascadeManager.LinkUnitStatus oldStatus = Com.Raritan.Idl.cascading.CascadeManager.LinkUnitStatus.UNKNOWN;
      public Com.Raritan.Idl.cascading.CascadeManager.LinkUnitStatus newStatus = Com.Raritan.Idl.cascading.CascadeManager.LinkUnitStatus.UNKNOWN;
    }

    public class GetStatusResult {
      public Com.Raritan.Idl.cascading.CascadeManager.Status _ret_;
    }

    public GetStatusResult getStatus() {
      JsonObject _parameters = null;
      var _result = RpcCall("getStatus", _parameters);
      var _ret = new GetStatusResult();
      _ret._ret_ = Com.Raritan.Idl.cascading.CascadeManager.Status.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest getStatus(AsyncRpcResponse<GetStatusResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getStatus(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getStatus(AsyncRpcResponse<GetStatusResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getStatus", _parameters,
        _result => {
          try {
            var _ret = new GetStatusResult();
            _ret._ret_ = Com.Raritan.Idl.cascading.CascadeManager.Status.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class AddLinkUnitResult {
      public int _ret_;
    }

    public AddLinkUnitResult addLinkUnit(int linkId, string host, string login, string password, string newPassword) {
      var _parameters = new LightJson.JsonObject();
      _parameters["linkId"] = linkId;
      _parameters["host"] = host;
      _parameters["login"] = login;
      _parameters["password"] = password;
      _parameters["newPassword"] = newPassword;

      var _result = RpcCall("addLinkUnit", _parameters);
      var _ret = new AddLinkUnitResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest addLinkUnit(int linkId, string host, string login, string password, string newPassword, AsyncRpcResponse<AddLinkUnitResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return addLinkUnit(linkId, host, login, password, newPassword, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest addLinkUnit(int linkId, string host, string login, string password, string newPassword, AsyncRpcResponse<AddLinkUnitResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["linkId"] = linkId;
        _parameters["host"] = host;
        _parameters["login"] = login;
        _parameters["password"] = password;
        _parameters["newPassword"] = newPassword;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("addLinkUnit", _parameters,
        _result => {
          try {
            var _ret = new AddLinkUnitResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class ReleaseLinkUnitResult {
      public int _ret_;
    }

    public ReleaseLinkUnitResult releaseLinkUnit(int linkId) {
      var _parameters = new LightJson.JsonObject();
      _parameters["linkId"] = linkId;

      var _result = RpcCall("releaseLinkUnit", _parameters);
      var _ret = new ReleaseLinkUnitResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest releaseLinkUnit(int linkId, AsyncRpcResponse<ReleaseLinkUnitResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return releaseLinkUnit(linkId, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest releaseLinkUnit(int linkId, AsyncRpcResponse<ReleaseLinkUnitResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["linkId"] = linkId;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("releaseLinkUnit", _parameters,
        _result => {
          try {
            var _ret = new ReleaseLinkUnitResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class RequestLinkResult {
      public int _ret_;
    }

    public RequestLinkResult requestLink(string token) {
      var _parameters = new LightJson.JsonObject();
      _parameters["token"] = token;

      var _result = RpcCall("requestLink", _parameters);
      var _ret = new RequestLinkResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest requestLink(string token, AsyncRpcResponse<RequestLinkResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return requestLink(token, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest requestLink(string token, AsyncRpcResponse<RequestLinkResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["token"] = token;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("requestLink", _parameters,
        _result => {
          try {
            var _ret = new RequestLinkResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class FinalizeLinkResult {
    }

    public FinalizeLinkResult finalizeLink(string token) {
      var _parameters = new LightJson.JsonObject();
      _parameters["token"] = token;

      var _result = RpcCall("finalizeLink", _parameters);
      var _ret = new FinalizeLinkResult();
      return _ret;
    }

    public AsyncRequest finalizeLink(string token, AsyncRpcResponse<FinalizeLinkResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return finalizeLink(token, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest finalizeLink(string token, AsyncRpcResponse<FinalizeLinkResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["token"] = token;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("finalizeLink", _parameters,
        _result => {
          try {
            var _ret = new FinalizeLinkResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class UnlinkResult {
    }

    public UnlinkResult unlink() {
      JsonObject _parameters = null;
      var _result = RpcCall("unlink", _parameters);
      var _ret = new UnlinkResult();
      return _ret;
    }

    public AsyncRequest unlink(AsyncRpcResponse<UnlinkResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return unlink(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest unlink(AsyncRpcResponse<UnlinkResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("unlink", _parameters,
        _result => {
          try {
            var _ret = new UnlinkResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
