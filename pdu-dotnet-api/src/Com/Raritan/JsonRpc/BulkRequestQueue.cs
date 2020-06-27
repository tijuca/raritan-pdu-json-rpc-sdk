// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using Com.Raritan.Idl;
using Com.Raritan.Util;
using LightJson;

#pragma warning disable 1591

namespace Com.Raritan.JsonRpc {
    public class BulkRequestQueue : IDisposable {
        private const int SEND_DELAY_MS = 250;

        public delegate JsonObject JsonDecoder(string json);

        private struct SubRequest {
            public string rid;
            public JsonObject json;
            public JsonDecoder decoder;
            public AsyncRequest task;
        }

        private Timer mExecutor = null;
        private readonly Com.Raritan.Idl.bulkrpc.BulkRequest bulkRpc;

        private List<SubRequest> queued = new List<SubRequest>();

        // TODO: implement this!
        public BulkRequestQueue(Agent agent) {
            bulkRpc = new Com.Raritan.Idl.bulkrpc.BulkRequest(agent, "/bulk");
        }

        public void Dispose() {
            if (mExecutor != null) {
                mExecutor.Stop();
                mExecutor.Dispose();
                mExecutor = null;
            }
        }

        public AsyncRequest EnqueueRequest(string rid, JsonObject obj, JsonDecoder decoder, AsyncRpcResponse<JsonObject>.SuccessHandler response, AsyncRpcResponse.FailureHandler fail) {
            AsyncRequest asyncRequest = new AsyncRequest(rid);

            asyncRequest.Success += data => response((JsonObject)data);
            asyncRequest.Failure += e => fail(e);

            SubRequest req = new SubRequest();
            req.rid = rid;
            req.json = obj;
            req.decoder = decoder;
            req.task = asyncRequest;

            lock (this) {
                queued.Add(req);

                if (mExecutor == null) {
                    mExecutor = new Timer() { Interval = SEND_DELAY_MS, AutoReset = false };
                    mExecutor.Elapsed += TimerElapsed;
                    mExecutor.Start();
                }
            }

            return asyncRequest;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e) {
            List<SubRequest> sent;

            lock (this) {
                sent = queued;
                queued = new List<SubRequest>();
                mExecutor = null;
            }

            try {
                List<Com.Raritan.Idl.bulkrpc.Request> requests = new List<Com.Raritan.Idl.bulkrpc.Request>();
                foreach (SubRequest subreq in sent) {
                    Com.Raritan.Idl.bulkrpc.Request request = new Com.Raritan.Idl.bulkrpc.Request();
                    request.rid = subreq.rid;
                    request.json = subreq.json.ToString();
                    requests.Add(request);
                }

                Com.Raritan.Idl.bulkrpc.BulkRequest.PerformRequestResult result = bulkRpc.performRequest(requests);
                for (int i = 0; i < result.responses.Count(); i++) {
                    SubRequest subreq = sent[i];
                    Com.Raritan.Idl.bulkrpc.Response response = result.responses.ElementAt(i);
                    if (response.statcode == (int)HttpStatusCode.OK) {
                        try {
                            JsonObject resp = subreq.decoder(response.json);
                            subreq.task.Succeeded(resp);
                        } catch (Exception ex) {
                            subreq.task.Failed(ex);
                        }
                    } else {
                        subreq.task.Failed(new RpcRequestException(response.statcode));
                    }
                }

            } catch (Exception ex) {
                foreach (SubRequest subreq in sent) {
                    subreq.task.Failed(ex);
                }
            }
        }
    }
}
