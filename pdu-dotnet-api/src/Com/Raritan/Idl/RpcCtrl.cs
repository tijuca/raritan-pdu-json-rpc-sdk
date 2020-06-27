// SPDX-License-Identifier: BSD-3-Clause
//
// Copyright 2018 Raritan Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 1591

namespace Com.Raritan.Idl {
    public class RpcCtrl {
        public enum BulkType { ALLOW, FORBID };

        private BulkType bulk;

        private static RpcCtrl defaultInst = new RpcCtrl();

        public RpcCtrl() : this(BulkType.ALLOW) {
        }

        public RpcCtrl(BulkType bulk) {
            this.bulk = bulk;
        }

        public static RpcCtrl Default {
            get { return defaultInst; }
        }

        public BulkType Bulk {
            get { return bulk; }
        }
    }
}
