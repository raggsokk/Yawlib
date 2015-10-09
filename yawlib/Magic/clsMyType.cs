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


        public SortedDictionary<string, clsMyProperty> WmiProperties = new SortedDictionary<string, clsMyProperty>();

        // Reflection
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
            this.CreateObject = Reflection.CompileCreateObject(t);            

            var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public); // | BindingFlags.SetProperty

            foreach(var p in props)
            {
                var myprop = new clsMyProperty(p);

                WmiProperties.Add(myprop.WmiName, myprop);
            }
        }

        internal string CreateSelectAll()
        {
            return string.Format("SELECT * FROM {0}",
                WmiClassName);
        }

        internal bool Convert(List<ManagementBaseObject> data, System.Collections.IList result)
        {
            if (data == null)
                throw new ArgumentNullException(string.Format("Argument cant be null. Arg='{0}', Type='{1}'", nameof(data), typeof(ManagementObjectCollection).Name));

            if (data.Count == 0)
                return true;

            foreach (var item in data)
            {
                var instance = this.CreateObject();

                foreach (var p in item.Properties)
                {
                    if (!p.IsLocal)
                        continue; // if prop not defined on this instance, skip searching for it.
                    //else if (p.IsArray)
                    //    continue; // array is not supported yet...

                    clsMyProperty myprop = null;

                    if (WmiProperties.TryGetValue(p.Name, out myprop))
                    {
                        object oset = null;

                        switch(myprop.DetailInfo)
                        {
                            case MyTypeInfoEnum.String:
                                if (myprop.IsArray && p.IsArray)
                                    oset = (string[])p.Value;
                                if(myprop.IsList && p.IsArray)
                                {
                                    var list = new List<string>();

                                    foreach(var o in (string[])p.Value)
                                    {
                                        list.Add(o);
                                    }
                                    oset = list;
                                }
                                else
                                    oset = p.Value;
                                break;
                            case MyTypeInfoEnum.Guid:
                                Guid g;
                                if (Guid.TryParse(p.Value as string, out g))
                                    oset = g;
                                break;
                            case MyTypeInfoEnum.DateTime:
                                oset = ManagementDateTimeConverter.ToDateTime(p.Value.ToString());
                                break;
                            case MyTypeInfoEnum.TimeSpan:
                                oset = ManagementDateTimeConverter.ToTimeSpan(p.Value.ToString());
                                break;
                            case MyTypeInfoEnum.Bool:
                                oset = (bool)p.Value;
                                break;
                            case MyTypeInfoEnum.UInt8:
                                oset = (byte)p.Value;
                                break;
                            case MyTypeInfoEnum.UInt16:
                                oset = (UInt16)p.Value;
                                break;
                            case MyTypeInfoEnum.UInt32:
                                oset = (UInt32)p.Value;
                                break;
                            case MyTypeInfoEnum.UInt64:
                                oset = (UInt64)p.Value;
                                break;
                            case MyTypeInfoEnum.Int8:
                                oset = (byte)p.Value;
                                break;
                            case MyTypeInfoEnum.Int16:
                                oset = (UInt16)p.Value;
                                break;
                            case MyTypeInfoEnum.Int32:
                                oset = (UInt32)p.Value;
                                break;
                            case MyTypeInfoEnum.Int64:
                                oset = (UInt64)p.Value;
                                break;
                            case MyTypeInfoEnum.Char:
                                oset = (Char)p.Value;
                                break;
                            case MyTypeInfoEnum.Float:
                                oset = (float)p.Value;
                                break;
                            case MyTypeInfoEnum.Double:
                                oset = (double)p.Value;
                                break;

                        }

                        if(myprop.DetailInfo != MyTypeInfoEnum.Invalid)
                            instance = myprop.GenericSetter(instance, oset);


                        //// convert.
                        //switch (p.Type)
                        //{
                        //    //case CimType.String:
                        //    //    //TODO: handle when string is actually GUID, UUID, IPAddress, etc.
                        //    //    oset = p.Value; // no conversion.                                
                        //    //    break;
                        //    case CimType.UInt8:
                        //        oset = (byte)p.Value;
                        //        break;
                        //    case CimType.UInt16:
                        //        oset = (UInt16)p.Value;
                        //        break;
                        //    case CimType.UInt32:
                        //        oset = (UInt32)p.Value;
                        //        break;
                        //    case CimType.UInt64:
                        //        oset = (UInt64)p.Value;
                        //        break;
                        //    case CimType.Boolean:
                        //        oset = (bool)p.Value;
                        //        break;
                        //    case CimType.SInt8:
                        //        oset = (sbyte)p.Value;
                        //        break;
                        //    case CimType.SInt16:
                        //        oset = (Int16)p.Value;
                        //        break;
                        //    case CimType.SInt32:
                        //        oset = (Int32)p.Value;
                        //        break;
                        //    case CimType.SInt64:
                        //        oset = (Int64)p.Value;
                        //        break;
                        //    case CimType.DateTime:
                        //        var strDatetime = p.Value.ToString();

                        //        if (myprop.DetailInfo == MyTypeInfoEnum.DateTime)
                        //            oset = ManagementDateTimeConverter.ToDateTime(strDatetime);
                        //        else if (myprop.DetailInfo == MyTypeInfoEnum.TimeSpan)
                        //            oset = ManagementDateTimeConverter.ToTimeSpan(strDatetime);
                        //        else if (myprop.DetailInfo == MyTypeInfoEnum.String)
                        //            oset = strDatetime;
                        //        else
                        //        {
                        //            // throw error.
                        //            throw new ArgumentException(string.Format("Cant convert WMi DateTime to type '{0}'", myprop.RefType));
                        //        }
                        //        break;
                        //    default:

                        //        // try to use clsMyType.
                        //        switch(myprop.DetailInfo)
                        //        {
                        //            case MyTypeInfoEnum.String:
                        //                oset = p.Value; // no conversion.
                        //                break;
                        //            case MyTypeInfoEnum.Guid:
                        //                Guid g;
                        //                if (Guid.TryParse(p.Value as string, out g))
                        //                    oset = g;
                        //                break;
                        //            case MyTypeInfoEnum.DateTime:
                        //                //todo: reuse datetime conversion above....
                        //                DateTime dt;
                        //                if (DateTime.TryParse(p.Value as string, out dt))
                        //                    oset = dt;
                        //                break;
                        //            default:
                        //                throw new NotSupportedException(string.Format(
                        //                    "Type '{0}' is not supported yet for conversion from '{1}'",
                        //                    myprop.RefType, p.Type));
                        //        }

                        //        break;
                        //}

                        //instance = myprop.GenericSetter(instance, oset);
                    }
                }

                result.Add(instance);
            }

            return true;
        }

    }
}
