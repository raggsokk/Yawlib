﻿#region License
//
// NdisMediumEnum.cs
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

namespace Yawlib.StandardCimv2
{
    public enum NdisMediumEnum
    {
        /// <summary>
        /// Ethernet
        /// </summary>
        Ieee802_3 = 0,
        /// <summary>
        /// Token Ring
        /// </summary>
        Ieee802_5 = 1,
        Fddi = 2,
        Wan = 3,
        LocalTalk = 4,
        Dix = 5,
        RawArcnet = 6,
        /// <summary>
        /// Arcnet
        /// </summary>
        Ieee878_2 = 7,
        Atm = 8,
        WirelessWan = 9,
        Irda = 10,
        Bpc = 11,
        ConnectionOrientedWan = 12,
        /// <summary>
        /// Firewire
        /// </summary>
        Ip1394 = 13,
        Ib = 14,
        Tunnel = 15,
        Native802_11 = 16,
        Loopback = 17,
        Wimax = 18,
        Ip = 19,

    }
}
