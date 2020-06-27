// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using Com.Raritan.Idl;
using Com.Raritan.Util;

#pragma warning disable 1591

namespace Com.Raritan.JsonRpc {
    public class Agent : IDisposable {
        // An unsecure default certificate validation callback
        public static readonly System.Net.Security.RemoteCertificateValidationCallback UnsecureTrustAllCertificatesCallback = delegate { return true; };

        private readonly string remoteUrl = null;

        private int requestId = 0;

        private BulkRequestQueue defaultBulkRequestQueue = null;

        private const string HEADER_CLIENT_TYPE = "X-Client-Type";
        private const string HEADER_SESSION_TOKEN = "X-SessionToken";

        public string RemoteUrl { get { return remoteUrl; } }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Token { get; set; }
        public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }

        public Agent(string remoteUrl) {
            ServerCertificateValidationCallback = null;
            Token = null;
            Password = null;
            Username = null;
            this.remoteUrl = remoteUrl;
        }

        public int GetNextRequestId() {
            return requestId++;
        }

        public string PerformRequest(string rid, string request) {
            // set a certificate validation callback
            if (ServerCertificateValidationCallback != null) {
                ServicePointManager.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
            }

            // enable all SSL/TLS versions
            SecurityProtocolType currentTlsVersions = ServicePointManager.SecurityProtocol;
            SecurityProtocolType tlsVersions = 0;
            foreach (SecurityProtocolType protocol in Enum.GetValues(typeof(SecurityProtocolType))) {
                tlsVersions |= protocol;
            }
            ServicePointManager.SecurityProtocol = tlsVersions;

            // do the web request
            try {
                Uri uri = new Uri(remoteUrl + "/" + rid);
                HttpWebRequest httpRequest = (HttpWebRequest) WebRequest.Create(uri);

                byte[] data = Encoding.ASCII.GetBytes(request);

                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.ContentLength = data.Length;
                httpRequest.Headers.Add(HEADER_CLIENT_TYPE, "Java RPC");

                if (Token != null) {
                    httpRequest.Headers.Add(HEADER_SESSION_TOKEN, Token);
                } else if (Username != null && Password != null) {
                    string userPass = Username + ":" + Password;
                    string basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userPass));
                    httpRequest.Headers.Add("Authorization", "Basic " + basicAuth);
                }

                using (var stream = httpRequest.GetRequestStream()) {
                    stream.Write(data, 0, data.Length);
                }

                using (var response = (HttpWebResponse) httpRequest.GetResponse()) {
                    HttpStatusCode status = response.StatusCode;
                    if (status != HttpStatusCode.OK) {
                        throw new RpcRequestException((int) status);
                    }

                    using (var stream = new StreamReader(response.GetResponseStream(), new System.Text.UTF8Encoding())) {
                        return stream.ReadToEnd();
                    }
                }
            } finally {
                // reset the supported TLS versions
                ServicePointManager.SecurityProtocol = currentTlsVersions;
                if (ServerCertificateValidationCallback != null) {
                    ServicePointManager.ServerCertificateValidationCallback -= ServerCertificateValidationCallback;
                }
            }
        }

        public AsyncRequest PerformRequest(string rid, string request, AsyncRpcResponse<string>.SuccessHandler succeeded, AsyncRpcResponse.FailureHandler failed) {
            AsyncRequest req = new AsyncRequest(rid);

            Action wrapperAction = () => {
                try {
                    string response = PerformRequest(rid, request);
                    if (succeeded != null) succeeded(response);
                    req.Succeeded(response);
                } catch(Exception ex) {
                    if (failed != null) failed(ex);
                    req.Failed(ex);
                }
            };
            // call EndInvoke in a callback handler
            wrapperAction.BeginInvoke(iar => ((Action)iar.AsyncState).EndInvoke(iar), wrapperAction);

            return req;
        }

        public void Dispose() {
            if (defaultBulkRequestQueue != null) {
                defaultBulkRequestQueue.Dispose();
                defaultBulkRequestQueue = null;
            }
        }

        public BulkRequestQueue DefaultBulkRequestQueue {
            get { return defaultBulkRequestQueue ?? (defaultBulkRequestQueue = new BulkRequestQueue(this)); }
        }

        public override int GetHashCode() {
            return remoteUrl.GetHashCode() + Username.GetHashCode();
        }

        public override bool Equals(object other) {
            if (other is Agent) {
                Agent otherAgent = (Agent)other;

                if (!StringUtils.StringsEqual(remoteUrl, otherAgent.remoteUrl)) {
                    return false;
                }
                if (!StringUtils.StringsEqual(Username, otherAgent.Username)) {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
