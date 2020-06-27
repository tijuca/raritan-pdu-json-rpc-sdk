// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using Com.Raritan.Idl;
using LightJson;
using LightJson.Serialization;

#pragma warning disable 1591

namespace Com.Raritan.JsonRpc {
    public class ValueObject {
        static public readonly TypeInfo typeInfo = new TypeInfo("idl.ValueObject:1.0.0", null);

        internal JsonObject origData;
        internal TypeInfo origType;
        internal Agent agent;

        public TypeInfo TypeInfo {
            get { return origType ?? ClassTypeInfo; }
        }

        protected virtual TypeInfo ClassTypeInfo {
            get { return typeInfo; }
        }

        public T InterpretAs<T>(TypeInfo type) where T : ValueObject {
            if (origData == null) {
                return null;
            }

            ValueObjectCodec voc = ValueObjectCodec.GetCodec(type);

            if (voc == null) {
                return null;
            }

            try {
                ValueObject result = voc.DecodeValObj(null, origData, agent);
                if (result != null) {
                    result.origData = origData;
                    result.origType = origType;
                    result.agent = agent;
                    return (T)result;
                }
            } catch (InvalidCastException) {
            } catch (JsonParseException) {
            }

            return null;
        }
    }
}
