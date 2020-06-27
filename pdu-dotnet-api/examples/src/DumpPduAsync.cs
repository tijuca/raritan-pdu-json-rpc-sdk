// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Com.Raritan.Idl;
using Com.Raritan.Idl.pdumodel;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

namespace examples {
    class DumpPduAsync {
        private class InletCache {
            private readonly ObjectProxy proxy;
            private string label;
            private string plug;
            private int minVoltage;
            private int maxVoltage;
            private int currentRating;
            private string name;

            public InletCache(ObjectProxy proxy) {
                this.proxy = proxy;
            }

            private AsyncRequest UpdateV1(Inlet inlet) {
                AsyncMultiRequest getDataTask = new AsyncMultiRequest("get_inlet_data");

                getDataTask.Add(inlet.getMetaData(
                    result => {
                        label = result._ret_.label;
                        plug = result._ret_.plugType;
                        minVoltage = result._ret_.rating.minVoltage;
                        maxVoltage = result._ret_.rating.maxVoltage;
                        currentRating = result._ret_.rating.current;
                    }, null));

                getDataTask.Add(inlet.getSettings(
                    result => {
                        name = result._ret_.name;
                    }, null));

                return getDataTask.Started();
            }

            private AsyncRequest UpdateV2(Inlet_2_0_0 inlet) {
                AsyncMultiRequest getDataTask = new AsyncMultiRequest("get_inlet_data");

                getDataTask.Add(inlet.getMetaData(
                    result => {
                        label = result._ret_.label;
                        plug = result._ret_.plugType;
                        minVoltage = result._ret_.rating.minVoltage;
                        maxVoltage = result._ret_.rating.maxVoltage;
                        currentRating = result._ret_.rating.current;
                    }, null));

                getDataTask.Add(inlet.getSettings(
                    result => {
                        name = result._ret_.name;
                    }, null));

                return getDataTask.Started();
            }

            public AsyncRequest Update() {
                TypeInfo type = proxy.StaticTypeInfo;
                if (type.IsCallCompatible(Inlet.typeInfo)) {
                    return UpdateV1(Inlet.StaticCast(proxy));
                } else if (type.IsCallCompatible(Inlet_2_0_0.typeInfo)) {
                    return UpdateV2(Inlet_2_0_0.StaticCast(proxy));
                } else {
                    Console.WriteLine("ERROR: Unsupported Inlet interface version: " + type.ToString());
                    return new AsyncRequest("update_inlet").Failed(null);
                }
            }

            public void Dump() {
                Console.WriteLine("Inlet {0} ({1})", label, name);
                Console.WriteLine("  Type:   {0}", plug);
                Console.WriteLine("  Rating: {0}-{1} V, {2} A", minVoltage, maxVoltage, currentRating);
                Console.WriteLine();
            }
        }

        private class OutletCache {
            private readonly ObjectProxy proxy;
            private string label;
            private string receptacle;
            private int currentRating;
            private string name;

            public OutletCache(ObjectProxy proxy) {
                this.proxy = proxy;
            }

            private AsyncRequest UpdateV1(Outlet outlet) {
                AsyncMultiRequest getDataTask = new AsyncMultiRequest("get_outlet_data");

                getDataTask.Add(outlet.getMetaData(
                    result => {
                        label = result._ret_.label;
                        receptacle = result._ret_.receptacleType;
                        currentRating = result._ret_.rating.current;
                    }, null));

                getDataTask.Add(outlet.getSettings(
                    result => {
                        name = result._ret_.name;
                    }, null));

                return getDataTask.Started();
            }

            private AsyncRequest UpdateV2(Outlet_2_0_0 outlet) {
                AsyncMultiRequest getDataTask = new AsyncMultiRequest("get_outlet_data");

                getDataTask.Add(outlet.getMetaData(
                    result => {
                        label = result._ret_.label;
                        receptacle = result._ret_.receptacleType;
                        currentRating = result._ret_.rating.current;
                    }, null));

                getDataTask.Add(outlet.getSettings(
                    result => {
                        name = result._ret_.name;
                    }, null));

                return getDataTask.Started();
            }

            public AsyncRequest Update() {
                TypeInfo type = proxy.StaticTypeInfo;
                if (type.IsCallCompatible(Outlet.typeInfo)) {
                    return UpdateV1(Outlet.StaticCast(proxy));
                } else if (type.IsCallCompatible(Outlet_2_0_0.typeInfo)) {
                    return UpdateV2(Outlet_2_0_0.StaticCast(proxy));
                } else {
                    Console.WriteLine("ERROR: Unsupported Outlet interface version: " + type.ToString());
                    return new AsyncRequest("update_outlet").Failed(null);
                }
            }

