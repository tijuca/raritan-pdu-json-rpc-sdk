// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Com.Raritan.Idl;
using Com.Raritan.Idl.pdumodel;
using Com.Raritan.Idl.sensors;
using Com.Raritan.JsonRpc;
using System.Collections;
using System.Globalization;

namespace examples {
    class DumpPduSync {
        private Com.Raritan.Idl.session.SessionManager sessmanV1;
        private Com.Raritan.Idl.session.SessionManager_2_0_0 sessmanV2;

        private string CreateSession(Agent agent) {
            ObjectProxy sessmanProxy = new ObjectProxy(agent, "/session");
            TypeInfo typeInfo = sessmanProxy.GetTypeInfo();

            if (typeInfo.IsCallCompatible(Com.Raritan.Idl.session.SessionManager.typeInfo)) {
                // SessionManager_1_0_0: Authentication token is part of the Session
                //                       structure returned by newSession
                sessmanV1 = Com.Raritan.Idl.session.SessionManager.StaticCast(sessmanProxy);
                Com.Raritan.Idl.session.SessionManager.NewSessionResult result = sessmanV1.newSession();
                if (result._ret_ == 0) {
                    return result.session.token;
                } else {
                    Console.WriteLine("ERROR: Failed to create a session with SessionManager_1_0_0");
                }
            } else if (typeInfo.IsCallCompatible(Com.Raritan.Idl.session.SessionManager_2_0_0.typeInfo)) {
                // SessionManager_2_0_0: Authentication token is not part of the Session
                //                       structure; it is returned as an out-parameter
                sessmanV2 = Com.Raritan.Idl.session.SessionManager_2_0_0.StaticCast(sessmanProxy);
                Com.Raritan.Idl.session.SessionManager_2_0_0.NewSessionResult result = sessmanV2.newSession();
                if (result._ret_ == 0) {
                    return result.token;
                } else {
                    Console.WriteLine("ERROR: Failed to create a session with SessionManager_2_0_0");
                }
            } else {
                Console.WriteLine("Unsupported version of SessionManager interface: " + typeInfo);
            }
            return null;
        }

        private void CloseSession() {
            if (sessmanV1 != null) {
                sessmanV1.closeCurrentSession(Com.Raritan.Idl.session.SessionManager.CloseReason.CLOSE_REASON_LOGOUT);
            } else if (sessmanV2 != null) {
                sessmanV2.closeCurrentSession(Com.Raritan.Idl.session.SessionManager_2_0_0.CloseReason.CLOSE_REASON_LOGOUT);
            }
        }

        private static string FormatSensorReading(double reading, int decdigits, string unit) {
            string fmt = "F" + decdigits;
            string result = reading.ToString(fmt, CultureInfo.InvariantCulture);
            if (unit != null && unit.Length > 0) {
                result += " " + unit;
            }
            return result;
        }

        private static void DumpNumericSensorV1(NumericSensor nsensor, string label) {
            NumericSensor.MetaData metadata = nsensor.getMetaData()._ret_;
            NumericSensor.Reading reading = nsensor.getReading()._ret_;

            string unit;
            switch (metadata.type.unit) {
                case Sensor.Unit.NONE:          unit = ""; break;
                case Sensor.Unit.VOLT:          unit = "V"; break;
                case Sensor.Unit.AMPERE:        unit = "A"; break;
                case Sensor.Unit.WATT:          unit = "W"; break;
                case Sensor.Unit.VOLT_AMP:      unit = "VA"; break;
                case Sensor.Unit.WATT_HOUR:     unit = "Wh"; break;
                case Sensor.Unit.VOLT_AMP_HOUR: unit = "VAh"; break;
                case Sensor.Unit.HZ:            unit = "Hz"; break;
                default:                        unit = "?"; break;
            }

            if (reading.valid) {
                Console.WriteLine(label + FormatSensorReading(reading.value, metadata.decdigits, unit));
            } else if (reading.available) {
                Console.WriteLine(label + "no reading");
            } else {
                Console.WriteLine(label + "unavailable");
            }
        }

