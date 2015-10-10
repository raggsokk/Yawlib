#region License
//
// NetFirewallProfile.cs
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
    [DebuggerDisplay("{Name}")]
    [WmiClassName("MSFT_NetFirewallProfile")]
    public class NetFirewallProfile
    {
        public string InstanceID { get; set; }

        public UInt16 AllowInboundRules { get; set; }
        public UInt16 AllowLocalFirewallRules { get; set; }
        public UInt16 AllowLocalIPsecRules { get; set; }
        public UInt16 AllowUnicastResponseToMulticast { get; set; }
        public UInt16 AllowUserApps { get; set; }
        public UInt16 AllowUserPorts { get; set; }
        public UInt16 DefaultInboundAction { get; set; }
        public UInt16 DefaultOutboundAction { get; set; }
        public List<string> DisabledInterfaceAliases { get; set; }
        public string ElementName { get; set; }
        public UInt16 Enabled { get; set; }
        public UInt16 EnableStealthModeForIPsec { get; set; }
        public UInt16 LogAllowed { get; set; }
        public UInt16 LogBlocked { get; set; }
        public string LogFileName { get; set; }
        public UInt16 LogIgnored { get; set; }
        public UInt64 LogMaxSizeKilobytes { get; set; }
        public string Name { get; set; }
        public UInt16 NotifyOnListen { get; set; }
    }
}
