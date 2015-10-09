#region License
//
// Bios.cs
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

namespace yawlib.Win32
{
    [WmiClassName("Win32_BIOS")]
    public class Bios
    {
        public string Name { get; set; }
        public string SoftwareElementID { get; set; }
        public UInt16 SoftwareElementState { get; set; }
        public UInt16 TargetOperationSystem { get; set; }
        public string Version { get; set; }
        //public UInt16[] BiosCharacteristics { get; set; }
        public List<string> BiosVersion { get; set; }
        public string Caption { get; set; }
        public string CurrentLanguage { get; set; }
        public string Description { get; set; }
        public byte EmbeddedControllerMajorVersion { get; set; }
        public byte EmbeddedControllerMinorVersion { get; set; }
        public UInt16 InstallableLanguages { get; set; }
        public List<string> ListOfLanguages { get; set; }
        public string Manufacturer { get; set; }
        public bool PrimaryBios { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string SerialNumber { get; set; }
        public string SMBIOSBIOSVersion { get; set; }
        public UInt16 SMBIOSMajorVersion { get; set; }
        public UInt16 SMBIOSMinorVersion { get; set; }
        public bool SMBIOSPresent { get; set; }
        public string Status { get; set; }
        public byte SystemBiosMajorVersion { get; set; }
        public byte SystemBiosMinorVersion { get; set; }

    }
}