        private static void DumpNumericSensorV3(NumericSensor_3_0_0 nsensor, string label) {
            // The main difference between NumericSensor interface versions 1 and 3
            // was the addition of new sensor types and units. Therefore this code is
            // identical to dumpNumericSensorV1 except for the use of more recent
            // data types.

            NumericSensor_3_0_0.MetaData metadata = nsensor.getMetaData()._ret_;
            NumericSensor_3_0_0.Reading reading = nsensor.getReading()._ret_;

            string unit;
            switch (metadata.type.unit) {
                case Sensor_3_0_0.Unit.NONE:          unit = ""; break;
                case Sensor_3_0_0.Unit.VOLT:          unit = "V"; break;
                case Sensor_3_0_0.Unit.AMPERE:        unit = "A"; break;
                case Sensor_3_0_0.Unit.WATT:          unit = "W"; break;
                case Sensor_3_0_0.Unit.VOLT_AMP:      unit = "VA"; break;
                case Sensor_3_0_0.Unit.WATT_HOUR:     unit = "Wh"; break;
                case Sensor_3_0_0.Unit.VOLT_AMP_HOUR: unit = "VAh"; break;
                case Sensor_3_0_0.Unit.HZ:            unit = "Hz"; break;
                default:                              unit = "?"; break;
            }

            if (reading.valid) {
                Console.WriteLine(label + FormatSensorReading(reading.value, metadata.decdigits, unit));
            } else if (reading.available) {
                Console.WriteLine(label + "no reading");
            } else {
                Console.WriteLine(label + "unavailable");
            }
        }

        private static void DumpNumericSensorV4(NumericSensor_4_0_0 nsensor, string label) {
            // Starting with NumericSensor_4_0_0 the existing ReadingType, Type and Unit
            // enumerations have been replaced with plain integers so that new sensor
            // types can now be implemented without formally breaking the interface
            // and incrementing the major version.

            NumericSensor_4_0_0.MetaData metadata = nsensor.getMetaData()._ret_;
            NumericSensor_4_0_0.Reading reading = nsensor.getReading()._ret_;

            string unit;
            switch (metadata.type.unit) {
                case Sensor_4_0_0.NONE:          unit = ""; break;
                case Sensor_4_0_0.VOLT:          unit = "V"; break;
                case Sensor_4_0_0.AMPERE:        unit = "A"; break;
                case Sensor_4_0_0.WATT:          unit = "W"; break;
                case Sensor_4_0_0.VOLT_AMP:      unit = "VA"; break;
                case Sensor_4_0_0.WATT_HOUR:     unit = "Wh"; break;
                case Sensor_4_0_0.VOLT_AMP_HOUR: unit = "VAh"; break;
                case Sensor_4_0_0.HZ:            unit = "Hz"; break;
                default:                         unit = "?"; break;
            }

            if (reading.valid) {
                Console.WriteLine(label + FormatSensorReading(reading.value, metadata.decdigits, unit));
            } else if (reading.available) {
                Console.WriteLine(label + "no reading");
            } else {
                Console.WriteLine(label + "unavailable");
            }
        }

        private static void DumpNumericSensor(ObjectProxy proxy, string label) {
            if (proxy == null) {
                return;
            }

            // There are four incompatible versions of the NumericSensor
            // interface. NumericSensor_2_x_y was only used in EMX, so we
            // don't need to support it here.

            TypeInfo typeInfo = proxy.GetTypeInfo();
            if (typeInfo.IsCallCompatible(NumericSensor.typeInfo)) {
                DumpNumericSensorV1(NumericSensor.StaticCast(proxy), label);
            } else if (typeInfo.IsCallCompatible(NumericSensor_3_0_0.typeInfo)) {
                DumpNumericSensorV3(NumericSensor_3_0_0.StaticCast(proxy), label);
            } else if (typeInfo.IsCallCompatible(NumericSensor_4_0_0.typeInfo)) {
                DumpNumericSensorV4(NumericSensor_4_0_0.StaticCast(proxy), label);
            } else {
                Console.WriteLine("Unsupported version of NumericSensor interface: " + typeInfo);
            }
        }

