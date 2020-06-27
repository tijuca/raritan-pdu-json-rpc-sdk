// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Com.Raritan.JsonRpc;
using System.Threading;

namespace examples {
    class BasicExamples {
        public void Run(string host, string username, string password) {
            Console.WriteLine("Connecting to " + host + " for basic examples ...");

            // Create an agent for performing JSON-RPC objects over HTTP(S)
            using (Agent agent = new Agent(host)) {
                agent.Username = "admin";
                agent.Password = "raritan";
                agent.ServerCertificateValidationCallback = Agent.UnsecureTrustAllCertificatesCallback;


                // Perform some display tests
                try {
                    var display = new Com.Raritan.Idl.test.Display_1_0_1(agent, "/test/Display");
                    display.enterTestMode();

                    var testStatus = display.getTestStatus();
                    Console.WriteLine("test status: " + testStatus._ret_);
                    Thread.Sleep(1000);
                    testStatus = display.getTestStatus();
                    Console.WriteLine("test status after 1 second: " + testStatus._ret_);
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine();


                // Query the metadata of the PDU.
                // Do it in a version-compatible way, so there is no need to use a version != 1.0
                // as long as we don't need anything which has been added later.
                try {
                    var pdu = new Com.Raritan.Idl.pdumodel.Pdu(agent, "/model/pdu/0");
                    var pduMetadata = pdu.getMetaData();
                    Console.WriteLine("PDU firmware revision: " + pduMetadata._ret_.fwRevision);
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine();


                // Query the reading of some well-known sensors.
                // Do it in a version-compatible way, so there is no need to use a version != 1.0
                // as long as we don't need anything which has been added later.
                string[] sensorsToQuery = {
                    "/model/pdu/0/inlet/0/voltage",
                    "/model/pdu/0/outlet/0/voltage",
                    "/model/pdu/0/inlet/0/pole/0/voltage",
                };
                foreach (string sensorToQuery in sensorsToQuery) {
                    try {
                        var sensor = new Com.Raritan.Idl.sensors.NumericSensor(agent, sensorToQuery);
                        var reading = sensor.getReading();
                        Console.WriteLine("Reading of sensor " + sensorToQuery + ": " + reading._ret_.value);
                    } catch {
                        Console.WriteLine("Unable to query sensor " + sensorToQuery);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
