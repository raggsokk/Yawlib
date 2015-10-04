#region License
//
// ComputerSystemProduct.cs
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
    [WmiClassName("Win32_ComputerSystemProduct")]
    public class ComputerSystemProduct
    {
        public string IdentifyingNumber { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public Guid UUID { get; set; }
        public string Vendor { get; set; }

        private static readonly string WqlSelect = "SELECT IdentifyingNumber,Name,Version,Caption,Description,UUID,Vendor from Win32_ComputerSystemProduct";

        public static ComputerSystemProduct Parse(ManagementBaseObject mba)
        {
            var csproduct = new ComputerSystemProduct();

            foreach(var p in mba.Properties)
            {
                switch(p.Name)
                {
                    case "IdentifyingNumber":
                        csproduct.IdentifyingNumber = p.Value as string;
                        break;
                    case "Name":
                        csproduct.Name = p.Value as string;
                        break;
                    case "Version":
                        csproduct.Vendor = p.Value as string;
                        break;
                    case "Caption":
                        csproduct.Caption = p.Value as string;
                        break;
                    case "Description":
                        csproduct.Description = p.Value as string;
                        break;
                    case "UUID":
                        csproduct.UUID = Guid.Parse(p.Value as string);
                        break;
                    case "Vendor":
                        csproduct.Vendor = p.Value as string;
                        break;
                    default:
                        break;
                }
            }

            return csproduct;
        }

        public static List<ComputerSystemProduct> Retrive(WmiConnection connection)
        {
            var q = new SelectQuery(WqlSelect);

            return connection.Query<ComputerSystemProduct>(q, (mbo) =>
            {
                return Parse(mbo);
            });
        }

        public static async Task<List<ComputerSystemProduct>> RetriveAsync(WmiConnection connection)
        {
            var q = new SelectQuery(WqlSelect);

            return await connection.QueryAsync<ComputerSystemProduct>(q, (mbo) =>
            {
                return Parse(mbo);
            });
        }
    }
}
