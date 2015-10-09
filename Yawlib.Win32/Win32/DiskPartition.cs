#region License
//
// DiskPartition.cs
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
using System.Text;
using System.Threading.Tasks;

using System.Management;
using System.Diagnostics;

namespace Yawlib.Win32
{
    [DebuggerDisplay("{Name}")]
    [WmiClassName("Win32_DiskPartition")]
    public class DiskPartition
    {
        public string DeviceID { get; set; }
        public UInt32 BlockSize { get; set; }
        public bool Bootable { get; set; }
        public bool BootPartition { get; set; }
        public string Caption { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public UInt32 DiskIndex { get; set; }
        public UInt32 Index { get; set; }
        public string Name { get; set; }
        public UInt64 NumberOfBlocks { get; set; }
        public bool PrimaryPartition { get; set; }
        public UInt64 Size { get; set; }
        public UInt64 StartingOffset { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public string Type { get; set; }
    }
}
