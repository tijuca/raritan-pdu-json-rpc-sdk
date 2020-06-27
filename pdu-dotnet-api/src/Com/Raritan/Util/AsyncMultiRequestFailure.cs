// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 1591

namespace Com.Raritan.Util {
    public class AsyncMultiRequestFailure : Exception {
        public List<AsyncRequest> FailedRequests { get; private set; }
        public List<Exception> FailureExceptions { get; private set; }

        public AsyncMultiRequestFailure(List<AsyncRequest> failRequests) : base("Some asynchronous requests failed.") {
            FailedRequests = failRequests;
            FailureExceptions = new List<Exception>();

            foreach (AsyncRequest request in failRequests) {
                FailureExceptions.Add(request.FailureException);
            }
        }
    }
}
