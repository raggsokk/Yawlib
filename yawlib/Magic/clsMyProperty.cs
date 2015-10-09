#region License
//
// clsMyProperty.cs
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
    [DebuggerDisplay("{WmiName}")]
    internal class clsMyProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The WMI Property Name 
        /// </summary>
        public string WmiName { get; set; }

        /// <summary>
        /// Reference to actualy type.
        /// </summary>
        public Type RefType { get; set; }

        /// <summary>
        /// More info about type here.
        /// </summary>
        public MyTypeInfoEnum DetailInfo { get; set; }

        // reflection
        public Reflection.GenericSetter GenericSetter { get; set; }

        public clsMyProperty(PropertyInfo p)
        {
            this.Name = p.Name;

            var attribWmiProp = p.GetCustomAttribute<WmiPropertyNameAttribute>();
            if (attribWmiProp != null)
                this.WmiName = attribWmiProp.WmiPropertyName;
            else            
                this.WmiName = p.Name;

            this.RefType = p.PropertyType;

            //TODO: Handle array creation.
            if (this.RefType == typeof(string))
                this.DetailInfo = MyTypeInfoEnum.String;
            else if (this.RefType == typeof(Guid))
                this.DetailInfo = MyTypeInfoEnum.Guid;
            else if (this.RefType == typeof(DateTime))
                this.DetailInfo = MyTypeInfoEnum.DateTime;
            else
                this.DetailInfo = MyTypeInfoEnum.Invalid;

            this.GenericSetter = Reflection.CompileGenericSetMethod(p.DeclaringType, p);
        }
    }
}
