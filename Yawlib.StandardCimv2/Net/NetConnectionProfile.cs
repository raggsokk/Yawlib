#region License
//
// NetConnectionProfile.cs
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
    //[DebuggerDisplay("{DestinationPrefix} gw {NextHop} if {InterfaceAlias}")]
    //[WmiClassName("MSFT_NetConnectionProfile")]
    [WmiClassName("MSFT_NetConnectionProfile")]
    [Obsolete("Class fails to query from wmi. Dont know why yet. Exception: Provider Loader Failure!")]
    public class NetConnectionProfile
    {
        public string InstanceID { get; set; }
        public string ElementName { get; set; }
        public string InterfaceAlias { get; set; }
        public UInt32 InterfaceIndex { get; set; }
        public UInt32 IPv4Connectivity { get; set; }
        public UInt32 IPv6Connectivity { get; set; }
        public string Name { get; set; }
        public UInt32 NetworkCategory { get; set; }        
    }
}
