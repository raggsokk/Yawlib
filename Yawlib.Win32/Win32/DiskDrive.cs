#region License
//
// DiskDrive.cs
// 
// The MIT License (MIT)
//
// Copyright (c) 2015 Jarle Hansen
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE. 
//
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Yawlib.Win32
{
    [DebuggerDisplay("{Name}")]
    [WmiClassName("Win32_DiskDrive")]
    public class DiskDrive
    {
        public string DeviceID { get; set; }
        public UInt32 BytesPerSector { get; set; }
        // Capabilities
        // CapabilityDescriptors.
        public string Caption { get; set; }
        public UInt32 ConfigManagerErrorCode { get; set; }
        public bool ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public string FirmwareRevision { get; set; }
        public UInt32 Index { get; set; }
        public string InterfaceType { get; set; }
        public string Manufacturer { get; set; }
        public bool MediaLoaded { get; set; }
        public string MediaType { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public UInt32 Partitions { get; set; }
        public string PNPDeviceID { get; set; }
        public UInt32 SCSIBus { get; set; }
        public UInt16 SCSILogicalUnit { get; set; }
        public UInt16 SCSIPort { get; set; }
        public UInt16 SCSITargetId { get; set; }
        public UInt32 SectorsPerTrack { get; set; }
        public string SerialNumber { get; set; }
        public UInt32 Signature { get; set; }
        public UInt64 Size { get; set; }
        public string Status { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public UInt64 TotalCylinders { get; set; }
        public UInt32 TotalHeads { get; set; }
        public UInt64 TotalSectors { get; set; }
        public UInt64 TotalTracks { get; set; }
        public UInt32 TracksPerCylinder { get; set; }

    }
}
