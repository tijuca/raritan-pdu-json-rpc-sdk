// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using LightJson;

namespace examples {
    class Program {
        private const string DEFAULT_URL = "https://10.0.42.2";
        private const string DEFAULT_USERNAME = "admin";
        private const string DEFAULT_PASSWORD = "raritan";

        static void Main(string[] args) {
            string host;
            if (args.Length >= 1) {
                host = args[0];
            } else {
                Console.WriteLine("Please enter PDU URL (default: " + DEFAULT_URL + "):");
                host = Console.ReadLine().Trim();
            }
            if (host == null || host.Length == 0) {
                host = DEFAULT_URL;
            }
            if (!host.Contains("://")) {
                host = "https://" + host;
            }

            new BasicExamples().Run(host, DEFAULT_USERNAME, DEFAULT_PASSWORD);
            Console.WriteLine();
            new DumpPduSync().Run(host, DEFAULT_USERNAME, DEFAULT_PASSWORD);
            Console.WriteLine();
            new DumpPduAsync().Run(host, DEFAULT_USERNAME, DEFAULT_PASSWORD);
            Console.WriteLine();

            Console.WriteLine("Please press any key to exit.");
            Console.ReadKey();
        }
    }
}
