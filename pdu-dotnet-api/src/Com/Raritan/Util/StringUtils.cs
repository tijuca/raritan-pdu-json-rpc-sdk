// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System.Text;

#pragma warning disable 1591

namespace Com.Raritan.Util {

    public static class StringUtils {
        public static bool StringsEqual(string s1, string s2) {
            if (s1 == null && s2 == null) {
                return true;
            }
            if (s1 == null && s2 != null) {
                return false;
            }
            if (s1 != null && s2 == null) {
                return false;
            }
            return s1.Equals(s2);
        }

        public static string Join(string[] s, string delimiter) {
            if (s == null) {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < s.Length; i++) {
                if (i > 0) {
                    builder.Append(delimiter);
                }
                builder.Append(s[i]);
            }
            return builder.ToString();
        }
    }

}
