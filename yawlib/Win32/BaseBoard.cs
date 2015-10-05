#region License
//
// BaseBoard.cs
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
    [WmiClassName("Win32_BaseBoard")]
    public class BaseBoard //: IWmiParseable
    {
        public string Caption { get; set; }
        public List<string> ConfigOptions { get; set; }
        public string Description { get; set; }
        public bool HostingBoard { get; set; }
        public bool HotSwappable { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public bool PoweredOn { get; set; }
        public string Product { get; set; }
        public bool Removable { get; set; }
        public bool Replaceable { get; set; }
        public bool RequiresDaughterBoard { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }

        //IWmiParseable IWmiParseable.Parse(ManagementBaseObject mba)
        //{
        //    var baseboard = new BaseBoard();

        //    foreach (var p in mba.Properties)
        //    {
        //        switch (p.Name)
        //        {
        //            case "Caption":
        //                baseboard.Caption = p.Value as string;
        //                break;
        //            case "ConfigOptions":
        //                if (p.IsArray && p.Type == CimType.String)
        //                {
        //                    baseboard.ConfigOptions = new List<string>();
        //                    var arr = p.Value as string[];
        //                    foreach (var c in arr)
        //                        baseboard.ConfigOptions.Add(c);
        //                }
        //                break;
        //            case "Description":
        //                baseboard.Description = p.Value as string;
        //                break;
        //            case "HostingBoard":
        //                baseboard.HostingBoard = (bool)p.Value;
        //                break;
        //            case "HotSwappable":
        //                baseboard.HotSwappable = (bool)p.Value;
        //                break;
        //            case "Manufacturer":
        //                baseboard.Manufacturer = p.Value as string;
        //                break;
        //            case "Name":
        //                baseboard.Name = p.Value as string;
        //                break;
        //            case "PoweredOn":
        //                baseboard.PoweredOn = (bool)p.Value;
        //                break;
        //            case "Removable":
        //                baseboard.Removable = (bool)p.Value;
        //                break;
        //            case "Replaceable":
        //                baseboard.Replaceable = (bool)p.Value;
        //                break;
        //            case "RequiresDaughterBoard":
        //                baseboard.RequiresDaughterBoard = (bool)p.Value;
        //                break;
        //            case "SerialNumber":
        //                baseboard.SerialNumber = p.Value as string;
        //                break;
        //            case "Status":
        //                baseboard.Status = p.Value as string;
        //                break;
        //            case "Version":
        //                baseboard.Version = p.Value as string;
        //                break;

        //            default:
        //                break;
        //        }
        //    }

        //    return baseboard;
        //}

        //public static BaseBoard Parse(ManagementBaseObject mba)
        //{
        //    var baseboard = new BaseBoard();

        //    foreach (var p in mba.Properties)
        //    {
        //        switch (p.Name)
        //        {
        //            case "Caption":
        //                baseboard.Caption = p.Value as string;
        //                break;
        //            case "ConfigOptions":
        //                if (p.IsArray && p.Type == CimType.String)
        //                {
        //                    baseboard.ConfigOptions = new List<string>();
        //                    var arr = p.Value as string[];
        //                    foreach (var c in arr)
        //                        baseboard.ConfigOptions.Add(c);
        //                }
        //                break;
        //            case "Description":
        //                baseboard.Description = p.Value as string;
        //                break;
        //            case "HostingBoard":
        //                baseboard.HostingBoard = (bool)p.Value;
        //                break;
        //            case "HotSwappable":
        //                baseboard.HotSwappable = (bool)p.Value;
        //                break;
        //            case "Manufacturer":
        //                baseboard.Manufacturer = p.Value as string;
        //                break;
        //            case "Name":
        //                baseboard.Name = p.Value as string;
        //                break;
        //            case "PoweredOn":
        //                baseboard.PoweredOn = (bool)p.Value;
        //                break;
        //            case "Removable":
        //                baseboard.Removable = (bool)p.Value;
        //                break;
        //            case "Replaceable":
        //                baseboard.Replaceable = (bool)p.Value;
        //                break;
        //            case "RequiresDaughterBoard":
        //                baseboard.RequiresDaughterBoard = (bool)p.Value;
        //                break;
        //            case "SerialNumber":
        //                baseboard.SerialNumber = p.Value as string;
        //                break;
        //            case "Status":
        //                baseboard.Status = p.Value as string;
        //                break;
        //            case "Version":
        //                baseboard.Version = p.Value as string;
        //                break;

        //            default:
        //                break;
        //        }
        //    }

        //    return baseboard;
        //}

        //public static List<BaseBoard> Retrive(WmiConnection connection)
        //{
        //    var q = new SelectQuery(WqlSelect);

        //    return connection.Query<BaseBoard>(q, (mbo) =>
        //    {
        //        return Parse(mbo);
        //    });
        //}

        //public static async Task<List<BaseBoard>> RetriveAsync(WmiConnection connection)
        //{
        //    var q = new SelectQuery(WqlSelect);

        //    return await connection.QueryAsync<BaseBoard>(q, (mbo) =>
        //    {
        //        return Parse(mbo);
        //    });
        //}
    }
}
