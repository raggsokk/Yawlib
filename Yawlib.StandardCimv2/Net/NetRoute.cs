#region License
//
// NetRoute.cs
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
    [DebuggerDisplay("{DestinationPrefix} gw {NextHop} if {InterfaceAlias}")]
    [WmiClassName("MSFT_NetRoute")]
    public class NetRoute
    {
        public string InstanceID { get; set; }
        public UInt16 AddressFamily { get; set; }
        public string DestinationPrefix { get; set; }
        public string InterfaceAlias { get; set; }
        public UInt32 InterfaceIndex { get; set; }
        public string NextHop { get; set; }
        //public DateTime PreferredLifetime { get; set; }
        public string PreferredLifetime { get; set; }
        public UInt16 Protocol { get; set; }
        public byte Publish { get; set; }
        public UInt16 RouteMetric { get; set; }
        public byte Store { get; set; }
        public UInt16 TypeOfRoute { get; set; }
        //public DateTime ValidLifetime { get; set; }
        public string ValidLifetime { get; set; }
    }
}
