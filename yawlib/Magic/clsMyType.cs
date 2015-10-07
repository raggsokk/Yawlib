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


        public SortedDictionary<string, clsMyPropery> WmiProperties = new SortedDictionary<string, clsMyPropery>();

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
                var myprop = new clsMyPropery(p);

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

                    clsMyPropery myprop = null;

                    if (WmiProperties.TryGetValue(p.Name, out myprop))
                    {
                        object oset = null;

                        // convert.
                        switch (p.Type)
                        {
                            //case CimType.String:
                            //    //TODO: handle when string is actually GUID, UUID, IPAddress, etc.
                            //    oset = p.Value; // no conversion.                                
                            //    break;
                            case CimType.UInt8:
                                oset = (byte)p.Value;
                                break;
                            case CimType.UInt16:
                                oset = (UInt16)p.Value;
                                break;
                            case CimType.UInt32:
                                oset = (UInt32)p.Value;
                                break;
                            case CimType.UInt64:
                                oset = (UInt64)p.Value;
                                break;
                            case CimType.Boolean:
                                oset = (bool)p.Value;
                                break;
                            case CimType.DateTime:
                                oset = (DateTime)p.Value;
                                break;
                            default:

                                // try to use clsMyType.
                                switch(myprop.DetailInfo)
                                {
                                    case MyTypeInfoEnum.String:
                                        oset = p.Value; // no conversion.
                                        break;
                                    case MyTypeInfoEnum.Guid:
                                        Guid g;
                                        if (Guid.TryParse(p.Value as string, out g))
                                            oset = g;
                                        break;
                                    case MyTypeInfoEnum.DateTime:
                                        DateTime dt;
                                        if (DateTime.TryParse(p.Value as string, out dt))
                                            oset = dt;
                                        break;
                                    default:
                                        throw new NotSupportedException(string.Format(
                                            "Type '{0}' is not supported yet for conversion from '{1}'",
                                            myprop.RefType, p.Type));
                                }

                                break;
                        }

                        instance = myprop.GenericSetter(instance, oset);
                    }
                }

                result.Add(instance);
            }

            return true;
        }

        //internal bool Convert(ManagementObjectCollection data, System.Collections.IList result)
        //{
        //    if (data == null)
        //        throw new ArgumentNullException(string.Format("Argument cant be null. Arg='{0}', Type='{1}'", nameof(data), typeof(ManagementObjectCollection).Name));


        //    foreach(var item in data)
        //    {
        //        var instance = this.CreateObject();                

        //        foreach (var p in item.Properties)
        //        {
        //            clsMyPropery myprop = null;

        //            if(WmiProperties.TryGetValue(p.Name, out myprop))
        //            {
        //                object oset = null;

        //                // convert.
        //                switch(p.Type)
        //                {
        //                    case CimType.String:
        //                        //TODO: handle when string is actually GUID, UUID, IPAddress, etc.
        //                        oset = p.Value; // no conversion.                                
        //                        break;
        //                    case CimType.UInt8:
        //                        oset = (byte)p.Value;
        //                        break;
        //                    case CimType.UInt16:
        //                        oset = (UInt16)p.Value;
        //                        break;
        //                    case CimType.UInt32:
        //                        oset = (UInt32)p.Value;
        //                        break;
        //                    case CimType.UInt64:
        //                        oset = (UInt64)p.Value;
        //                        break;
        //                    case CimType.Boolean:
        //                        oset = (bool)p.Value;
        //                        break;
        //                    case CimType.DateTime:
        //                        oset = (DateTime)p.Value;
        //                        break;                            
        //                    default:
        //                        break;     
        //                }

        //                instance = myprop.GenericSetter(instance, oset);
        //            }
        //        }

        //        result.Add(instance);
        //    }

        //    return true;
        //}

    }
}
