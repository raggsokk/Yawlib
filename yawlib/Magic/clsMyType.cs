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

    }
}