            public void Dump() {
                Console.WriteLine("Outlet {0} ({1})", label, name);
                Console.WriteLine("  Type:   {0}", receptacle);
                Console.WriteLine("  Rating: {0} A", currentRating);
                Console.WriteLine();
            }
        }

        private class PduCache {
            private readonly ObjectProxy proxy;
            private string model;
            private string serialNumber;
            private string name;
            private List<InletCache> inlets = new List<InletCache>();
            private List<OutletCache> outlets = new List<OutletCache>();

            public PduCache(ObjectProxy proxy) {
                this.proxy = proxy;
            }

            private AsyncRequest UpdateV1(Pdu pdu) {
                AsyncMultiRequest getDataTask = new AsyncMultiRequest("get_pdu_v1_data");

                getDataTask.Add(pdu.getMetaData(result => {
                        model = result._ret_.nameplate.model;
                        serialNumber = result._ret_.nameplate.serialNumber;
                    }, null));

                getDataTask.Add(pdu.getSettings(result => {
                        name = result._ret_.name;
                    }, null));

                getDataTask.Add(pdu.getInlets(result => {
                        foreach (ObjectProxy o in result._ret_) {
                            inlets.Add(new InletCache(o));
                        }
                    }, null));

                getDataTask.Add(pdu.getOutlets(result => {
                        foreach (ObjectProxy o in result._ret_) {
                            outlets.Add(new OutletCache(o));
                        }
                    }, null));

                return getDataTask.Started();
            }

            private AsyncRequest UpdateV2(Pdu_2_0_0 pdu) {
                AsyncMultiRequest getDataTask = new AsyncMultiRequest("get_pdu_v2_data");

                getDataTask.Add(pdu.getMetaData(result => {
                        model = result._ret_.nameplate.model;
                        serialNumber = result._ret_.nameplate.serialNumber;
                    }, null));

                getDataTask.Add(pdu.getSettings(result => {
                        name = result._ret_.name;
                    }, null));

                getDataTask.Add(pdu.getInlets(result => {
                        foreach (ObjectProxy o in result._ret_) {
                            inlets.Add(new InletCache(o));
                        }
                    }, null));

                getDataTask.Add(pdu.getOutlets(result => {
                        foreach (ObjectProxy o in result._ret_) {
                            outlets.Add(new OutletCache(o));
                        }
                    }, null));

                return getDataTask.Started();
            }

            private AsyncRequest UpdateV3(Pdu_3_0_0 pdu) {
                AsyncMultiRequest getDataTask = new AsyncMultiRequest("get_pdu_v3_data");

                getDataTask.Add(pdu.getMetaData(result => {
                        model = result._ret_.nameplate.model;
                        serialNumber = result._ret_.nameplate.serialNumber;
                    }, null));

                getDataTask.Add(pdu.getSettings(result => {
                        name = result._ret_.name;
                    }, null));

                getDataTask.Add(pdu.getInlets(result => {
                        foreach (ObjectProxy o in result._ret_) {
                            inlets.Add(new InletCache((ObjectProxy)o));
                        }
                    }, null));

                getDataTask.Add(pdu.getOutlets(result => {
                        foreach (ObjectProxy o in result._ret_) {
                            outlets.Add(new OutletCache((ObjectProxy)o));
                        }
                    }, null));

                return getDataTask.Started();
            }

            private AsyncRequest UpdateV4(Pdu_4_0_0 pdu) {
                AsyncMultiRequest getDataTask = new AsyncMultiRequest("get_pdu_v4_data");

                getDataTask.Add(pdu.getMetaData(result => {
                        model = result._ret_.nameplate.model;
                        serialNumber = result._ret_.nameplate.serialNumber;
                    }, null));

                getDataTask.Add(pdu.getSettings(result => {
                        name = result._ret_.name;
                    }, null));

                getDataTask.Add(pdu.getInlets(result => {
                        foreach (ObjectProxy o in result._ret_) {
                            inlets.Add(new InletCache(o));
                        }
                    }, null));

                getDataTask.Add(pdu.getOutlets(result => {
                        foreach (ObjectProxy o in result._ret_) {
                            outlets.Add(new OutletCache(o));
                        }
                    }, null));

                return getDataTask.Started();
            }

