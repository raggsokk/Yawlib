#region License
//
// Convert.cs
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
    /// Functions related to converting wmi query result into our types.
    /// </summary>
    internal static class MyConvert
    {        
        /// <summary>
        /// Convert a list of ManagementBaseObjects into our types.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="myType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        internal static bool Convert(List<ManagementBaseObject> data, clsMyType myType, System.Collections.IList result)
        {
            bool flagPerfectConversion = true;

            if (data == null)
                throw new ArgumentNullException(string.Format("Argument cant be null. Arg='{0}', Type='{1}'", nameof(data), typeof(ManagementObjectCollection).Name));

            if (data.Count == 0)
                return true;

            foreach (var item in data)
            {
                // Create the instance.
                var instance = myType.CreateObject();

                foreach (var p in item.Properties)
                {
                    if (!p.IsLocal) // are there any info in this propname??
                        continue; // no, then continue.

                    clsMyProperty myprop = null;

                    // try to match wmi prop against our props.
                    if (myType.WmiProperties.TryGetValue(p.Name, out myprop))
                    {
                        if (myprop.DetailInfo == MyTypeInfoEnum.Invalid) // ignore types we dont know how to handle.
                        {
                            flagPerfectConversion = false; // set not perfect conversion flag.
                            continue;
                        }

                        object oset = null;

                        if (myprop.IsArray)
                        {
                            //TODO: Have 100% wmi prop value type to myprop value type match.
                            //if (myprop.BaseType.IsPrimitive)
                            //    oset = p.Value;
                            //else
                                oset = CreateArray(p.Value, myprop);
                        }                            
                        else if (myprop.IsList)
                            oset = CreateGenericList(p.Value, myprop);
                        else
                            oset = ConvertObject(p.Value, myprop.DetailInfo, myprop.IsNullable);

                        //if (myprop.DetailInfo != MyTypeInfoEnum.Invalid)
                        instance = myprop.GenericSetter(instance, oset);
                    }
                }

                result.Add(instance);
            }

            return flagPerfectConversion;
        }

        /// <summary>
        /// Convert wmi value data into .net friendly values.
        /// </summary>
        /// <param name="wmivalue">raw wmi type.</param>
        /// <param name="detailinfo">.net type enum.</param>
        /// <param name="nullable">if value is nullable or should return default value.</param>
        /// <returns></returns>
        internal static object ConvertObject(object wmivalue, MyTypeInfoEnum detailinfo, bool nullable = false)
        {            
            switch(detailinfo)
            {
                case MyTypeInfoEnum.Guid:
                    Guid g;
                    if (Guid.TryParse(wmivalue as string, out g))
                        return g;
                    else if (nullable)
                        return null;
                    else
                        return Guid.Empty;
                case MyTypeInfoEnum.DateTime:
                    return ManagementDateTimeConverter.ToDateTime(wmivalue.ToString());
                case MyTypeInfoEnum.TimeSpan:
                    return ManagementDateTimeConverter.ToTimeSpan(wmivalue.ToString());
                default:
                    return wmivalue; // no conversion needed. hopefully.
            }
        }

        /// <summary>
        /// Creates a 
        /// </summary>
        /// <param name="wmivalue"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        internal static object CreateArray(object wmivalue, clsMyProperty prop)
        {
            //TODO: Find out if we can in some cases just set whoe array directly?

            var array = (Array)wmivalue; // cast to array.

            // create new array of base type and same length.
            var result = Array.CreateInstance(prop.BaseType, array.Length);

            int current = 0;

            // enumerate wmi array,
            foreach(var obj in array)
            {
                // convert objects in case of guid array or datetime arrays
                var o = ConvertObject(obj, prop.DetailInfo, prop.IsNullable);

                // set object.
                result.SetValue(o, current++);
            }

            return result;
        }

        /// <summary>
        /// Creates a generic list and adds array into it.
        /// </summary>
        /// <param name="wmivalue"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        internal static object CreateGenericList(object wmivalue, clsMyProperty prop)
        {
            // retrive or compile new generic list create delegate.
            var create = Reflection.Instance.TryGetCreateObject(prop.RefType.FullName, prop.RefType);

            // create array.
            var list = (System.Collections.IList)create();

            // cast to object[]
            //TODO: cast to array instead?
            var array = (object[])wmivalue;

            foreach(var obj in array)
            {
                // convert objects in case of guid list or datetime list
                var o = ConvertObject(obj, prop.DetailInfo, prop.IsNullable);

                // add
                list.Add(o);
            }

            return list;
        }
    }
}
