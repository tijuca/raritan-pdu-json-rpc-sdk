// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using LightJson;

#pragma warning disable 1591

namespace Com.Raritan.JsonRpc.bulkrpc {
    public class JsonObject_Codec {
        public static Com.Raritan.Idl.bulkrpc.JsonObject Decode(JsonObject json, Agent agent) {
            Com.Raritan.Idl.bulkrpc.JsonObject inst = new Com.Raritan.Idl.bulkrpc.JsonObject();
            inst.Json = json["json"];
            return inst;
        }

        public static JsonObject Encode(Com.Raritan.Idl.bulkrpc.JsonObject inst) {
            JsonObject json = new JsonObject();
            json["json"] = inst.Json;
            return json;
        }
    }
}
