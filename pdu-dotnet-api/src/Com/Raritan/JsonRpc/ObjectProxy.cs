// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Raritan.Idl;
using Com.Raritan.Util;
using LightJson;
using LightJson.Serialization;

#pragma warning disable 1591

namespace Com.Raritan.JsonRpc {
    public class ObjectProxy {
        /* dummy Register method to prevent compiler from complaining about "new" keyword */
        public static ObjectProxy StaticCast(ObjectProxy proxy) {
            return proxy;
        }

        protected readonly Agent agent;
        protected readonly string rid;
        protected TypeInfo typeInfo;

        public Agent Agent { get { return agent; } }
        public string Rid { get { return rid; } }

        private const string JSONRPC_KEY = "jsonrpc";
        private const string JSONRPC_VERSION = "2.0";
        private const string METHOD_KEY = "method";
        private const string PARAMS_KEY = "params";
        private const string ID_KEY = "id";
        private const string RESULT_KEY = "result";
        private const string ERROR_KEY = "error";
        private const string ERROR_CODE_KEY = "code";
        private const string ERROR_MESSAGE_KEY = "message";

        public ObjectProxy(Agent agent, string rid, TypeInfo typeInfo) {
            this.agent = agent;
            this.rid = rid;
            this.typeInfo = typeInfo;
        }

        public ObjectProxy(Agent agent, string rid) : this(agent, rid, null) {
        }

        public ObjectProxy(ObjectProxy proxy) {
            this.agent = proxy.agent;
            this.rid = proxy.rid;
            this.typeInfo = proxy.typeInfo;
        }

        public override int GetHashCode() {
            return rid.GetHashCode();
        }

        public override bool Equals(object other) {
            if (other is ObjectProxy) {
                ObjectProxy otherProxy = (ObjectProxy)other;
                return StringUtils.StringsEqual(rid, otherProxy.rid) && agent.Equals(otherProxy.agent);
            }

            return false;
        }

        public override string ToString() {
            return rid;
        }

        private class JsonRpcError {
            public int Code { get; private set; }
            public string Message { get; private set; }

            public JsonRpcError(JsonObject json) {
                Code = json[ERROR_CODE_KEY];
                Message = json[ERROR_MESSAGE_KEY];
            }
        }

        private class MethodResults {
            public string JsonRpcVersion { get; private set; }
            public int Id { get; private set; }
            public JsonObject Result { get; private set; }
            public JsonRpcError Error { get; private set; }

            public MethodResults(JsonObject json) {
                try {
                    JsonRpcVersion = json[JSONRPC_KEY];
                    Id = json[ID_KEY];
                    if (json.ContainsKey(ERROR_KEY)) {
                        Error = new JsonRpcError(json[ERROR_KEY]);
                    } else if (json.ContainsKey(RESULT_KEY)) {
                        Result = json[RESULT_KEY];
                    }
                } catch (JsonParseException) {
                }
            }

            public bool IsValid(int expectedId) {
                return JSONRPC_VERSION.Equals(JsonRpcVersion) && Id == expectedId;
            }
        }

        protected JsonObject RpcCall(string methodName, JsonObject parameters) {
            int id = agent.GetNextRequestId();

            JsonObject reqJson = new JsonObject();
            reqJson[JSONRPC_KEY] = JSONRPC_VERSION;
            reqJson[METHOD_KEY] = methodName;
            reqJson[PARAMS_KEY] = parameters;
            reqJson[ID_KEY] = id;

            string response = agent.PerformRequest(rid, reqJson.ToString());
            JsonObject rspJson = JsonValue.Parse(response);

            MethodResults results = new MethodResults(rspJson);
            if (!results.IsValid(id)) {
                throw new RpcFormatException();
            } else if (results.Error != null) {
                throw new RpcErrorException(results.Error.Code, results.Error.Message);
            }
            return results.Result;
        }

        protected AsyncRequest RpcCall(string methodName, JsonObject parameters, AsyncRpcResponse<JsonObject>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
            if (rpcCtrl.Bulk == RpcCtrl.BulkType.ALLOW) {
                int id = agent.GetNextRequestId();
                JsonObject reqJson = new JsonObject();

                try {
                    reqJson[JSONRPC_KEY] = JSONRPC_VERSION;
                    reqJson[METHOD_KEY] = methodName;
                    reqJson[PARAMS_KEY] = parameters;
                    reqJson[ID_KEY] = id;
                } catch (Exception e) {
                    if (fail != null) fail(e);
                }

                BulkRequestQueue queue = agent.DefaultBulkRequestQueue;
                return queue.EnqueueRequest(rid, reqJson, json => {
                    JsonObject rspJson = JsonValue.Parse(json);
                    MethodResults results = new MethodResults(rspJson);

                    if (!results.IsValid(id)) {
                        throw new RpcFormatException();
                    } else if (results.Error != null) {
                        throw new RpcErrorException(results.Error.Code, results.Error.Message);
                    }

                    return results.Result;
                }, rsp, fail);
            } else {
                string label = typeInfo + "::" + methodName;
                AsyncRequest request = new AsyncRequest(label);

                Action wrapperAction = () => {
                    try {
                        JsonObject json = RpcCall(methodName, parameters);
                        if (rsp != null) rsp(json);
                        request.Succeeded(json);
                    } catch (Exception ex) {
                        if (fail != null) fail(ex);
                        request.Failed(ex);
                    }
                };
                wrapperAction.BeginInvoke(iar => ((Action)iar.AsyncState).EndInvoke(iar), wrapperAction);

                return request;
            }
        }

        private TypeInfo DecodeTypeInfo(JsonObject json) {
            string id = json["id"];
            TypeInfo baseType = null;
            if (!json["base"].IsNull) {
                baseType = DecodeTypeInfo(json["base"]);
            }
            return new TypeInfo(id, baseType);
        }

        public TypeInfo GetTypeInfo() {
            if (typeInfo == null) {
                JsonObject result = RpcCall("getTypeInfo", null);
                typeInfo = DecodeTypeInfo(result["_ret_"]);
            }
            return typeInfo;
        }

        public AsyncRequest GetTypeInfo(AsyncRpcResponse<TypeInfo>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail) {
            return GetTypeInfo(rsp, fail, RpcCtrl.Default);
        }

        public AsyncRequest GetTypeInfo(AsyncRpcResponse<TypeInfo>.SuccessHandler rsp, AsyncRpcResponse.FailureHandler fail, RpcCtrl rpcCtrl) {
            return RpcCall("getTypeInfo", null, result => {
                try {
                    typeInfo = DecodeTypeInfo(result["_ret_"]);
                    if (rsp != null) rsp(typeInfo);
                } catch (Exception ex) {
                    if (fail != null) fail(ex);
                }
            }, fail, rpcCtrl);
        }

        private static readonly TypeInfo objectTypeInfo = new TypeInfo("idl.Object:1.0.0", null);

        public TypeInfo StaticTypeInfo {
            get { return typeInfo ?? objectTypeInfo; }
        }

        public static ObjectProxy Decode(JsonObject json, Agent agent) {
            return json == null
                ? null
                : new ObjectProxy(agent, json["rid"], new TypeInfo(json["type"], null));
        }

        public JsonValue Encode() {
            JsonObject json = new JsonObject();
            json["type"] = StaticTypeInfo.ToString();
            json["rid"] = Rid;
            return json;
        }

    }
}
