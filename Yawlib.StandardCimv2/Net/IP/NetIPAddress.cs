#region License
//
// NetIPAddress.cs
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
    [DebuggerDisplay("{InterfaceAlias} -- {IPAddress}/{PrefixLength}")]
    [WmiClassName("MSFT_NetIPAddress")]
    public class NetIPAddress
    {
        public string Name { get; set; }
        public UInt16 AddressFamily { get; set; }
        public UInt16 AddressOrigin { get; set; }
        public UInt16 AddressState { get; set; }
        public UInt16 EnabledDefault { get; set; }
        public string InterfaceAlias { get; set; }
        public UInt32 InterfaceIndex { get; set; }
        public string IPAddress { get; set; }
        public string IPV4Address { get; set; }
        public string IPV6Address { get; set; }
        //public DateTime PreferredLifetime { get; set; }
        public string PreferredLifetime { get; set; }
        public byte PrefixLength { get; set; }
        public UInt16 PrefixOrigin { get; set; }
        public UInt16 ProtocolIFType { get; set; }
        public UInt16 RequestedState { get; set; }
        public bool SkipAsSource { get; set; }
        public byte Store { get; set; }
        public UInt16 SuffixOrigin { get; set; }
        public UInt16 TransitioningToState { get; set; }
        public byte Type { get; set; }
        public string ValidLifetime { get; set; }

    }
}
