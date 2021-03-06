// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from AssetStrip.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.assetmgrmodel {
  public class AssetStrip : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("assetmgrmodel.AssetStrip:1.0.0", null);

    public AssetStrip(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public AssetStrip(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new AssetStrip StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new AssetStrip(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public const int NO_ERROR = 0;

    public const int ERR_INVALID_PARAM = 1;

    public const int ERR_NOT_AVAILABLE = 2;

    public const int ERR_NO_SUCH_OBJECT = 3;

    public enum Events {
      EVT_KEY_STATE_CHANGED,
      EVT_KEY_SETTINGS_CHANGED,
      EVT_KEY_TAGS_CHANGED,
    }

    public enum State {
      DISCONNECTED,
      FIRMWARE_UPDATE,
      UNSUPPORTED,
      AVAILABLE,
    }

    public enum LEDOperationMode {
      LED_OPERATION_MANUAL,
      LED_OPERATION_AUTO,
    }

    public enum LEDMode {
      LED_MODE_ON,
      LED_MODE_OFF,
      LED_MODE_BLINK_FAST,
      LED_MODE_BLINK_SLOW,
    }

    public enum Orientation {
      TOP_CONNECTOR,
      BOTTOM_CONNECTOR,
      UNKNOWN_ORIENTATION,
    }

    public enum NumberingMode {
      TOP_DOWN,
      BOTTOM_UP,
    }

    public enum ScanMode {
      SCANMODE_DISABLED,
      SCANMODE_BOTH,
    }

    public class DeviceInfo : ICloneable {
      public object Clone() {
        DeviceInfo copy = new DeviceInfo();
        copy.deviceId = this.deviceId;
        copy.hardwareId = this.hardwareId;
        copy.protocolVersion = this.protocolVersion;
        copy.bootVersion = this.bootVersion;
        copy.appVersion = this.appVersion;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["deviceId"] = this.deviceId;
        json["hardwareId"] = this.hardwareId;
        json["protocolVersion"] = this.protocolVersion;
        json["bootVersion"] = this.bootVersion;
        json["appVersion"] = this.appVersion;
        return json;
      }

      public static DeviceInfo Decode(LightJson.JsonObject json, Agent agent) {
        DeviceInfo inst = new DeviceInfo();
        inst.deviceId = (int)json["deviceId"];
        inst.hardwareId = (int)json["hardwareId"];
        inst.protocolVersion = (int)json["protocolVersion"];
        inst.bootVersion = (int)json["bootVersion"];
        inst.appVersion = (int)json["appVersion"];
        return inst;
      }

      public int deviceId = 0;
      public int hardwareId = 0;
      public int protocolVersion = 0;
      public int bootVersion = 0;
      public int appVersion = 0;
    }

    public class TagInfo : ICloneable {
      public object Clone() {
        TagInfo copy = new TagInfo();
        copy.channel = this.channel;
        copy.familyDesc = this.familyDesc;
        copy.rawId = this.rawId;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["channel"] = this.channel;
        json["familyDesc"] = this.familyDesc;
        json["rawId"] = this.rawId;
        return json;
      }

      public static TagInfo Decode(LightJson.JsonObject json, Agent agent) {
        TagInfo inst = new TagInfo();
        inst.channel = (int)json["channel"];
        inst.familyDesc = (string)json["familyDesc"];
        inst.rawId = (string)json["rawId"];
        return inst;
      }

      public int channel = 0;
      public string familyDesc = "";
      public string rawId = "";
    }

    public class LEDColor : ICloneable {
      public object Clone() {
        LEDColor copy = new LEDColor();
        copy.r = this.r;
        copy.g = this.g;
        copy.b = this.b;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["r"] = this.r;
        json["g"] = this.g;
        json["b"] = this.b;
        return json;
      }

      public static LEDColor Decode(LightJson.JsonObject json, Agent agent) {
        LEDColor inst = new LEDColor();
        inst.r = (int)json["r"];
        inst.g = (int)json["g"];
        inst.b = (int)json["b"];
        return inst;
      }

      public int r = 0;
      public int g = 0;
      public int b = 0;
    }

    public class StripSettings : ICloneable {
      public object Clone() {
        StripSettings copy = new StripSettings();
        copy.channelCount = this.channelCount;
        copy.name = this.name;
        copy.scanMode = this.scanMode;
        copy.defaultColorConnected = this.defaultColorConnected;
        copy.defaultColorDisconnected = this.defaultColorDisconnected;
        copy.numberingMode = this.numberingMode;
        copy.numberingOffset = this.numberingOffset;
        copy.orientationSensAvailable = this.orientationSensAvailable;
        copy.orientation = this.orientation;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["channelCount"] = this.channelCount;
        json["name"] = this.name;
        json["scanMode"] = (int)this.scanMode;
        json["defaultColorConnected"] = this.defaultColorConnected.Encode();
        json["defaultColorDisconnected"] = this.defaultColorDisconnected.Encode();
        json["numberingMode"] = (int)this.numberingMode;
        json["numberingOffset"] = this.numberingOffset;
        json["orientationSensAvailable"] = this.orientationSensAvailable;
        json["orientation"] = (int)this.orientation;
        return json;
      }

      public static StripSettings Decode(LightJson.JsonObject json, Agent agent) {
        StripSettings inst = new StripSettings();
        inst.channelCount = (int)json["channelCount"];
        inst.name = (string)json["name"];
        inst.scanMode = (Com.Raritan.Idl.assetmgrmodel.AssetStrip.ScanMode)(int)json["scanMode"];
        inst.defaultColorConnected = Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor.Decode(json["defaultColorConnected"], agent);
        inst.defaultColorDisconnected = Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor.Decode(json["defaultColorDisconnected"], agent);
        inst.numberingMode = (Com.Raritan.Idl.assetmgrmodel.AssetStrip.NumberingMode)(int)json["numberingMode"];
        inst.numberingOffset = (int)json["numberingOffset"];
        inst.orientationSensAvailable = (bool)json["orientationSensAvailable"];
        inst.orientation = (Com.Raritan.Idl.assetmgrmodel.AssetStrip.Orientation)(int)json["orientation"];
        return inst;
      }

      public int channelCount = 0;
      public string name = "";
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.ScanMode scanMode = Com.Raritan.Idl.assetmgrmodel.AssetStrip.ScanMode.SCANMODE_DISABLED;
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor defaultColorConnected = new Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor();
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor defaultColorDisconnected = new Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor();
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.NumberingMode numberingMode = Com.Raritan.Idl.assetmgrmodel.AssetStrip.NumberingMode.TOP_DOWN;
      public int numberingOffset = 0;
      public bool orientationSensAvailable = false;
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.Orientation orientation = Com.Raritan.Idl.assetmgrmodel.AssetStrip.Orientation.TOP_CONNECTOR;
    }

    public class RackUnitSettings : ICloneable {
      public object Clone() {
        RackUnitSettings copy = new RackUnitSettings();
        copy.opmode = this.opmode;
        copy.mode = this.mode;
        copy.color = this.color;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["opmode"] = (int)this.opmode;
        json["mode"] = (int)this.mode;
        json["color"] = this.color.Encode();
        return json;
      }

      public static RackUnitSettings Decode(LightJson.JsonObject json, Agent agent) {
        RackUnitSettings inst = new RackUnitSettings();
        inst.opmode = (Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDOperationMode)(int)json["opmode"];
        inst.mode = (Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDMode)(int)json["mode"];
        inst.color = Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor.Decode(json["color"], agent);
        return inst;
      }

      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDOperationMode opmode = Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDOperationMode.LED_OPERATION_MANUAL;
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDMode mode = Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDMode.LED_MODE_ON;
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor color = new Com.Raritan.Idl.assetmgrmodel.AssetStrip.LEDColor();
    }

    public class RackUnitInfo : ICloneable {
      public object Clone() {
        RackUnitInfo copy = new RackUnitInfo();
        copy.channel = this.channel;
        copy.rackUnitPosition = this.rackUnitPosition;
        copy.settings = this.settings;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["channel"] = this.channel;
        json["rackUnitPosition"] = this.rackUnitPosition;
        json["settings"] = this.settings.Encode();
        return json;
      }

      public static RackUnitInfo Decode(LightJson.JsonObject json, Agent agent) {
        RackUnitInfo inst = new RackUnitInfo();
        inst.channel = (int)json["channel"];
        inst.rackUnitPosition = (int)json["rackUnitPosition"];
        inst.settings = Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings.Decode(json["settings"], agent);
        return inst;
      }

      public int channel = 0;
      public int rackUnitPosition = 0;
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings settings = new Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings();
    }

    public class GetStateResult {
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.State _ret_;
    }

    public GetStateResult getState() {
      JsonObject _parameters = null;
      var _result = RpcCall("getState", _parameters);
      var _ret = new GetStateResult();
      _ret._ret_ = (Com.Raritan.Idl.assetmgrmodel.AssetStrip.State)(int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest getState(AsyncRpcResponse<GetStateResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getState(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getState(AsyncRpcResponse<GetStateResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getState", _parameters,
        _result => {
          try {
            var _ret = new GetStateResult();
            _ret._ret_ = (Com.Raritan.Idl.assetmgrmodel.AssetStrip.State)(int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetDeviceInfoResult {
      public int _ret_;
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.DeviceInfo info;
    }

    public GetDeviceInfoResult getDeviceInfo() {
      JsonObject _parameters = null;
      var _result = RpcCall("getDeviceInfo", _parameters);
      var _ret = new GetDeviceInfoResult();
      _ret._ret_ = (int)_result["_ret_"];
      _ret.info = Com.Raritan.Idl.assetmgrmodel.AssetStrip.DeviceInfo.Decode(_result["info"], agent);
      return _ret;
    }

    public AsyncRequest getDeviceInfo(AsyncRpcResponse<GetDeviceInfoResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getDeviceInfo(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getDeviceInfo(AsyncRpcResponse<GetDeviceInfoResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getDeviceInfo", _parameters,
        _result => {
          try {
            var _ret = new GetDeviceInfoResult();
            _ret._ret_ = (int)_result["_ret_"];
            _ret.info = Com.Raritan.Idl.assetmgrmodel.AssetStrip.DeviceInfo.Decode(_result["info"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetStripSettingsResult {
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.StripSettings _ret_;
    }

    public GetStripSettingsResult getStripSettings() {
      JsonObject _parameters = null;
      var _result = RpcCall("getStripSettings", _parameters);
      var _ret = new GetStripSettingsResult();
      _ret._ret_ = Com.Raritan.Idl.assetmgrmodel.AssetStrip.StripSettings.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest getStripSettings(AsyncRpcResponse<GetStripSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getStripSettings(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getStripSettings(AsyncRpcResponse<GetStripSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getStripSettings", _parameters,
        _result => {
          try {
            var _ret = new GetStripSettingsResult();
            _ret._ret_ = Com.Raritan.Idl.assetmgrmodel.AssetStrip.StripSettings.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SetStripSettingsResult {
      public int _ret_;
    }

    public SetStripSettingsResult setStripSettings(Com.Raritan.Idl.assetmgrmodel.AssetStrip.StripSettings settings) {
      var _parameters = new LightJson.JsonObject();
      _parameters["settings"] = settings.Encode();

      var _result = RpcCall("setStripSettings", _parameters);
      var _ret = new SetStripSettingsResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest setStripSettings(Com.Raritan.Idl.assetmgrmodel.AssetStrip.StripSettings settings, AsyncRpcResponse<SetStripSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setStripSettings(settings, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setStripSettings(Com.Raritan.Idl.assetmgrmodel.AssetStrip.StripSettings settings, AsyncRpcResponse<SetStripSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["settings"] = settings.Encode();
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("setStripSettings", _parameters,
        _result => {
          try {
            var _ret = new SetStripSettingsResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SetRackUnitSettingsResult {
      public int _ret_;
    }

    public SetRackUnitSettingsResult setRackUnitSettings(int channel, Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings settings) {
      var _parameters = new LightJson.JsonObject();
      _parameters["channel"] = channel;
      _parameters["settings"] = settings.Encode();

      var _result = RpcCall("setRackUnitSettings", _parameters);
      var _ret = new SetRackUnitSettingsResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest setRackUnitSettings(int channel, Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings settings, AsyncRpcResponse<SetRackUnitSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setRackUnitSettings(channel, settings, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setRackUnitSettings(int channel, Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings settings, AsyncRpcResponse<SetRackUnitSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["channel"] = channel;
        _parameters["settings"] = settings.Encode();
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("setRackUnitSettings", _parameters,
        _result => {
          try {
            var _ret = new SetRackUnitSettingsResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SetMultipleRackUnitSettingsResult {
      public int _ret_;
    }

    public SetMultipleRackUnitSettingsResult setMultipleRackUnitSettings(System.Collections.Generic.IEnumerable<int> channels, System.Collections.Generic.IEnumerable<Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings> settings) {
      var _parameters = new LightJson.JsonObject();
      _parameters["channels"] = new JsonArray(channels.Select(
        _value => (JsonValue)(_value)));
      _parameters["settings"] = new JsonArray(settings.Select(
        _value => (JsonValue)(_value.Encode())));

      var _result = RpcCall("setMultipleRackUnitSettings", _parameters);
      var _ret = new SetMultipleRackUnitSettingsResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest setMultipleRackUnitSettings(System.Collections.Generic.IEnumerable<int> channels, System.Collections.Generic.IEnumerable<Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings> settings, AsyncRpcResponse<SetMultipleRackUnitSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setMultipleRackUnitSettings(channels, settings, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setMultipleRackUnitSettings(System.Collections.Generic.IEnumerable<int> channels, System.Collections.Generic.IEnumerable<Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitSettings> settings, AsyncRpcResponse<SetMultipleRackUnitSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["channels"] = new JsonArray(channels.Select(
          _value => (JsonValue)(_value)));
        _parameters["settings"] = new JsonArray(settings.Select(
          _value => (JsonValue)(_value.Encode())));
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("setMultipleRackUnitSettings", _parameters,
        _result => {
          try {
            var _ret = new SetMultipleRackUnitSettingsResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetRackUnitInfoResult {
      public int _ret_;
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitInfo info;
    }

    public GetRackUnitInfoResult getRackUnitInfo(int channel) {
      var _parameters = new LightJson.JsonObject();
      _parameters["channel"] = channel;

      var _result = RpcCall("getRackUnitInfo", _parameters);
      var _ret = new GetRackUnitInfoResult();
      _ret._ret_ = (int)_result["_ret_"];
      _ret.info = Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitInfo.Decode(_result["info"], agent);
      return _ret;
    }

    public AsyncRequest getRackUnitInfo(int channel, AsyncRpcResponse<GetRackUnitInfoResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getRackUnitInfo(channel, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getRackUnitInfo(int channel, AsyncRpcResponse<GetRackUnitInfoResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["channel"] = channel;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("getRackUnitInfo", _parameters,
        _result => {
          try {
            var _ret = new GetRackUnitInfoResult();
            _ret._ret_ = (int)_result["_ret_"];
            _ret.info = Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitInfo.Decode(_result["info"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetAllRackUnitInfosResult {
      public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitInfo> _ret_;
    }

    public GetAllRackUnitInfosResult getAllRackUnitInfos() {
      JsonObject _parameters = null;
      var _result = RpcCall("getAllRackUnitInfos", _parameters);
      var _ret = new GetAllRackUnitInfosResult();
      _ret._ret_ = new System.Collections.Generic.List<Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitInfo>(_result["_ret_"].AsJsonArray.Select(
        _value => Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitInfo.Decode(_value, agent)));
      return _ret;
    }

    public AsyncRequest getAllRackUnitInfos(AsyncRpcResponse<GetAllRackUnitInfosResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getAllRackUnitInfos(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getAllRackUnitInfos(AsyncRpcResponse<GetAllRackUnitInfosResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getAllRackUnitInfos", _parameters,
        _result => {
          try {
            var _ret = new GetAllRackUnitInfosResult();
            _ret._ret_ = new System.Collections.Generic.List<Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitInfo>(_result["_ret_"].AsJsonArray.Select(
              _value => Com.Raritan.Idl.assetmgrmodel.AssetStrip.RackUnitInfo.Decode(_value, agent)));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetTagResult {
      public int _ret_;
      public Com.Raritan.Idl.assetmgrmodel.AssetStrip.TagInfo tagInfo;
    }

    public GetTagResult getTag(int channel) {
      var _parameters = new LightJson.JsonObject();
      _parameters["channel"] = channel;

      var _result = RpcCall("getTag", _parameters);
      var _ret = new GetTagResult();
      _ret._ret_ = (int)_result["_ret_"];
      _ret.tagInfo = Com.Raritan.Idl.assetmgrmodel.AssetStrip.TagInfo.Decode(_result["tagInfo"], agent);
      return _ret;
    }

    public AsyncRequest getTag(int channel, AsyncRpcResponse<GetTagResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getTag(channel, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getTag(int channel, AsyncRpcResponse<GetTagResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["channel"] = channel;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("getTag", _parameters,
        _result => {
          try {
            var _ret = new GetTagResult();
            _ret._ret_ = (int)_result["_ret_"];
            _ret.tagInfo = Com.Raritan.Idl.assetmgrmodel.AssetStrip.TagInfo.Decode(_result["tagInfo"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetAllTagsResult {
      public int _ret_;
      public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.assetmgrmodel.AssetStrip.TagInfo> tags;
    }

    public GetAllTagsResult getAllTags() {
      JsonObject _parameters = null;
      var _result = RpcCall("getAllTags", _parameters);
      var _ret = new GetAllTagsResult();
      _ret._ret_ = (int)_result["_ret_"];
      _ret.tags = new System.Collections.Generic.List<Com.Raritan.Idl.assetmgrmodel.AssetStrip.TagInfo>(_result["tags"].AsJsonArray.Select(
        _value => Com.Raritan.Idl.assetmgrmodel.AssetStrip.TagInfo.Decode(_value, agent)));
      return _ret;
    }

    public AsyncRequest getAllTags(AsyncRpcResponse<GetAllTagsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getAllTags(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getAllTags(AsyncRpcResponse<GetAllTagsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getAllTags", _parameters,
        _result => {
          try {
            var _ret = new GetAllTagsResult();
            _ret._ret_ = (int)_result["_ret_"];
            _ret.tags = new System.Collections.Generic.List<Com.Raritan.Idl.assetmgrmodel.AssetStrip.TagInfo>(_result["tags"].AsJsonArray.Select(
              _value => Com.Raritan.Idl.assetmgrmodel.AssetStrip.TagInfo.Decode(_value, agent)));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class TriggerPowercycleResult {
      public int _ret_;
    }

    public TriggerPowercycleResult triggerPowercycle(bool hard) {
      var _parameters = new LightJson.JsonObject();
      _parameters["hard"] = hard;

      var _result = RpcCall("triggerPowercycle", _parameters);
      var _ret = new TriggerPowercycleResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest triggerPowercycle(bool hard, AsyncRpcResponse<TriggerPowercycleResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return triggerPowercycle(hard, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest triggerPowercycle(bool hard, AsyncRpcResponse<TriggerPowercycleResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["hard"] = hard;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("triggerPowercycle", _parameters,
        _result => {
          try {
            var _ret = new TriggerPowercycleResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
