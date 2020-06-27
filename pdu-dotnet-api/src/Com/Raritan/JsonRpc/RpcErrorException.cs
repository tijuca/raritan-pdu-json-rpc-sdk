// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 1591

namespace Com.Raritan.JsonRpc {
    public class RpcErrorException : RpcException {
        private readonly int errorNumber;
        private readonly string errorString;

        public RpcErrorException(int errorNumber, string errorString) : base("" + errorNumber + " - " + errorString) {
            this.errorNumber = errorNumber;
            this.errorString = errorString;
        }

        public int ErrorNumber {
            get { return errorNumber; }
        }

        public string ErrorString {
            get { return errorString; }
        }
    }
}
