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
    /// <summary>
    /// Creates cache about a property.
    /// </summary>
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

        /// <summary>
        /// The wmi type compatible for direct mapping.
        /// </summary>
        public CimType CimType { get; set; }

        /// <summary>
        /// Is this a generic list.
        /// </summary>
        public bool IsList { get; set; }
        /// <summary>
        /// Is this an array.
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// Base array. For nullable and array this is the underlying type. For list this is the generic type.
        /// </summary>
        public Type BaseType { get; set; }

        /// <summary>
        /// If this object is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        // reflection
        /// <summary>
        /// Stores a generic setter for this property.
        /// </summary>
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

            var typename = RefType.Name;

            if(RefType.IsArray)
            {
                this.IsArray = true;                
                this.BaseType = RefType.GetElementType();
                //TODO: get wmi datatype here to enable direct array setting.

                typename = this.BaseType.Name;
            }
            else if (RefType.IsGenericType)
            {
                var g = RefType.GetGenericTypeDefinition();

                if (g.Equals(typeof(Nullable<>)))
                {
                    this.IsNullable = true;
                    this.BaseType = this.RefType.UnderlyingSystemType;
                    typename = this.BaseType.Name;
                }
                else if (g.Equals(typeof(List<>)))
                {
                    this.IsList = true;
                    this.BaseType = RefType.GetGenericArguments().First();
                    typename = this.BaseType.Name;
                }
                else
                    typename = "INVALID";

            }

            this.CimType = CimType.None;

            switch (typename)
            {
                case "String":
                    this.DetailInfo = MyTypeInfoEnum.String;
                    this.CimType = CimType.String;
                    break;
                case "Guid":
                    this.DetailInfo = MyTypeInfoEnum.Guid;                    
                    break;
                case "DateTime":
                    this.DetailInfo = MyTypeInfoEnum.DateTime;
                    this.CimType = CimType.DateTime;
                    break;
                case "TimeSpan":
                    this.DetailInfo = MyTypeInfoEnum.TimeSpan;
                    break;
                case "UInt16":
                    this.DetailInfo = MyTypeInfoEnum.UInt16;
                    this.CimType = CimType.UInt16;
                    break;
                case "UInt32":
                    this.DetailInfo = MyTypeInfoEnum.UInt32;
                    this.CimType = CimType.UInt32;
                    break;
                case "UInt64":
                    this.DetailInfo = MyTypeInfoEnum.UInt64;
                    this.CimType = CimType.UInt64;
                    break;
                case "Boolean":
                    this.DetailInfo = MyTypeInfoEnum.Bool;
                    this.CimType = CimType.Boolean;
                    break;
                case "Int16":
                    this.DetailInfo = MyTypeInfoEnum.Int16;
                    this.CimType = CimType.SInt16;
                    break;
                case "Int32":
                    this.DetailInfo = MyTypeInfoEnum.Int32;
                    this.CimType = CimType.SInt32;
                    break;
                case "Int64":
                    this.DetailInfo = MyTypeInfoEnum.Int64;
                    this.CimType = CimType.SInt64;
                    break;
                case "Char":
                    this.DetailInfo = MyTypeInfoEnum.Char;
                    this.CimType = CimType.Char16;
                    break;
                case "Single":
                    this.DetailInfo = MyTypeInfoEnum.Float;
                    this.CimType = CimType.Real32;
                    break;
                case "Double":
                    this.DetailInfo = MyTypeInfoEnum.Double;
                    this.CimType = CimType.Real64;
                    break;
                case "Byte":
                    this.DetailInfo = MyTypeInfoEnum.UInt8;
                    this.CimType = CimType.UInt8;
                    break;
                case "SByte":
                    this.DetailInfo = MyTypeInfoEnum.Int8;
                    this.CimType = CimType.SInt8;
                    break;
                default:
                    this.DetailInfo = MyTypeInfoEnum.Invalid;
                    break;
            }

            this.GenericSetter = Reflection.CompileGenericSetMethod(p.DeclaringType, p);
        }
    }
}
