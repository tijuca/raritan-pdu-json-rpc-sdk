// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from OutletGroup.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.pdumodel {
  public class OutletGroup : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("pdumodel.OutletGroup:1.0.0", null);

    public OutletGroup(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public OutletGroup(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new OutletGroup StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new OutletGroup(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public const int ERR_INVALID_ARGUMENT = 1;

    public class Sensors : ICloneable {
      public object Clone() {
        Sensors copy = new Sensors();
        copy.activePower = this.activePower;
        copy.activeEnergy = this.activeEnergy;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["activePower"] = this.activePower != null ? this.activePower.Encode() : JsonValue.Null;
        json["activeEnergy"] = this.activeEnergy != null ? this.activeEnergy.Encode() : JsonValue.Null;
        return json;
      }

      public static Sensors Decode(LightJson.JsonObject json, Agent agent) {
        Sensors inst = new Sensors();
        inst.activePower = Com.Raritan.Idl.sensors.NumericSensor_4_0_2.StaticCast(ObjectProxy.Decode(json["activePower"], agent));
        inst.activeEnergy = Com.Raritan.Idl.sensors.AccumulatingNumericSensor_2_0_2.StaticCast(ObjectProxy.Decode(json["activeEnergy"], agent));
        return inst;
      }

      public Com.Raritan.Idl.sensors.NumericSensor_4_0_2 activePower = null;
      public Com.Raritan.Idl.sensors.AccumulatingNumericSensor_2_0_2 activeEnergy = null;
    }

    public class Settings : ICloneable {
      public object Clone() {
        Settings copy = new Settings();
        copy.name = this.name;
        copy.members = this.members;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["name"] = this.name;
        json["members"] = new JsonArray(this.members.Select(
          _value => (JsonValue)(_value != null ? _value.Encode() : JsonValue.Null)));
        return json;
      }

      public static Settings Decode(LightJson.JsonObject json, Agent agent) {
        Settings inst = new Settings();
        inst.name = (string)json["name"];
        inst.members = new System.Collections.Generic.List<Com.Raritan.Idl.pdumodel.Outlet_2_1_4>(json["members"].AsJsonArray.Select(
          _value => Com.Raritan.Idl.pdumodel.Outlet_2_1_4.StaticCast(ObjectProxy.Decode(_value, agent))));
        return inst;
      }

      public string name = "";
      public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.pdumodel.Outlet_2_1_4> members = new System.Collections.Generic.List<Com.Raritan.Idl.pdumodel.Outlet_2_1_4>();
    }

    public class MetaData : ICloneable {
      public object Clone() {
        MetaData copy = new MetaData();
        copy.groupId = this.groupId;
        copy.uniqueId = this.uniqueId;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["groupId"] = this.groupId;
        json["uniqueId"] = this.uniqueId;
        return json;
      }

      public static MetaData Decode(LightJson.JsonObject json, Agent agent) {
        MetaData inst = new MetaData();
        inst.groupId = (int)json["groupId"];
        inst.uniqueId = (int)json["uniqueId"];
        return inst;
      }

      public int groupId = 0;
      public int uniqueId = 0;
    }

    public class SensorsChangedEvent : Com.Raritan.Idl.idl.Event {
      static public readonly new TypeInfo typeInfo = new TypeInfo("pdumodel.OutletGroup.SensorsChangedEvent:1.0.0", Com.Raritan.Idl.idl.Event.typeInfo);

      public Com.Raritan.Idl.pdumodel.OutletGroup.Sensors oldSensors = new Com.Raritan.Idl.pdumodel.OutletGroup.Sensors();
      public Com.Raritan.Idl.pdumodel.OutletGroup.Sensors newSensors = new Com.Raritan.Idl.pdumodel.OutletGroup.Sensors();
    }

    public class SettingsChangedEvent : Com.Raritan.Idl._event.UserEvent {
      static public readonly new TypeInfo typeInfo = new TypeInfo("pdumodel.OutletGroup.SettingsChangedEvent:1.0.0", Com.Raritan.Idl._event.UserEvent.typeInfo);

      public Com.Raritan.Idl.pdumodel.OutletGroup.Settings oldSettings = new Com.Raritan.Idl.pdumodel.OutletGroup.Settings();
      public Com.Raritan.Idl.pdumodel.OutletGroup.Settings newSettings = new Com.Raritan.Idl.pdumodel.OutletGroup.Settings();
    }

    public class PowerControlEvent : Com.Raritan.Idl._event.UserEvent {
      static public readonly new TypeInfo typeInfo = new TypeInfo("pdumodel.OutletGroup.PowerControlEvent:1.0.0", Com.Raritan.Idl._event.UserEvent.typeInfo);

      public Com.Raritan.Idl.pdumodel.Outlet_2_1_4.PowerState state = Com.Raritan.Idl.pdumodel.Outlet_2_1_4.PowerState.PS_OFF;
      public bool cycle = false;
    }

    public class GetSensorsResult {
      public Com.Raritan.Idl.pdumodel.OutletGroup.Sensors _ret_;
    }

    public GetSensorsResult getSensors() {
      JsonObject _parameters = null;
      var _result = RpcCall("getSensors", _parameters);
      var _ret = new GetSensorsResult();
      _ret._ret_ = Com.Raritan.Idl.pdumodel.OutletGroup.Sensors.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest getSensors(AsyncRpcResponse<GetSensorsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getSensors(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getSensors(AsyncRpcResponse<GetSensorsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getSensors", _parameters,
        _result => {
          try {
            var _ret = new GetSensorsResult();
            _ret._ret_ = Com.Raritan.Idl.pdumodel.OutletGroup.Sensors.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetMetaDataResult {
      public Com.Raritan.Idl.pdumodel.OutletGroup.MetaData _ret_;
    }

    public GetMetaDataResult getMetaData() {
      JsonObject _parameters = null;
      var _result = RpcCall("getMetaData", _parameters);
      var _ret = new GetMetaDataResult();
      _ret._ret_ = Com.Raritan.Idl.pdumodel.OutletGroup.MetaData.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest getMetaData(AsyncRpcResponse<GetMetaDataResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getMetaData(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getMetaData(AsyncRpcResponse<GetMetaDataResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getMetaData", _parameters,
        _result => {
          try {
            var _ret = new GetMetaDataResult();
            _ret._ret_ = Com.Raritan.Idl.pdumodel.OutletGroup.MetaData.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetSettingsResult {
      public Com.Raritan.Idl.pdumodel.OutletGroup.Settings _ret_;
    }

    public GetSettingsResult getSettings() {
      JsonObject _parameters = null;
      var _result = RpcCall("getSettings", _parameters);
      var _ret = new GetSettingsResult();
      _ret._ret_ = Com.Raritan.Idl.pdumodel.OutletGroup.Settings.Decode(_result["_ret_"], agent);
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
            _ret._ret_ = Com.Raritan.Idl.pdumodel.OutletGroup.Settings.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SetSettingsResult {
      public int _ret_;
    }

    public SetSettingsResult setSettings(Com.Raritan.Idl.pdumodel.OutletGroup.Settings settings) {
      var _parameters = new LightJson.JsonObject();
      _parameters["settings"] = settings.Encode();

      var _result = RpcCall("setSettings", _parameters);
      var _ret = new SetSettingsResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest setSettings(Com.Raritan.Idl.pdumodel.OutletGroup.Settings settings, AsyncRpcResponse<SetSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setSettings(settings, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setSettings(Com.Raritan.Idl.pdumodel.OutletGroup.Settings settings, AsyncRpcResponse<SetSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
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

    public class SetAllOutletPowerStatesResult {
      public int _ret_;
    }

    public SetAllOutletPowerStatesResult setAllOutletPowerStates(Com.Raritan.Idl.pdumodel.Outlet_2_1_4.PowerState pstate) {
      var _parameters = new LightJson.JsonObject();
      _parameters["pstate"] = (int)pstate;

      var _result = RpcCall("setAllOutletPowerStates", _parameters);
      var _ret = new SetAllOutletPowerStatesResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest setAllOutletPowerStates(Com.Raritan.Idl.pdumodel.Outlet_2_1_4.PowerState pstate, AsyncRpcResponse<SetAllOutletPowerStatesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setAllOutletPowerStates(pstate, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setAllOutletPowerStates(Com.Raritan.Idl.pdumodel.Outlet_2_1_4.PowerState pstate, AsyncRpcResponse<SetAllOutletPowerStatesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["pstate"] = (int)pstate;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("setAllOutletPowerStates", _parameters,
        _result => {
          try {
            var _ret = new SetAllOutletPowerStatesResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class CycleAllOutletPowerStatesResult {
      public int _ret_;
    }

    public CycleAllOutletPowerStatesResult cycleAllOutletPowerStates() {
      JsonObject _parameters = null;
      var _result = RpcCall("cycleAllOutletPowerStates", _parameters);
      var _ret = new CycleAllOutletPowerStatesResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest cycleAllOutletPowerStates(AsyncRpcResponse<CycleAllOutletPowerStatesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return cycleAllOutletPowerStates(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest cycleAllOutletPowerStates(AsyncRpcResponse<CycleAllOutletPowerStatesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("cycleAllOutletPowerStates", _parameters,
        _result => {
          try {
            var _ret = new CycleAllOutletPowerStatesResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}