            private AsyncRequest UpdateChildren() {
                AsyncMultiRequest updateChildrenTask = new AsyncMultiRequest("update_pdu_children");

                foreach (InletCache inlet in inlets) {
                    updateChildrenTask.Add(inlet.Update());
                }

                foreach (OutletCache outlet in outlets) {
                    updateChildrenTask.Add(outlet.Update());
                }

                return updateChildrenTask.Started();
            }

            public AsyncRequest Update() {
                AsyncRequest updateFullTask = new AsyncRequest("update_pdu_full");

                AsyncRequest getPduDataTask = null;

                // First step: query Pdu properties (metadata, settings, outlets and inlets);
                // this must be coded separately for each supported major interface version.
                TypeInfo type = proxy.StaticTypeInfo;
                if (type.IsCallCompatible(Pdu.typeInfo)) {
                    getPduDataTask = UpdateV1(Pdu.StaticCast(proxy));
                } else if (type.IsCallCompatible(Pdu_2_0_0.typeInfo)) {
                    getPduDataTask = UpdateV2(Pdu_2_0_0.StaticCast(proxy));
                } else if (type.IsCallCompatible(Pdu_3_0_0.typeInfo)) {
                    getPduDataTask = UpdateV3(Pdu_3_0_0.StaticCast(proxy));
                } else if (type.IsCallCompatible(Pdu_4_0_0.typeInfo)) {
                    getPduDataTask = UpdateV4(Pdu_4_0_0.StaticCast(proxy));
                } else {
                    Console.WriteLine("ERROR: Unsupported PDU interface version: " + type.ToString());
                    return updateFullTask.Failed(null);
                }

                getPduDataTask.Success += data1 => {
                    // Second step: ask the inlet and outlet cache objects to update themselves;
                    // those objects are responsible for dealing with different interface versions.
                    AsyncRequest updateChildrenTask = UpdateChildren();

                    updateChildrenTask.Success += data2 => {
                        // all is well; all data fields and child objects are now up-to-date
                        updateFullTask.Succeeded(null);
                    };

                    updateChildrenTask.Failure += e => {
                        Console.WriteLine("Failed to update Pdu children");
                        updateFullTask.Failed(e);
                    };
                };

                getPduDataTask.Failure += e => {
                    Console.WriteLine("Failed to update Pdu members");
                        updateFullTask.Failed(e);
                    };

                return updateFullTask.Started();
            }

            public void Dump() {
                Console.WriteLine("Model:            " + model);
                Console.WriteLine("Serial number:    " + serialNumber);
                Console.WriteLine("Device name:      " + name);
                Console.WriteLine();

                foreach (InletCache inlet in inlets) {
                    inlet.Dump();
                }

                foreach (OutletCache outlet in outlets) {
                    outlet.Dump();
                }
            }
        }

        
        
        public void Run(string host, string username, string password) {
            Console.WriteLine("Connecting to " + host + " for asynchronous API ...");
            try {
                // Create an agent for performing JSON-RPC objects over HTTP(S)
                using (Agent agent = new Agent(host)) {
                    agent.Username = "admin";
                    agent.Password = "raritan";
                    agent.ServerCertificateValidationCallback = Agent.UnsecureTrustAllCertificatesCallback;

                    ObjectProxy pduProxy = null;
                    try {
                        // Create a proxy for the root PDU object, synchronously query its type information
                        Console.WriteLine("Querying Pdu interface version ...");
                        pduProxy = new ObjectProxy(agent, "/model/pdu/0");
                        TypeInfo type = pduProxy.GetTypeInfo();
                        Console.WriteLine("Pdu interface type: " + type.ToString());
                    } catch (Exception e) {
                        Console.WriteLine(e.ToString());
                        System.Environment.Exit(1);
                    }

                    // Create a PDU cache object and request an asynchronous update
                    PduCache pdu = new PduCache(pduProxy);
                    AsyncRequest task = pdu.Update();

                    // When the PDU cache has been successfully updated, dump the PDU
                    task.Success += data => pdu.Dump();

                    // In case of a update failure, show an error message
                    task.Failure += ex => {
                        Console.WriteLine("Error: PDU cache update failed!");
                        Console.WriteLine(ex.ToString());
                    };

                    object locker = new object();

                    // In either case, wake up the main thread
                    task.Done += () => {
                        lock (locker) {
                            Monitor.Pulse(locker);
                        }
                    };

                    // wait for all background tasks to finish
                    while (!task.IsDone) {
                        lock (locker) {
                            Monitor.Wait(locker);
                        }
                    }

                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