        private static void DumpInletV1(Inlet inlet) {
            Inlet.MetaData metadata = inlet.getMetaData()._ret_;
            Inlet.Settings settings = inlet.getSettings()._ret_;
            Inlet.Sensors sensors = inlet.getSensors()._ret_;

            String name = settings.name;
            if (string.IsNullOrEmpty(name)) name = "no name";
            Console.WriteLine("Inlet {0} ({1}):", metadata.label, name);

            Console.WriteLine("  Plug type:      " + metadata.plugType);
            DumpNumericSensor(sensors.voltage,       "  Voltage:        ");
            DumpNumericSensor(sensors.current,       "  Current:        ");
            DumpNumericSensor(sensors.activePower,   "  Active Power:   ");
            DumpNumericSensor(sensors.apparentPower, "  Apparent Power: ");
            DumpNumericSensor(sensors.powerFactor,   "  Power Factor:   ");
            Console.WriteLine();
        }

        private static void DumpInletV2(Inlet_2_0_0 inlet) {
            Inlet_2_0_0.MetaData metadata = inlet.getMetaData()._ret_;
            Inlet_2_0_0.Settings settings = inlet.getSettings()._ret_;
            Inlet_2_0_0.Sensors sensors = inlet.getSensors()._ret_;

            String name = settings.name;
            if (string.IsNullOrEmpty(name)) name = "no name";
            Console.WriteLine("Inlet {0} ({1}):", metadata.label, name);

            Console.WriteLine("  Plug type:      " + metadata.plugType);
            DumpNumericSensor(sensors.voltage,       "  Voltage:        ");
            DumpNumericSensor(sensors.current,       "  Current:        ");
            DumpNumericSensor(sensors.activePower,   "  Active Power:   ");
            DumpNumericSensor(sensors.apparentPower, "  Apparent Power: ");
            DumpNumericSensor(sensors.powerFactor,   "  Power Factor:   ");
            Console.WriteLine();
        }

        private static void DumpInlet(ObjectProxy proxy) {
            TypeInfo typeInfo = proxy.GetTypeInfo();
            if (typeInfo.IsCallCompatible(Inlet.typeInfo)) {
                DumpInletV1(Inlet.StaticCast(proxy));
            } else if (typeInfo.IsCallCompatible(Inlet_2_0_0.typeInfo)){
                DumpInletV2(Inlet_2_0_0.StaticCast(proxy));
            } else {
                Console.WriteLine("Unsupported version of Inlet interface: " + typeInfo);
            }
        }

        private static void DumpOutletV1(Outlet outlet) {
            Outlet.MetaData metadata = outlet.getMetaData()._ret_;
            Outlet.Settings settings = outlet.getSettings()._ret_;
            Outlet.Sensors sensors = outlet.getSensors()._ret_;

            string name = settings.name;
            if (string.IsNullOrEmpty(name)) name = "no name";
            Console.WriteLine("Outlet {0} ({1}):", metadata.label, name);

            Console.WriteLine("  Receptacle type: " + metadata.receptacleType);
            DumpNumericSensor(sensors.voltage,       "  Voltage:         ");
            DumpNumericSensor(sensors.current,       "  Current:         ");
            DumpNumericSensor(sensors.activePower,   "  Active Power:    ");
            DumpNumericSensor(sensors.apparentPower, "  Apparent Power:  ");
            DumpNumericSensor(sensors.powerFactor,   "  Power Factor:    ");
            Console.WriteLine();
        }

