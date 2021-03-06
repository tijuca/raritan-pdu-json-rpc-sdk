// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Smtp.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.devsettings {
  public class Smtp_2_0_0 : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("devsettings.Smtp:2.0.0", null);

    public Smtp_2_0_0(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public Smtp_2_0_0(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new Smtp_2_0_0 StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new Smtp_2_0_0(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public const int ERR_INVALID_PARAMS = 1;

    public class Configuration : ICloneable {
      public object Clone() {
        Configuration copy = new Configuration();
        copy.host = this.host;
        copy.port = this.port;
        copy.useTls = this.useTls;
        copy.allowOffTimeRangeCerts = this.allowOffTimeRangeCerts;
        copy.caCertChain = this.caCertChain;
        copy.sender = this.sender;
        copy.useAuth = this.useAuth;
        copy.username = this.username;
        copy.password = this.password;
        copy.retryCount = this.retryCount;
        copy.retryInterval = this.retryInterval;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["host"] = this.host;
        json["port"] = this.port;
        json["useTls"] = this.useTls;
        json["allowOffTimeRangeCerts"] = this.allowOffTimeRangeCerts;
        json["caCertChain"] = this.caCertChain;
        json["sender"] = this.sender;
        json["useAuth"] = this.useAuth;
        json["username"] = this.username;
        json["password"] = this.password;
        json["retryCount"] = this.retryCount;
        json["retryInterval"] = this.retryInterval;
        return json;
      }

      public static Configuration Decode(LightJson.JsonObject json, Agent agent) {
        Configuration inst = new Configuration();
        inst.host = (string)json["host"];
        inst.port = (int)json["port"];
        inst.useTls = (bool)json["useTls"];
        inst.allowOffTimeRangeCerts = (bool)json["allowOffTimeRangeCerts"];
        inst.caCertChain = (string)json["caCertChain"];
        inst.sender = (string)json["sender"];
        inst.useAuth = (bool)json["useAuth"];
        inst.username = (string)json["username"];
        inst.password = (string)json["password"];
        inst.retryCount = (int)json["retryCount"];
        inst.retryInterval = (int)json["retryInterval"];
        return inst;
      }

      public string host = "";
      public int port = 0;
      public bool useTls = false;
      public bool allowOffTimeRangeCerts = false;
      public string caCertChain = "";
      public string sender = "";
      public bool useAuth = false;
      public string username = "";
      public string password = "";
      public int retryCount = 0;
      public int retryInterval = 0;
    }

    public class GetConfigurationResult {
      public Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration _ret_;
    }

    public GetConfigurationResult getConfiguration() {
      JsonObject _parameters = null;
      var _result = RpcCall("getConfiguration", _parameters);
      var _ret = new GetConfigurationResult();
      _ret._ret_ = Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest getConfiguration(AsyncRpcResponse<GetConfigurationResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getConfiguration(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getConfiguration(AsyncRpcResponse<GetConfigurationResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getConfiguration", _parameters,
        _result => {
          try {
            var _ret = new GetConfigurationResult();
            _ret._ret_ = Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SetConfigurationResult {
      public int _ret_;
    }

    public SetConfigurationResult setConfiguration(Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration cfg) {
      var _parameters = new LightJson.JsonObject();
      _parameters["cfg"] = cfg.Encode();

      var _result = RpcCall("setConfiguration", _parameters);
      var _ret = new SetConfigurationResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest setConfiguration(Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration cfg, AsyncRpcResponse<SetConfigurationResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setConfiguration(cfg, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setConfiguration(Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration cfg, AsyncRpcResponse<SetConfigurationResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["cfg"] = cfg.Encode();
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("setConfiguration", _parameters,
        _result => {
          try {
            var _ret = new SetConfigurationResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class TestResult : ICloneable {
      public object Clone() {
        TestResult copy = new TestResult();
        copy.status = this.status;
        copy.message = this.message;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["status"] = this.status;
        json["message"] = this.message;
        return json;
      }

      public static TestResult Decode(LightJson.JsonObject json, Agent agent) {
        TestResult inst = new TestResult();
        inst.status = (int)json["status"];
        inst.message = (string)json["message"];
        return inst;
      }

      public int status = 0;
      public string message = "";
    }

    public class TestConfigurationResult {
      public Com.Raritan.Idl.devsettings.Smtp_2_0_0.TestResult _ret_;
    }

    public TestConfigurationResult testConfiguration(Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration cfg, System.Collections.Generic.IEnumerable<string> recipients) {
      var _parameters = new LightJson.JsonObject();
      _parameters["cfg"] = cfg.Encode();
      _parameters["recipients"] = new JsonArray(recipients.Select(
        _value => (JsonValue)(_value)));

      var _result = RpcCall("testConfiguration", _parameters);
      var _ret = new TestConfigurationResult();
      _ret._ret_ = Com.Raritan.Idl.devsettings.Smtp_2_0_0.TestResult.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest testConfiguration(Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration cfg, System.Collections.Generic.IEnumerable<string> recipients, AsyncRpcResponse<TestConfigurationResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return testConfiguration(cfg, recipients, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest testConfiguration(Com.Raritan.Idl.devsettings.Smtp_2_0_0.Configuration cfg, System.Collections.Generic.IEnumerable<string> recipients, AsyncRpcResponse<TestConfigurationResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["cfg"] = cfg.Encode();
        _parameters["recipients"] = new JsonArray(recipients.Select(
          _value => (JsonValue)(_value)));
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("testConfiguration", _parameters,
        _result => {
          try {
            var _ret = new TestConfigurationResult();
            _ret._ret_ = Com.Raritan.Idl.devsettings.Smtp_2_0_0.TestResult.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
