// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Com.Raritan.Idl;
using LightJson;

#pragma warning disable 1591

namespace Com.Raritan.JsonRpc {
    public abstract class ValueObjectCodec {
        /* dummy Register method to prevent compiler from complaining about "new" keyword */
        public static void Register() {
        }

        /* methods to be implemented by value object codec classes */
        public abstract void EncodeValObj(JsonObject json, ValueObject vo);

        public abstract ValueObject DecodeValObj(ValueObject vo, JsonObject json, Agent agent);

        /*
         * static value object codec registry:
         * - key is stripped type name with all version numbers removed
         * - value is a map of fully versioned type infos to codecs
         */
        private static Dictionary<string, Dictionary<TypeInfo, ValueObjectCodec>> codecMap =
                new Dictionary<string, Dictionary<TypeInfo, ValueObjectCodec>>();

        public static void RegisterCodec(TypeInfo ti, ValueObjectCodec codec) {
            string strippedName = ti.StrippedTypeName;
            if (!codecMap.ContainsKey(strippedName)) {
                codecMap[strippedName] = new Dictionary<TypeInfo, ValueObjectCodec>();
            }
            Dictionary<TypeInfo, ValueObjectCodec> map = codecMap[strippedName];
            Debug.Assert(!map.ContainsKey(ti));
            map[ti] = codec;
        }

        private static Dictionary<TypeInfo, ValueObjectCodec> GetCandidateCodecs(TypeInfo type) {
            if (codecMap.Count == 0) {
                ValObjCodecRegistrar.Act();
            }
            return codecMap[type.StrippedTypeName];
        }

        internal static ValueObjectCodec GetCodec(TypeInfo type) {
            return GetCandidateCodecs(type)[type];
        }

        public static JsonObject Encode(ValueObject vo) {
            if (vo == null) return null;

            TypeInfo ti = vo.TypeInfo;
            ValueObjectCodec voc = GetCodec(ti);
            if (voc == null) {
                throw new ArgumentException("No codec for " + ti);
            }

            JsonObject value = new JsonObject();
            voc.EncodeValObj(value, vo);

            JsonObject json = new JsonObject();
            json.Add("type", ti.ToString());
            json.Add("value", value);
            return json;
        }

        public static ValueObject DecodeAs(JsonObject json, Agent agent, TypeInfo type) {
            if (json == null) {
                return null;
            }

            TypeInfo ti = new TypeInfo(json["type"], null);
            Dictionary<TypeInfo, ValueObjectCodec> map = GetCandidateCodecs(ti);
            TypeInfo bestTypeInfo = null;
            if (map != null) {
                foreach (TypeInfo entry in map.Keys) {
                    if (ti.IsCompatible(entry)) {
                        bestTypeInfo = entry;
                    }
                }
            }
            if (bestTypeInfo == null) {
                throw new ArgumentException("No codec for " + ti);
            }

            TypeInfo t = bestTypeInfo;
            while (t != null && !t.IsCompatible(type)) {
                t = t.Base;
            }
            if (t == null) {
                throw new ArgumentException("Codec found for " + bestTypeInfo + ", but incompatible with " + type);
            }

            map = codecMap[type.TypeName];
            ValueObjectCodec voc = map == null ? null : map[type];
            if (voc == null) {
                throw new ArgumentException("No codec for desired type " + type);
            }

            JsonObject data = json["value"];
            ValueObject result = voc.DecodeValObj(null, data, agent);
            if (result != null) {
                result.origData = data;
                result.origType = bestTypeInfo;
                result.agent = agent;
            }
            return result;
        }

    }
}