        private static void DumpOutletV2(Outlet_2_0_0 outlet) {
            Outlet_2_0_0.MetaData metadata = outlet.getMetaData()._ret_;
            Outlet_2_0_0.Settings settings = outlet.getSettings()._ret_;
            Outlet_2_0_0.Sensors sensors = outlet.getSensors()._ret_;

            string name = settings.name;
            if (string.IsNullOrEmpty(name)) name = "no name";
            Console.WriteLine("Outlet {0} ({1}):", metadata.label, name);

            Console.WriteLine("  Receptacle type: " + metadata.receptacleType);
            DumpNumericSensor(sensors.voltage,       "  Voltage:         ");
            DumpNumericSensor(sensors.current,       "  Current:         ");
            DumpNumericSensor(sensors.activePower,   "  Active Power:    ");
            DumpNumericSensor(sensors.apparentPower, "  Apparent Power:  ");
            DumpNumericSensor(sensors.powerFactor,   "  Power Factor:    ");
            Console.WriteLine();
        }

        private static void DumpOutlet(ObjectProxy proxy) {
            TypeInfo typeInfo = proxy.GetTypeInfo();
            if (typeInfo.IsCallCompatible(Outlet.typeInfo)) {
                DumpOutletV1(Outlet.StaticCast(proxy));
            } else if (typeInfo.IsCallCompatible(Outlet_2_0_0.typeInfo)){
                DumpOutletV2(Outlet_2_0_0.StaticCast(proxy));
            } else {
                Console.WriteLine("Unsupported version of Outlet interface: " + typeInfo);
            }
        }

        private static void DumpPduV1(Pdu pdu) {
            Pdu.MetaData metadata = pdu.getMetaData()._ret_;
            Console.WriteLine("Model:            " + metadata.nameplate.model);
            Console.WriteLine("Serial number:    "  + metadata.nameplate.serialNumber);
            Console.WriteLine("Rating:           {0}, {1}, {2}, {3}",
                    metadata.nameplate.rating.voltage, metadata.nameplate.rating.frequency,
                    metadata.nameplate.rating.current, metadata.nameplate.rating.power);
            Console.WriteLine("Firmware version: " + metadata.fwRevision);

            Pdu.Settings settings = pdu.getSettings()._ret_;
            Console.WriteLine("PDU name:         " + settings.name);
            Console.WriteLine();

            var inlets = pdu.getInlets()._ret_;
            foreach (var inlet in inlets) {
                DumpInlet(inlet);
            }

            var outlets = pdu.getOutlets()._ret_;
            foreach (var outlet in outlets) {
                DumpOutlet(outlet);
            }
        }

        private static void DumpPduV2(Pdu_2_0_0 pdu) {
            Pdu_2_0_0.MetaData metadata = pdu.getMetaData()._ret_;
            Console.WriteLine("Model:            " + metadata.nameplate.model);
            Console.WriteLine("Serial number:    "  + metadata.nameplate.serialNumber);
            Console.WriteLine("Rating:           {0}, {1}, {2}, {3}",
                    metadata.nameplate.rating.voltage, metadata.nameplate.rating.frequency,
                    metadata.nameplate.rating.current, metadata.nameplate.rating.power);
            Console.WriteLine("Firmware version: " + metadata.fwRevision);

            Pdu_2_0_0.Settings settings = pdu.getSettings()._ret_;
            Console.WriteLine("PDU name:         " + settings.name);
            Console.WriteLine();

            // We only checked whether the proxy object was call-compatible with
            // the Pdu_2_0_0 interface, so we can't rely on the version numbers
            // of the returned inlets and outlets

            var inlets = pdu.getInlets()._ret_;
            foreach (var inlet in inlets) {
                DumpInlet(inlet);
            }

            var outlets = pdu.getOutlets()._ret_;
            foreach (var outlet in outlets) {
                DumpOutlet(outlet);
            }
        }

