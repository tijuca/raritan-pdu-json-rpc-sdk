// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from WebcamChannel.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.webcam {
  public class Channel : ObjectProxy {

    static public readonly new TypeInfo typeInfo = new TypeInfo("webcam.Channel:1.0.0", null);

    public Channel(Agent agent, string rid, TypeInfo ti) : base(agent, rid, ti) {}
    public Channel(Agent agent, string rid) : this(agent, rid, typeInfo) {}

    public static new Channel StaticCast(ObjectProxy proxy) {
      return proxy == null ? null : new Channel(proxy.Agent, proxy.Rid, proxy.StaticTypeInfo);
    }

    public const int NO_ERROR = 0;

    public const int ERR_NOT_AVAILABLE = 2;

    public const int ERR_NO_SUCH_OBJECT = 3;

    public class GetClientTypeResult {
      public string _ret_;
    }

    public GetClientTypeResult getClientType() {
      JsonObject _parameters = null;
      var _result = RpcCall("getClientType", _parameters);
      var _ret = new GetClientTypeResult();
      _ret._ret_ = (string)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest getClientType(AsyncRpcResponse<GetClientTypeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getClientType(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getClientType(AsyncRpcResponse<GetClientTypeResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getClientType", _parameters,
        _result => {
          try {
            var _ret = new GetClientTypeResult();
            _ret._ret_ = (string)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class GetWebcamResult {
      public Com.Raritan.Idl.webcam.Webcam_2_0_0 _ret_;
    }

    public GetWebcamResult getWebcam() {
      JsonObject _parameters = null;
      var _result = RpcCall("getWebcam", _parameters);
      var _ret = new GetWebcamResult();
      _ret._ret_ = Com.Raritan.Idl.webcam.Webcam_2_0_0.StaticCast(ObjectProxy.Decode(_result["_ret_"], agent));
      return _ret;
    }

    public AsyncRequest getWebcam(AsyncRpcResponse<GetWebcamResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return getWebcam(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest getWebcam(AsyncRpcResponse<GetWebcamResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("getWebcam", _parameters,
        _result => {
          try {
            var _ret = new GetWebcamResult();
            _ret._ret_ = Com.Raritan.Idl.webcam.Webcam_2_0_0.StaticCast(ObjectProxy.Decode(_result["_ret_"], agent));
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class IsAvailableResult {
      public bool _ret_;
    }

    public IsAvailableResult isAvailable() {
      JsonObject _parameters = null;
      var _result = RpcCall("isAvailable", _parameters);
      var _ret = new IsAvailableResult();
      _ret._ret_ = (bool)_result["_ret_"];
      return _ret;
    }

    public AsyncRequest isAvailable(AsyncRpcResponse<IsAvailableResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return isAvailable(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest isAvailable(AsyncRpcResponse<IsAvailableResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("isAvailable", _parameters,
        _result => {
          try {
            var _ret = new IsAvailableResult();
            _ret._ret_ = (bool)_result["_ret_"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class ReleaseResult {
    }

    public ReleaseResult release() {
      JsonObject _parameters = null;
      var _result = RpcCall("release", _parameters);
      var _ret = new ReleaseResult();
      return _ret;
    }

    public AsyncRequest release(AsyncRpcResponse<ReleaseResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return release(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest release(AsyncRpcResponse<ReleaseResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("release", _parameters,
        _result => {
          try {
            var _ret = new ReleaseResult();
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class CaptureImageResult {
      public int _ret_;
      public Com.Raritan.Idl.webcam.Image_2_0_0 image;
    }

    public CaptureImageResult captureImage() {
      JsonObject _parameters = null;
      var _result = RpcCall("captureImage", _parameters);
      var _ret = new CaptureImageResult();
      _ret._ret_ = (int)_result["_ret_"];
      _ret.image = Com.Raritan.Idl.webcam.Image_2_0_0.Decode(_result["image"], agent);
      return _ret;
    }

    public AsyncRequest captureImage(AsyncRpcResponse<CaptureImageResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return captureImage(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest captureImage(AsyncRpcResponse<CaptureImageResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("captureImage", _parameters,
        _result => {
          try {
            var _ret = new CaptureImageResult();
            _ret._ret_ = (int)_result["_ret_"];
            _ret.image = Com.Raritan.Idl.webcam.Image_2_0_0.Decode(_result["image"], agent);
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

    public class TriggerCaptureResult {
      public int _ret_;
      public string captureToken;
    }

    public TriggerCaptureResult triggerCapture() {
      JsonObject _parameters = null;
      var _result = RpcCall("triggerCapture", _parameters);
      var _ret = new TriggerCaptureResult();
      _ret._ret_ = (int)_result["_ret_"];
      _ret.captureToken = (string)_result["captureToken"];
      return _ret;
    }

    public AsyncRequest triggerCapture(AsyncRpcResponse<TriggerCaptureResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
      return triggerCapture(rsp, fail, RpcCtrl.Default);
    }

    public AsyncRequest triggerCapture(AsyncRpcResponse<TriggerCaptureResult>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
      JsonObject _parameters = null;
      return RpcCall("triggerCapture", _parameters,
        _result => {
          try {
            var _ret = new TriggerCaptureResult();
            _ret._ret_ = (int)_result["_ret_"];
            _ret.captureToken = (string)_result["captureToken"];
            rsp(_ret);
          } catch (Exception e) {
            if (fail != null) fail(e);
          }
        }, fail, rpcCtrl);
    }

  }
}
