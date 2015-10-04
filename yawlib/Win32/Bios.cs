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
        public int SoftwareElementState { get; set; }
        public int TargetOperationSystem { get; set; }
        public string Version { get; set; }
        //public UInt16[] BiosCharacteristics { get; set; }
        public List<string> BiosVersion { get; set; }
        public string Caption { get; set; }
        public string CurrentLanguage { get; set; }
        public string Description { get; set; }
        public byte EmbeddedControllerMajorVersion { get; set; }
        public byte EmbeddedControllerMinorVersion { get; set; }
        public int InstallableLanguages { get; set; }
        public List<string> ListOfLanguages { get; set; }
        public string Manufacturer { get; set; }
        public bool PrimaryBios { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string SerialNumber { get; set; }
        public string SMBIOSBIOSVersion { get; set; }
        public int SMBIOSMajorVersion { get; set; }
        public int SMBIOSMinorVersion { get; set; }
        public bool SMBIOSPresent { get; set; }
        public string Status { get; set; }
        public byte SystemBiosMajorVersion { get; set; }
        public byte SystemBiosMinorVersion { get; set; }

        private static readonly string WqlSelect = "SELECT * from Win32_BIOS";

        public static Bios Parse(ManagementBaseObject mba)
        {
            var bios = new Bios();

            foreach (var p in mba.Properties)
            {
                switch (p.Name)
                {
                    case "Name":
                        bios.Name = p.Value as string;
                        break;
                    case "Version":
                        bios.Version = p.Value as string;
                        break;
                    case "Caption":
                        bios.Caption = p.Value as string;
                        break;
                    case "CurrentLanguage":
                        bios.CurrentLanguage = p.Value as string;
                        break;
                    case "Description":
                        bios.Description = p.Value as string;
                        break;
                    case "Manufacturer":
                        bios.Manufacturer = p.Value as string;
                        break;
                    case "PrimaryBIOS":
                        bios.PrimaryBios = (bool)p.Value;
                        break;
                    case "ReleaseDate":
                        bios.ReleaseDate = (DateTime)p.Value;
                        break;
                    case "SerialNumber":
                        bios.SerialNumber = p.Value as string;
                        break;
                    case "SMBIOSBIOSVersion":
                        bios.SMBIOSBIOSVersion = p.Value as string;
                        break;
                    case "SMBIOSMajorVersion":
                        bios.SMBIOSMajorVersion = (int)p.Value;
                        break;
                    case "SMBIOSMinorVersion":
                        bios.SMBIOSMinorVersion = (int)p.Value;
                        break;
                    case "SMBIOSPresent":
                        bios.SMBIOSPresent = (bool)p.Value;
                        break;
                    case "Status":
                        bios.Status = p.Value as string;
                        break;
                    case "SystemBiosMajorVersion":
                        bios.SystemBiosMajorVersion = (byte)p.Value;
                        break;
                    case "SystemBiosMinorVersion":
                        bios.SystemBiosMinorVersion = (byte)p.Value;
                        break;
                    default:
                        break;
                }
            }

            return bios;
        }

        public static List<Bios> Retrive(WmiConnection connection)
        {
            var q = new SelectQuery(WqlSelect);

            return connection.Query<Bios>(q, (mbo) =>
            {
                return Parse(mbo);
            });
        }

        public static async Task<List<Bios>> RetriveAsync(WmiConnection connection)
        {
            var q = new SelectQuery(WqlSelect);

            return await connection.QueryAsync<Bios>(q, (mbo) =>
            {
                return Parse(mbo);
            });
        }
    }
}
