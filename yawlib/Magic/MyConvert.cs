﻿#region License
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


namespace Yawlib.Magic
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
                    //if (!p.IsLocal) // are there any info in this propname??
                    //    continue; // no, then continue.

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
                            if (myprop.CimType == p.Type)
                                oset = p.Value;
                            else
                                oset = CreateArray(p.Value, myprop);
                        }
                        else if (myprop.IsList)
                            oset = CreateGenericList(p.Value, myprop);
                        else
                            oset = ConvertObject(p.Value, myprop);
                        //oset = ConvertObject(p.Value, myprop.DetailInfo, myprop.IsNullable);

                        //if (myprop.DetailInfo != MyTypeInfoEnum.Invalid)
                        if (myprop.IsNullable || oset != null)
                            instance = myprop.GenericSetter(instance, oset);
                    }
                }

                result.Add(instance);
            }

            return flagPerfectConversion;
        }

        /// <summary>
        /// Convert a wmi version string into an actual version struct.
        /// </summary>
        /// <param name="wmiValue"></param>
        /// <returns></returns>
        private static object ConvertVersion(object wmiValue)
        {
            var strVersion = wmiValue as string;

            if(strVersion != null)
            {
                Version v;

                if (Version.TryParse(strVersion, out v))
                    return v;
            }

            return null;
        }

        /// <summary>
        /// Convert wmi guid string into an actual guid.
        /// </summary>
        /// <param name="wmiValue"></param>
        /// <returns></returns>
        private static object ConvertGuid(object wmiValue)
        {            
            Guid g;
            var strGuid = wmiValue as string;

            if(strGuid != null)
            {
                if (Guid.TryParse(strGuid, out g))
                    return g;
            }

            return null;
        }
        private static object ConvertEnum(object wmiValue, clsMyProperty myProp)
        {
            //TODO: handle conversion errors.
            return Enum.Parse(myProp.RefType, wmiValue.ToString());
            //return null;
        }

        /// <summary>
        /// Convert wmi datetime string into datetime.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="CustomFormat"></param>
        /// <returns></returns>
        private static object ConvertDateTime(object dt, string CustomFormat = null) //, bool nullable = false)
        {            
            var strDateTime = dt as string;
            DateTime result;

            if(!string.IsNullOrWhiteSpace(CustomFormat))
            {
                if (DateTime.TryParseExact(strDateTime, CustomFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result))
                    return result;
            }

            // if not custom format succeded, try fallback anyway.
            if(strDateTime != null)
            {
                if (strDateTime.Length > 22 && (strDateTime[21] == '+' || strDateTime[21] == '-' || strDateTime[21] == ':'))
                    return ManagementDateTimeConverter.ToDateTime(strDateTime);
                else
                {
                    //DateTime result;
                    if (DateTime.TryParse(strDateTime, out result))
                        return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Convert wmi value data into .net friendly values.
        /// </summary>
        /// <param name="wmivalue">raw wmi type.</param>
        /// <param name="myProp"></param>
        /// <returns></returns>
        internal static object ConvertObject(object wmivalue, clsMyProperty myProp) // MyTypeInfoEnum detailinfo, bool nullable = false)
        {
            //TODO: mayby change backup function prameters to before with not whole clsMyProperty??
            switch (myProp.DetailInfo)
            {
                case MyTypeInfoEnum.Guid:
                    return ConvertGuid(wmivalue);
                case MyTypeInfoEnum.DateTime:
                    return ConvertDateTime(wmivalue, myProp.CustomFormat);
                    //return ManagementDateTimeConverter.ToDateTime(wmivalue.ToString());
                case MyTypeInfoEnum.TimeSpan:
                    return ManagementDateTimeConverter.ToTimeSpan(wmivalue.ToString());
                case MyTypeInfoEnum.Version:
                    return ConvertVersion(wmivalue);
                case MyTypeInfoEnum.Enum:
                    return ConvertEnum(wmivalue, myProp);
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
                //var o = ConvertObject(obj, prop.DetailInfo, prop.IsNullable);
                var o = ConvertObject(obj, prop);

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
                //var o = ConvertObject(obj, prop.DetailInfo, prop.IsNullable);
                var o = ConvertObject(obj, prop);

                // add
                list.Add(o);
            }

            return list;
        }
    }
}
