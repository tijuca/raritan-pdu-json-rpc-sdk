// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 1591

namespace Com.Raritan.Idl {
    public static class AsyncRpcResponse {
        public delegate void FailureHandler(Exception ex);
    }

    public static class AsyncRpcResponse<T> {
        public delegate void SuccessHandler(T result);
    }
}
