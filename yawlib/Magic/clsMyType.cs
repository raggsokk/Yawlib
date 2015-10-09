#region License
//
// clsMyType.cs
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
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace yawlib.Magic
{
    [DebuggerDisplay("{WmiClassName}")]
    internal class clsMyType
    {
        /// <summary>
        /// .net name of type.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// The full .net name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Name to use during wmi queries.
        /// </summary>
        public string WmiClassName { get; set; }

        /// <summary>
        /// The type this is mapped on.
        /// </summary>
        public Type RefType { get; set; }

        /// <summary>
        /// A hash of wmi name to property name mapping.
        /// </summary>
        public SortedDictionary<string, clsMyProperty> WmiProperties = new SortedDictionary<string, clsMyProperty>();

        // Reflection
        /// <summary>
        /// Direct reference to a type creation delegate.
        /// </summary>
        public Reflection.CreateObject CreateObject { get; set; }                    

        internal clsMyType(Type t)
        {
            // create mytype.

            this.TypeName = t.Name;
            this.FullName = t.FullName;

            // set wmi class name to use.
            var attribWmiClassName = t.GetCustomAttribute<WmiClassNameAttribute>();
            if (attribWmiClassName != null)
                this.WmiClassName = attribWmiClassName.WmiClassName;
            else
                this.WmiClassName = t.Name;

            // Compile createobject.
            //this.CreateObject = Reflection.CompileCreateObject(t);            
            this.CreateObject = Reflection.Instance.TryGetCreateObject(t.FullName, t);

            var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public); // | BindingFlags.SetProperty

            foreach(var p in props)
            {
                var myprop = new clsMyProperty(p);

                WmiProperties.Add(myprop.WmiName, myprop);
            }
        }

        /// <summary>
        /// Creates a Select All WQL Query.
        /// </summary>
        /// <returns></returns>
        internal string CreateSelectAll()
        {
            return string.Format("SELECT * FROM {0}",
                WmiClassName);
        }

    }
}
