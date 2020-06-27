// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using Com.Raritan.JsonRpc;

#pragma warning disable 1591

namespace Com.Raritan.Idl.bulkrpc {
    public class JsonObject : ICloneable {
        public LightJson.JsonObject Json { get; set; }

        public JsonObject() {
            Json = new LightJson.JsonObject();
        }

        public object Clone() {
            JsonObject copy = new JsonObject();
            LightJson.JsonValue val = Json;
            string s = val.ToString();
            LightJson.JsonValue newVal = LightJson.JsonValue.Parse(s);
            copy.Json = newVal.AsJsonObject;
            return copy;
        }

        public LightJson.JsonObject Encode() {
            LightJson.JsonObject json = new LightJson.JsonObject();
            json["json"] = this.Json;
            return json;
        }

        public static JsonObject Decode(LightJson.JsonObject json, Agent agent) {
            JsonObject inst = new JsonObject();
            inst.Json = json["json"];
            return inst;
        }
    }
}
