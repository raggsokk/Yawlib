#region License
//
// NetAdapter.cs
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
using System.Diagnostics;

namespace Yawlib.StandardCimv2
{
    [DebuggerDisplay("{InterfaceDescription}")]
    [WmiClassName("MSFT_NetAdapter")]
    public class NetAdapter
    {
        public string CreationClassName { get; set; }
        public Guid DeviceID { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public UInt64 ActiveMaximumTransmissionUnit { get; set; }
        public bool AdminLocked { get; set; }
        public string ComponentID { get; set; }
        public bool ConnectorPresent { get; set; }
        public string DeviceName { get; set; }
        public bool DeviceWakeUpEnable { get; set; }
        [WmiPropertyName("DriverDate", DateTimeFormat = "yyyy-MM-dd")]
        public DateTime DriverDate { get; set; }
        public UInt64 DriverDateData { get; set; }
        public string DriverDescription { get; set; }
        public byte DriverMajorNdisVersion { get; set; }
        public byte DriverMinorNdisVersion { get; set; }
        public string DriverName { get; set; }
        public string DriverProvider { get; set; }
        //Not sure this is a good idea.
        public Version DriverVersionString { get; set; }
        //public UInt16 EnabledDefault { get; set; }
        public EnabledDefaultEnum EnabledDefault { get; set; }
        public UInt16 EnabledState { get; set; }
        public bool EndPointInterface { get; set; }
        public bool HardwareInterface { get; set; }
        public bool Hidden { get; set; }
        public UInt32[] HigherLayerInterfaceIndices { get; set; }
        public bool IMFilter { get; set; }
        public Guid InstanceID { get; set; }
        public UInt32 InterfaceAdminStatus { get; set; }
        public string InterfaceDescription { get; set; }
        public Guid InterfaceGuid { get; set; }
        public UInt32 InterfaceIndex { get; set; }
        public string InterfaceName { get; set; }
        public UInt32 InterfaceOperationalStatus { get; set; }
        public UInt32 InterfaceType { get; set; }
        public bool iSCSIInterface { get; set; }
        public UInt16 MajorDriverVersion { get; set; }
        public UInt32 MediaConnectedState { get; set; }
        //public UInt32 MediaDuplexState { get; set; }
        public MediaDuplexStateEnum MediaDuplexState { get; set; }
        public UInt16 MinorDriverVersion { get; set; }
        public UInt32 MtuSize { get; set; }
        public string Name { get; set; }
        //public UInt32 NdisMedium { get; set; }
        public NdisMediumEnum NdisMedium { get; set; }
        public UInt32 NdisPhysicalMedium { get; set; }
        public UInt64 NetLuid { get; set; }
        public UInt32 NetLuidIndex { get; set; }
        public List<string> NetworkAddresses { get; set; }
        public bool NotUserRemovable { get; set; }
        public bool OperationStatusDownDefaultPortNotAuthenticated { get; set; }
        public bool OperationStatusDownInterfacePaused { get; set; }
        public bool OperationStatusDownLowPowerState { get; set; }
        public bool OperationStatusDownMediaDisconnected { get; set; }
        public string PermanentAddress { get; set; }
        public string PnPDeviceID { get; set; }
        public UInt16 PortNumber { get; set; }
        public bool PromiscuousMode { get; set; }
        public UInt16 RequestedState { get; set; }
        public UInt32 State { get; set; }
        public UInt16 TransitioningToState { get; set; }
        public bool Virtual { get; set; }
        public bool WdmInterace { get; set; }

    }
}
