// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Com.Raritan.Util;

#pragma warning disable 1591

namespace Com.Raritan.Idl {
    public class TypeInfo : IComparable<TypeInfo> {
        private readonly string typename;
        private readonly int major;
        private readonly int submajor;
        private readonly int minor;
        private readonly TypeInfo baseType;
        private List<TypeInfo> segments;

        public string TypeName { get { return typename; } }
        public TypeInfo Base { get { return baseType; } }

        public TypeInfo(string typename, int major, int submajor, int minor) {
            this.typename = typename;
            this.major = major;
            this.submajor = submajor;
            this.minor = minor;
            this.baseType = null;
        }

        public TypeInfo(string typeName, TypeInfo baseType) {
            string[] tokens = typeName.Split(':');
            typename = tokens[0];
            tokens = tokens[1].Split('.');
            major = int.Parse(tokens[0]);
            submajor = int.Parse(tokens[1]);
            minor = int.Parse(tokens[2]);
            this.baseType = baseType;
        }

        public override string ToString() {
            return typename + ":" + major + "." + submajor + "." + minor;
        }

        public override int GetHashCode() {
            int hash = 3;

            hash += 5 * typename.GetHashCode();
            hash += 7 * major;
            hash += 9 * submajor;
            hash += 11 * minor;

            return hash;
        }

        public override bool Equals(object other) {
            if (other is TypeInfo) {
                TypeInfo otherInfo = (TypeInfo)other;
                return StringUtils.StringsEqual(typename, otherInfo.typename) &&
                       major == otherInfo.major &&
                       submajor == otherInfo.submajor &&
                       minor == otherInfo.minor;
            }
            return false;
        }

        public int CompareTo(TypeInfo other) {
            int result = typename.CompareTo(other.typename);
            if (result != 0) {
                return result;
            }
            if (major != other.major) {
                return major < other.major ? -1 : 1;
            }
            if (submajor != other.submajor) {
                return submajor < other.submajor ? -1 : 1;
            }
            if (minor != other.minor) {
                return minor < other.minor ? -1 : 1;
            }
            return 0;
        }

        // returns the type name with all version numbers removed
        public string StrippedTypeName {
            get {
                EnsureSegments();
                StringBuilder s = new StringBuilder();
                foreach (TypeInfo segment in segments) {
                    if (s.Length > 0) s.Append('.');
                    s.Append(segment.typename);
                }
                return s.ToString();
            }
        }

        public bool IsCompatible(TypeInfo other) {
            EnsureSegments();
            other.EnsureSegments();
            if (segments.Count != other.segments.Count) {
                return false;
            }
            for (int i = 0; i < segments.Count; i++) {
                TypeInfo s = segments[i];
                TypeInfo os = other.segments[i];
                if (!StringUtils.StringsEqual(s.typename, os.typename) ||
                        s.major != os.major ||
                        s.submajor != os.submajor ||
                        s.minor < os.minor) {
                    return false;
                }
            }

            return true;
        }

        public bool IsCallCompatible(TypeInfo other) {
            EnsureSegments();
            other.EnsureSegments();
            if (segments.Count != other.segments.Count) {
                return false;
            }
            for (int i = 0; i < segments.Count; i++) {
                TypeInfo s = segments[i];
                TypeInfo os = other.segments[i];
                if (!StringUtils.StringsEqual(s.typename, os.typename) ||
                        s.major != os.major ||
                        s.minor < os.minor) {
                    return false;
                }
            }

            return true;
        }

        private void EnsureSegments() {
            if (segments != null) {
                return;
            }
            segments = new List<TypeInfo>();

            string[] parts = typename.Split('.');
            for (int i = 0; i < parts.Length - 1; i++) {
                string[] nameElems = parts[i].Split('_');
                int maj, submaj, min;

                if (nameElems.Length >= 4) {
                    try {
                        maj = int.Parse(nameElems[nameElems.Length - 3]);
                        submaj = int.Parse(nameElems[nameElems.Length - 2]);
                        min = int.Parse(nameElems[nameElems.Length - 1]);

                        Array.Resize(ref nameElems, nameElems.Length - 3);
                        string actualName = StringUtils.Join(nameElems, "_");
                        segments.Add(new TypeInfo(actualName, maj, submaj, min));
                        continue;
                    } catch (FormatException) {
                    }
                }
                segments.Add(new TypeInfo(parts[i], 1, 0, 0));
            }

            segments.Add(new TypeInfo(parts[parts.Length - 1], major, submajor, minor));
        }

    }
}
