# SPDX-License-Identifier: BSD-3-Clause
#
# Copyright 2020 Raritan Inc. All rights reserved.
#
# This file was generated by IdlC from Firmware.idl.

use strict;

package Raritan::RPC::firmware::ImageState;

use constant NONE => 0;
use constant UPLOADING => 1;
use constant UPLOAD_FAILED => 2;
use constant DOWNLOADING => 3;
use constant DOWNLOAD_FAILED => 4;
use constant COMPLETE => 5;

1;
