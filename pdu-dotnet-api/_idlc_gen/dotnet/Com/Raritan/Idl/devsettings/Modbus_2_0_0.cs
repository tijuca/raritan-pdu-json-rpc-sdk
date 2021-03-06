// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Modbus.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.devsettings {
  public class Modbus_2_0_0 : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("devsettings.Modbus:2.0.0", null);

    public Modbus_2_0_0(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public Modbus_2_0_0(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new Modbus_2_0_0 StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new Modbus_2_0_0(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public const int ERR_INVALID_PARAM = 1;

    public class Capabilities : ICloneable {
      public object Clone() {
        Capabilities copy = new Capabilities();
        copy.hasModbusSerial = this.hasModbusSerial;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["hasModbusSerial"] = this.hasModbusSerial;
        return json;
      }

      public static Capabilities Decode(LightJson.JsonObject json, Agent agent) {
        Capabilities inst = new Capabilities();
        inst.hasModbusSerial = (bool)json["hasModbusSerial"];
        return inst;
      }

      public bool hasModbusSerial = false;
    }

    public class GetCapabilitiesResult {
      public Com.Raritan.Idl.devsettings.Modbus_2_0_0.Capabilities _ret_;
    }

    public GetCapabilitiesResult getCapabilities() {
      JsonObject _parameters = null;
      var _result = RpcCall("getCapabilities", _parameters);
      var _ret = new GetCapabilitiesResult();
      _ret._ret_ = Com.Raritan.Idl.devsettings.Modbus_2_0_0.Capabilities.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest getCapabilities(AsyncRpcResponse<GetCapabilitiesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getCapabilities(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getCapabilities(AsyncRpcResponse<GetCapabilitiesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getCapabilities", _parameters,
        _result => {
          try {
            var _ret = new GetCapabilitiesResult();
            _ret._ret_ = Com.Raritan.Idl.devsettings.Modbus_2_0_0.Capabilities.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class TcpSettings : ICloneable {
      public object Clone() {
        TcpSettings copy = new TcpSettings();
        copy._readonly = this._readonly;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["readonly"] = this._readonly;
        return json;
      }

      public static TcpSettings Decode(LightJson.JsonObject json, Agent agent) {
        TcpSettings inst = new TcpSettings();
        inst._readonly = (bool)json["readonly"];
        return inst;
      }

      public bool _readonly = false;
    }

    public enum Parity {
      NONE,
      EVEN,
      ODD,
    }

    public class SerialSettings : ICloneable {
      public object Clone() {
        SerialSettings copy = new SerialSettings();
        copy.enabled = this.enabled;
        copy.baudrate = this.baudrate;
        copy.parity = this.parity;
        copy.stopbits = this.stopbits;
        copy._readonly = this._readonly;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["enabled"] = this.enabled;
        json["baudrate"] = this.baudrate;
        json["parity"] = (int)this.parity;
        json["stopbits"] = this.stopbits;
        json["readonly"] = this._readonly;
        return json;
      }

      public static SerialSettings Decode(LightJson.JsonObject json, Agent agent) {
        SerialSettings inst = new SerialSettings();
        inst.enabled = (bool)json["enabled"];
        inst.baudrate = (int)json["baudrate"];
        inst.parity = (Com.Raritan.Idl.devsettings.Modbus_2_0_0.Parity)(int)json["parity"];
        inst.stopbits = (int)json["stopbits"];
        inst._readonly = (bool)json["readonly"];
        return inst;
      }

      public bool enabled = false;
      public int baudrate = 0;
      public Com.Raritan.Idl.devsettings.Modbus_2_0_0.Parity parity = Com.Raritan.Idl.devsettings.Modbus_2_0_0.Parity.NONE;
      public int stopbits = 0;
      public bool _readonly = false;
    }

    public class Settings : ICloneable {
      public object Clone() {
        Settings copy = new Settings();
        copy.tcp = this.tcp;
        copy.serial = this.serial;
        copy.primaryUnitId = this.primaryUnitId;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["tcp"] = this.tcp.Encode();
        json["serial"] = this.serial.Encode();
        json["primaryUnitId"] = this.primaryUnitId;
        return json;
      }

      public static Settings Decode(LightJson.JsonObject json, Agent agent) {
        Settings inst = new Settings();
        inst.tcp = Com.Raritan.Idl.devsettings.Modbus_2_0_0.TcpSettings.Decode(json["tcp"], agent);
        inst.serial = Com.Raritan.Idl.devsettings.Modbus_2_0_0.SerialSettings.Decode(json["serial"], agent);
        inst.primaryUnitId = (int)json["primaryUnitId"];
        return inst;
      }

      public Com.Raritan.Idl.devsettings.Modbus_2_0_0.TcpSettings tcp = new Com.Raritan.Idl.devsettings.Modbus_2_0_0.TcpSettings();
      public Com.Raritan.Idl.devsettings.Modbus_2_0_0.SerialSettings serial = new Com.Raritan.Idl.devsettings.Modbus_2_0_0.SerialSettings();
      public int primaryUnitId = 0;
    }

    public class GetSettingsResult {
      public Com.Raritan.Idl.devsettings.Modbus_2_0_0.Settings _ret_;
    }

    public GetSettingsResult getSettings() {
      JsonObject _parameters = null;
      var _result = RpcCall("getSettings", _parameters);
      var _ret = new GetSettingsResult();
      _ret._ret_ = Com.Raritan.Idl.devsettings.Modbus_2_0_0.Settings.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest getSettings(AsyncRpcResponse<GetSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getSettings(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getSettings(AsyncRpcResponse<GetSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getSettings", _parameters,
        _result => {
          try {
            var _ret = new GetSettingsResult();
            _ret._ret_ = Com.Raritan.Idl.devsettings.Modbus_2_0_0.Settings.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SetSettingsResult {
      public int _ret_;
    }

    public SetSettingsResult setSettings(Com.Raritan.Idl.devsettings.Modbus_2_0_0.Settings settings) {
      var _parameters = new LightJson.JsonObject();
      _parameters["settings"] = settings.Encode();

      var _result = RpcCall("setSettings", _parameters);
      var _ret = new SetSettingsResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest setSettings(Com.Raritan.Idl.devsettings.Modbus_2_0_0.Settings settings, AsyncRpcResponse<SetSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setSettings(settings, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setSettings(Com.Raritan.Idl.devsettings.Modbus_2_0_0.Settings settings, AsyncRpcResponse<SetSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["settings"] = settings.Encode();
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("setSettings", _parameters,
        _result => {
          try {
            var _ret = new SetSettingsResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
