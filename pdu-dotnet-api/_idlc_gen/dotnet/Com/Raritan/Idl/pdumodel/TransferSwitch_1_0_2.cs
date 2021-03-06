// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from TransferSwitch.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.pdumodel {
  public class TransferSwitch_1_0_2 : Com.Raritan.Idl.pdumodel.EDevice {

    static public readonly new TypeInfo typeInfo = new TypeInfo("pdumodel.TransferSwitch:1.0.2", null);

    public TransferSwitch_1_0_2(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public TransferSwitch_1_0_2(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new TransferSwitch_1_0_2 StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new TransferSwitch_1_0_2(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public const int ERR_INVALID_PARAM = 1;

    public const int ERR_SWITCH_FAULT = 2;

    public const int ERR_SWITCH_POWER_FAIL = 3;

    public enum Events {
      EVT_KEY_SETTINGS_CHANGED,
    }

    public enum Type {
      STS,
    }

    public enum TransferReason {
      REASON_UNKNOWN,
      REASON_STARTUP,
      REASON_MANUAL_TRANSFER,
      REASON_AUTO_RETRANSFER,
      REASON_POWER_FAILURE,
      REASON_POWER_QUALITY,
      REASON_OVERLOAD,
      REASON_OVERHEAT,
      REASON_INTERNAL_FAILURE,
    }

    public class MetaData : ICloneable {
      public object Clone() {
        MetaData copy = new MetaData();
        copy.label = this.label;
        copy.namePlate = this.namePlate;
        copy.rating = this.rating;
        copy.type = this.type;
        copy.sourceCount = this.sourceCount;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["label"] = this.label;
        json["namePlate"] = this.namePlate.Encode();
        json["rating"] = this.rating.Encode();
        json["type"] = (int)this.type;
        json["sourceCount"] = this.sourceCount;
        return json;
      }

      public static MetaData Decode(LightJson.JsonObject json, Agent agent) {
        MetaData inst = new MetaData();
        inst.label = (string)json["label"];
        inst.namePlate = Com.Raritan.Idl.pdumodel.Nameplate.Decode(json["namePlate"], agent);
        inst.rating = Com.Raritan.Idl.pdumodel.Rating.Decode(json["rating"], agent);
        inst.type = (Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Type)(int)json["type"];
        inst.sourceCount = (int)json["sourceCount"];
        return inst;
      }

      public string label = "";
      public Com.Raritan.Idl.pdumodel.Nameplate namePlate = new Com.Raritan.Idl.pdumodel.Nameplate();
      public Com.Raritan.Idl.pdumodel.Rating rating = new Com.Raritan.Idl.pdumodel.Rating();
      public Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Type type = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Type.STS;
      public int sourceCount = 0;
    }

    public const int OPERATIONAL_STATE_OFF = 0;

    public const int OPERATIONAL_STATE_NORMAL = 1;

    public const int OPERATIONAL_STATE_STANDBY = 2;

    public class Sensors : ICloneable {
      public object Clone() {
        Sensors copy = new Sensors();
        copy.selectedSource = this.selectedSource;
        copy.operationalState = this.operationalState;
        copy.sourceVoltagePhaseSyncAngle = this.sourceVoltagePhaseSyncAngle;
        copy.temperature = this.temperature;
        copy.fanSpeed = this.fanSpeed;
        copy.overloadAlarm = this.overloadAlarm;
        copy.overheatAlarm = this.overheatAlarm;
        copy.phaseSyncAlarm = this.phaseSyncAlarm;
        copy.movFault = this.movFault;
        copy.scrOpenFault = this.scrOpenFault;
        copy.scrShortFault = this.scrShortFault;
        copy.fanFault = this.fanFault;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["selectedSource"] = this.selectedSource != null ? this.selectedSource.Encode() : JsonValue.Null;
        json["operationalState"] = this.operationalState != null ? this.operationalState.Encode() : JsonValue.Null;
        json["sourceVoltagePhaseSyncAngle"] = this.sourceVoltagePhaseSyncAngle != null ? this.sourceVoltagePhaseSyncAngle.Encode() : JsonValue.Null;
        json["temperature"] = this.temperature != null ? this.temperature.Encode() : JsonValue.Null;
        json["fanSpeed"] = this.fanSpeed != null ? this.fanSpeed.Encode() : JsonValue.Null;
        json["overloadAlarm"] = this.overloadAlarm != null ? this.overloadAlarm.Encode() : JsonValue.Null;
        json["overheatAlarm"] = this.overheatAlarm != null ? this.overheatAlarm.Encode() : JsonValue.Null;
        json["phaseSyncAlarm"] = this.phaseSyncAlarm != null ? this.phaseSyncAlarm.Encode() : JsonValue.Null;
        json["movFault"] = this.movFault != null ? this.movFault.Encode() : JsonValue.Null;
        json["scrOpenFault"] = this.scrOpenFault != null ? this.scrOpenFault.Encode() : JsonValue.Null;
        json["scrShortFault"] = this.scrShortFault != null ? this.scrShortFault.Encode() : JsonValue.Null;
        json["fanFault"] = this.fanFault != null ? this.fanFault.Encode() : JsonValue.Null;
        return json;
      }

      public static Sensors Decode(LightJson.JsonObject json, Agent agent) {
        Sensors inst = new Sensors();
        inst.selectedSource = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["selectedSource"], agent));
        inst.operationalState = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["operationalState"], agent));
        inst.sourceVoltagePhaseSyncAngle = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["sourceVoltagePhaseSyncAngle"], agent));
        inst.temperature = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["temperature"], agent));
        inst.fanSpeed = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["fanSpeed"], agent));
        inst.overloadAlarm = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["overloadAlarm"], agent));
        inst.overheatAlarm = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["overheatAlarm"], agent));
        inst.phaseSyncAlarm = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["phaseSyncAlarm"], agent));
        inst.movFault = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["movFault"], agent));
        inst.scrOpenFault = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["scrOpenFault"], agent));
        inst.scrShortFault = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["scrShortFault"], agent));
        inst.fanFault = Com.Raritan.Idl.sensors.StateSensor_3_0_1.StaticCast(ObjectProxy.Decode(json["fanFault"], agent));
        return inst;
      }

      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 selectedSource = null;
      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 operationalState = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 sourceVoltagePhaseSyncAngle = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 temperature = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 fanSpeed = null;
      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 overloadAlarm = null;
      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 overheatAlarm = null;
      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 phaseSyncAlarm = null;
      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 movFault = null;
      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 scrOpenFault = null;
      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 scrShortFault = null;
      public Com.Raritan.Idl.sensors.StateSensor_3_0_1 fanFault = null;
    }

    public class OutputSensors : ICloneable {
      public object Clone() {
        OutputSensors copy = new OutputSensors();
        copy.voltage = this.voltage;
        copy.current = this.current;
        copy.peakCurrent = this.peakCurrent;
        copy.activePower = this.activePower;
        copy.apparentPower = this.apparentPower;
        copy.powerFactor = this.powerFactor;
        copy.activeEnergy = this.activeEnergy;
        copy.apparentEnergy = this.apparentEnergy;
        copy.unbalancedCurrent = this.unbalancedCurrent;
        copy.lineFrequency = this.lineFrequency;
        copy.phaseAngle = this.phaseAngle;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["voltage"] = this.voltage != null ? this.voltage.Encode() : JsonValue.Null;
        json["current"] = this.current != null ? this.current.Encode() : JsonValue.Null;
        json["peakCurrent"] = this.peakCurrent != null ? this.peakCurrent.Encode() : JsonValue.Null;
        json["activePower"] = this.activePower != null ? this.activePower.Encode() : JsonValue.Null;
        json["apparentPower"] = this.apparentPower != null ? this.apparentPower.Encode() : JsonValue.Null;
        json["powerFactor"] = this.powerFactor != null ? this.powerFactor.Encode() : JsonValue.Null;
        json["activeEnergy"] = this.activeEnergy != null ? this.activeEnergy.Encode() : JsonValue.Null;
        json["apparentEnergy"] = this.apparentEnergy != null ? this.apparentEnergy.Encode() : JsonValue.Null;
        json["unbalancedCurrent"] = this.unbalancedCurrent != null ? this.unbalancedCurrent.Encode() : JsonValue.Null;
        json["lineFrequency"] = this.lineFrequency != null ? this.lineFrequency.Encode() : JsonValue.Null;
        json["phaseAngle"] = this.phaseAngle != null ? this.phaseAngle.Encode() : JsonValue.Null;
        return json;
      }

      public static OutputSensors Decode(LightJson.JsonObject json, Agent agent) {
        OutputSensors inst = new OutputSensors();
        inst.voltage = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["voltage"], agent));
        inst.current = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["current"], agent));
        inst.peakCurrent = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["peakCurrent"], agent));
        inst.activePower = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["activePower"], agent));
        inst.apparentPower = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["apparentPower"], agent));
        inst.powerFactor = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["powerFactor"], agent));
        inst.activeEnergy = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["activeEnergy"], agent));
        inst.apparentEnergy = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["apparentEnergy"], agent));
        inst.unbalancedCurrent = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["unbalancedCurrent"], agent));
        inst.lineFrequency = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["lineFrequency"], agent));
        inst.phaseAngle = Com.Raritan.Idl.sensors.NumericSensor_3_0_0.StaticCast(ObjectProxy.Decode(json["phaseAngle"], agent));
        return inst;
      }

      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 voltage = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 current = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 peakCurrent = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 activePower = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 apparentPower = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 powerFactor = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 activeEnergy = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 apparentEnergy = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 unbalancedCurrent = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 lineFrequency = null;
      public Com.Raritan.Idl.sensors.NumericSensor_3_0_0 phaseAngle = null;
    }

    public class PowerQualitySettings : ICloneable {
      public object Clone() {
        PowerQualitySettings copy = new PowerQualitySettings();
        copy.minMarginalVoltage = this.minMarginalVoltage;
        copy.minGoodVoltage = this.minGoodVoltage;
        copy.maxGoodVoltage = this.maxGoodVoltage;
        copy.maxMarginalVoltage = this.maxMarginalVoltage;
        copy.voltageHysteresis = this.voltageHysteresis;
        copy.voltageDetectTime = this.voltageDetectTime;
        copy.minGoodFrequency = this.minGoodFrequency;
        copy.maxGoodFrequency = this.maxGoodFrequency;
        copy.frequencyHysteresis = this.frequencyHysteresis;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["minMarginalVoltage"] = this.minMarginalVoltage;
        json["minGoodVoltage"] = this.minGoodVoltage;
        json["maxGoodVoltage"] = this.maxGoodVoltage;
        json["maxMarginalVoltage"] = this.maxMarginalVoltage;
        json["voltageHysteresis"] = this.voltageHysteresis;
        json["voltageDetectTime"] = this.voltageDetectTime;
        json["minGoodFrequency"] = this.minGoodFrequency;
        json["maxGoodFrequency"] = this.maxGoodFrequency;
        json["frequencyHysteresis"] = this.frequencyHysteresis;
        return json;
      }

      public static PowerQualitySettings Decode(LightJson.JsonObject json, Agent agent) {
        PowerQualitySettings inst = new PowerQualitySettings();
        inst.minMarginalVoltage = (double)json["minMarginalVoltage"];
        inst.minGoodVoltage = (double)json["minGoodVoltage"];
        inst.maxGoodVoltage = (double)json["maxGoodVoltage"];
        inst.maxMarginalVoltage = (double)json["maxMarginalVoltage"];
        inst.voltageHysteresis = (double)json["voltageHysteresis"];
        inst.voltageDetectTime = (int)json["voltageDetectTime"];
        inst.minGoodFrequency = (double)json["minGoodFrequency"];
        inst.maxGoodFrequency = (double)json["maxGoodFrequency"];
        inst.frequencyHysteresis = (double)json["frequencyHysteresis"];
        return inst;
      }

      public double minMarginalVoltage = 0.0;
      public double minGoodVoltage = 0.0;
      public double maxGoodVoltage = 0.0;
      public double maxMarginalVoltage = 0.0;
      public double voltageHysteresis = 0.0;
      public int voltageDetectTime = 0;
      public double minGoodFrequency = 0.0;
      public double maxGoodFrequency = 0.0;
      public double frequencyHysteresis = 0.0;
    }

    public class Settings : ICloneable {
      public object Clone() {
        Settings copy = new Settings();
        copy.name = this.name;
        copy.preferredSource = this.preferredSource;
        copy.autoRetransfer = this.autoRetransfer;
        copy.noAutoRetransferIfPhaseFault = this.noAutoRetransferIfPhaseFault;
        copy.autoRetransferWaitTime = this.autoRetransferWaitTime;
        copy.manualTransferEnabled = this.manualTransferEnabled;
        copy.powerQuality = this.powerQuality;
        return copy;
      }

      public LightJson.JsonObject Encode() {
        LightJson.JsonObject json = new LightJson.JsonObject();
        json["name"] = this.name;
        json["preferredSource"] = this.preferredSource;
        json["autoRetransfer"] = this.autoRetransfer;
        json["noAutoRetransferIfPhaseFault"] = this.noAutoRetransferIfPhaseFault;
        json["autoRetransferWaitTime"] = this.autoRetransferWaitTime;
        json["manualTransferEnabled"] = this.manualTransferEnabled;
        json["powerQuality"] = this.powerQuality.Encode();
        return json;
      }

      public static Settings Decode(LightJson.JsonObject json, Agent agent) {
        Settings inst = new Settings();
        inst.name = (string)json["name"];
        inst.preferredSource = (int)json["preferredSource"];
        inst.autoRetransfer = (bool)json["autoRetransfer"];
        inst.noAutoRetransferIfPhaseFault = (bool)json["noAutoRetransferIfPhaseFault"];
        inst.autoRetransferWaitTime = (int)json["autoRetransferWaitTime"];
        inst.manualTransferEnabled = (bool)json["manualTransferEnabled"];
        inst.powerQuality = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.PowerQualitySettings.Decode(json["powerQuality"], agent);
        return inst;
      }

      public string name = "";
      public int preferredSource = 0;
      public bool autoRetransfer = false;
      public bool noAutoRetransferIfPhaseFault = false;
      public int autoRetransferWaitTime = 0;
      public bool manualTransferEnabled = false;
      public Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.PowerQualitySettings powerQuality = new Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.PowerQualitySettings();
    }

    public class GetMetaDataResult {
      public Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.MetaData _ret_;
    }

    public GetMetaDataResult getMetaData() {
      JsonObject _parameters = null;
      var _result = RpcCall("getMetaData", _parameters);
      var _ret = new GetMetaDataResult();
      _ret._ret_ = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.MetaData.Decode(_result["_ret_"], agent);
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
            _ret._ret_ = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.MetaData.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetSensorsResult {
      public Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Sensors _ret_;
    }

    public GetSensorsResult getSensors() {
      JsonObject _parameters = null;
      var _result = RpcCall("getSensors", _parameters);
      var _ret = new GetSensorsResult();
      _ret._ret_ = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Sensors.Decode(_result["_ret_"], agent);
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
            _ret._ret_ = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Sensors.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetOutputSensorsResult {
      public Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.OutputSensors _ret_;
    }

    public GetOutputSensorsResult getOutputSensors() {
      JsonObject _parameters = null;
      var _result = RpcCall("getOutputSensors", _parameters);
      var _ret = new GetOutputSensorsResult();
      _ret._ret_ = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.OutputSensors.Decode(_result["_ret_"], agent);
      return _ret;
    }

    public AsyncRequest getOutputSensors(AsyncRpcResponse<GetOutputSensorsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getOutputSensors(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getOutputSensors(AsyncRpcResponse<GetOutputSensorsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getOutputSensors", _parameters,
        _result => {
          try {
            var _ret = new GetOutputSensorsResult();
            _ret._ret_ = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.OutputSensors.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetPolesResult {
      public System.Collections.Generic.IEnumerable<Com.Raritan.Idl.pdumodel.ThrowPole> _ret_;
    }

    public GetPolesResult getPoles() {
      JsonObject _parameters = null;
      var _result = RpcCall("getPoles", _parameters);
      var _ret = new GetPolesResult();
      _ret._ret_ = new System.Collections.Generic.List<Com.Raritan.Idl.pdumodel.ThrowPole>(_result["_ret_"].AsJsonArray.Select(
        _value => Com.Raritan.Idl.pdumodel.ThrowPole.Decode(_value, agent)));
      return _ret;
    }

    public AsyncRequest getPoles(AsyncRpcResponse<GetPolesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getPoles(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getPoles(AsyncRpcResponse<GetPolesResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getPoles", _parameters,
        _result => {
          try {
            var _ret = new GetPolesResult();
            _ret._ret_ = new System.Collections.Generic.List<Com.Raritan.Idl.pdumodel.ThrowPole>(_result["_ret_"].AsJsonArray.Select(
              _value => Com.Raritan.Idl.pdumodel.ThrowPole.Decode(_value, agent)));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetSettingsResult {
      public Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Settings _ret_;
    }

    public GetSettingsResult getSettings() {
      JsonObject _parameters = null;
      var _result = RpcCall("getSettings", _parameters);
      var _ret = new GetSettingsResult();
      _ret._ret_ = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Settings.Decode(_result["_ret_"], agent);
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
            _ret._ret_ = Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Settings.Decode(_result["_ret_"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class SetSettingsResult {
      public int _ret_;
    }

    public SetSettingsResult setSettings(Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Settings settings) {
      var _parameters = new LightJson.JsonObject();
      _parameters["settings"] = settings.Encode();

      var _result = RpcCall("setSettings", _parameters);
      var _ret = new SetSettingsResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest setSettings(Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Settings settings, AsyncRpcResponse<SetSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return setSettings(settings, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest setSettings(Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.Settings settings, AsyncRpcResponse<SetSettingsResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
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

    public class TransferToSourceResult {
      public int _ret_;
    }

    public TransferToSourceResult transferToSource(int source, bool faultOverride) {
      var _parameters = new LightJson.JsonObject();
      _parameters["source"] = source;
      _parameters["faultOverride"] = faultOverride;

      var _result = RpcCall("transferToSource", _parameters);
      var _ret = new TransferToSourceResult();
      _ret._ret_ = (int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest transferToSource(int source, bool faultOverride, AsyncRpcResponse<TransferToSourceResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return transferToSource(source, faultOverride, rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest transferToSource(int source, bool faultOverride, AsyncRpcResponse<TransferToSourceResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      var _parameters = new LightJson.JsonObject();
      try {
        _parameters["source"] = source;
        _parameters["faultOverride"] = faultOverride;
      } catch (Exception e) {
        if (fail != null) fail(e);
      }

      return RpcCall("transferToSource", _parameters,
        _result => {
          try {
            var _ret = new TransferToSourceResult();
            _ret._ret_ = (int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetLastTransferReasonResult {
      public Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.TransferReason _ret_;
    }

    public GetLastTransferReasonResult getLastTransferReason() {
      JsonObject _parameters = null;
      var _result = RpcCall("getLastTransferReason", _parameters);
      var _ret = new GetLastTransferReasonResult();
      _ret._ret_ = (Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.TransferReason)(int)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest getLastTransferReason(AsyncRpcResponse<GetLastTransferReasonResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getLastTransferReason(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getLastTransferReason(AsyncRpcResponse<GetLastTransferReasonResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getLastTransferReason", _parameters,
        _result => {
          try {
            var _ret = new GetLastTransferReasonResult();
            _ret._ret_ = (Com.Raritan.Idl.pdumodel.TransferSwitch_1_0_2.TransferReason)(int)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