        private static void DumpPduV3(Pdu_3_0_0 pdu) {
            Pdu_3_0_0.MetaData metadata = pdu.getMetaData()._ret_;
            Console.WriteLine("Model:            " + metadata.nameplate.model);
            Console.WriteLine("Serial number:    "  + metadata.nameplate.serialNumber);
            Console.WriteLine("Rating:           {0}, {1}, {2}, {3}",
                    metadata.nameplate.rating.voltage, metadata.nameplate.rating.frequency,
                    metadata.nameplate.rating.current, metadata.nameplate.rating.power);
            Console.WriteLine("Firmware version: " + metadata.fwRevision);

            Pdu_3_0_0.Settings settings = pdu.getSettings()._ret_;
            Console.WriteLine("PDU name:         " + settings.name);
            Console.WriteLine();

            // We only checked whether the proxy object was call-compatible with
            // the Pdu_3_0_0 interface, so we can't rely on the version numbers
            // of the returned inlets and outlets

            var inlets = pdu.getInlets()._ret_;
            foreach (var inlet in inlets) {
                DumpInlet(inlet);
            }

            var outlets = pdu.getOutlets()._ret_;
            foreach (var outlet in outlets) {
                DumpOutlet(outlet);
            }
        }

        private static void DumpPduV4(Pdu_4_0_0 pdu) {
            Pdu_4_0_0.MetaData metadata = pdu.getMetaData()._ret_;
            Console.WriteLine("Model:            " + metadata.nameplate.model);
            Console.WriteLine("Serial number:    "  + metadata.nameplate.serialNumber);
            Console.WriteLine("Rating:           {0}, {1}, {2}, {3}",
                    metadata.nameplate.rating.voltage, metadata.nameplate.rating.frequency,
                    metadata.nameplate.rating.current, metadata.nameplate.rating.power);
            Console.WriteLine("Firmware version: " + metadata.fwRevision);

            Pdu_4_0_0.Settings settings = pdu.getSettings()._ret_;
            Console.WriteLine("PDU name:         " + settings.name);
            Console.WriteLine();

            // We only checked whether the proxy object was call-compatible with
            // the Pdu_4_0_0 interface, so we can't rely on the version numbers
            // of the returned inlets and outlets

            var inlets = pdu.getInlets()._ret_;
            foreach (var inlet in inlets) {
                DumpInlet(inlet);
            }

            var outlets = pdu.getOutlets()._ret_;
            foreach (var outlet in outlets) {
                DumpOutlet(outlet);
            }
        }

        private void DumpPdu(ObjectProxy proxy) {
            // Check which Pdu object we have -- there
            // have been incompatible changes between those versions.

            TypeInfo typeInfo = proxy.GetTypeInfo();
            if (typeInfo.IsCallCompatible(Pdu.typeInfo)) {
                DumpPduV1(Pdu.StaticCast(proxy));
            } else if (typeInfo.IsCallCompatible(Pdu_2_0_0.typeInfo)) {
                DumpPduV2(Pdu_2_0_0.StaticCast(proxy));
            } else if (typeInfo.IsCallCompatible(Pdu_3_0_0.typeInfo)) {
                DumpPduV3(Pdu_3_0_0.StaticCast(proxy));
            } else if (typeInfo.IsCallCompatible(Pdu_4_0_0.typeInfo)) {
                DumpPduV4(Pdu_4_0_0.StaticCast(proxy));
            } else {
                Console.WriteLine("Unsupported version of Pdu interface: " + typeInfo);
            }
        }

        public void Run(string host, string username, string password) {
            Console.WriteLine("Connecting to " + host + " for synchronous API ...");
            try {
                // Create an agent for performing JSON-RPC objects over HTTP(S)
                using (Agent agent = new Agent(host)) {
                    agent.Username = "admin";
                    agent.Password = "raritan";
                    agent.ServerCertificateValidationCallback = Agent.UnsecureTrustAllCertificatesCallback;

                    // Optional: Create an authenticated session; it's also possible
                    // to authenticate all requests with username and password.
                    string token = CreateSession(agent);
                    if (token != null) {
                        // Session successfully created, authenticate all subsequent
                        // requests with its secret token
                        agent.Token = token;
                    }

                    // Create the root PDU object and dump its topology
                    ObjectProxy pduProxy = new ObjectProxy(agent, "/model/pdu/0");
                    DumpPdu(pduProxy);

                    // Finally: close the authenticated session
                    CloseSession();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
