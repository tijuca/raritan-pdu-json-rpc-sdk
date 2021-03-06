// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2020 Raritan Inc. All rights reserved.
//
// This file was generated by IdlC from Firmware.idl.

using System;
using System.Linq;
using LightJson;
using Com.Raritan.Idl;
using Com.Raritan.JsonRpc;
using Com.Raritan.Util;

#pragma warning disable 0108, 0219, 0414, 1591

namespace Com.Raritan.Idl.firmware {

  public class ImageInfo_1_0_1 : ICloneable {
    public object Clone() {
      ImageInfo_1_0_1 copy = new ImageInfo_1_0_1();
      copy.valid = this.valid;
      copy.version = this.version;
      copy.min_required_version = this.min_required_version;
      copy.min_downgrade_version = this.min_downgrade_version;
      copy.product = this.product;
      copy.platform = this.platform;
      copy.oem = this.oem;
      copy.hwid_whitelist = this.hwid_whitelist;
      copy.hwid_blacklist = this.hwid_blacklist;
      copy.compatible = this.compatible;
      copy.signature_present = this.signature_present;
      copy.signed_by = this.signed_by;
      copy.signature_good = this.signature_good;
      copy.certified_by = this.certified_by;
      copy.certificate_good = this.certificate_good;
      copy.model_list_present = this.model_list_present;
      copy.model_supported = this.model_supported;
      return copy;
    }

    public LightJson.JsonObject Encode() {
      LightJson.JsonObject json = new LightJson.JsonObject();
      json["valid"] = this.valid;
      json["version"] = this.version;
      json["min_required_version"] = this.min_required_version;
      json["min_downgrade_version"] = this.min_downgrade_version;
      json["product"] = this.product;
      json["platform"] = this.platform;
      json["oem"] = this.oem;
      json["hwid_whitelist"] = this.hwid_whitelist;
      json["hwid_blacklist"] = this.hwid_blacklist;
      json["compatible"] = this.compatible;
      json["signature_present"] = this.signature_present;
      json["signed_by"] = this.signed_by;
      json["signature_good"] = this.signature_good;
      json["certified_by"] = this.certified_by;
      json["certificate_good"] = this.certificate_good;
      json["model_list_present"] = this.model_list_present;
      json["model_supported"] = this.model_supported;
      return json;
    }

    public static ImageInfo_1_0_1 Decode(LightJson.JsonObject json, Agent agent) {
      ImageInfo_1_0_1 inst = new ImageInfo_1_0_1();
      inst.valid = (bool)json["valid"];
      inst.version = (string)json["version"];
      inst.min_required_version = (string)json["min_required_version"];
      inst.min_downgrade_version = (string)json["min_downgrade_version"];
      inst.product = (string)json["product"];
      inst.platform = (string)json["platform"];
      inst.oem = (string)json["oem"];
      inst.hwid_whitelist = (string)json["hwid_whitelist"];
      inst.hwid_blacklist = (string)json["hwid_blacklist"];
      inst.compatible = (bool)json["compatible"];
      inst.signature_present = (bool)json["signature_present"];
      inst.signed_by = (string)json["signed_by"];
      inst.signature_good = (bool)json["signature_good"];
      inst.certified_by = (string)json["certified_by"];
      inst.certificate_good = (bool)json["certificate_good"];
      inst.model_list_present = (bool)json["model_list_present"];
      inst.model_supported = (bool)json["model_supported"];
      return inst;
    }

    public bool valid = false;
    public string version = "";
    public string min_required_version = "";
    public string min_downgrade_version = "";
    public string product = "";
    public string platform = "";
    public string oem = "";
    public string hwid_whitelist = "";
    public string hwid_blacklist = "";
    public bool compatible = false;
    public bool signature_present = false;
    public string signed_by = "";
    public bool signature_good = false;
    public string certified_by = "";
    public bool certificate_good = false;
    public bool model_list_present = false;
    public bool model_supported = false;
  }
}
