// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 1591

namespace Com.Raritan.Util {
    public static class DictionaryHelper {
        public static IDictionary<K, V> Create<K, V>(IEnumerable<KeyValuePair<K, V>> values) {
            IDictionary<K, V> ret = new Dictionary<K, V>();
            foreach (KeyValuePair<K, V> value in values) {
                ret.Add(value);
            }
            return ret;
        }
    }
}
