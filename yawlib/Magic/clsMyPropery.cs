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
    internal class clsMyPropery
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The WMI Property Name 
        /// </summary>
        public string WmiName { get; set; }

        // reflection
        public Reflection.GenericSetter GenericSetter { get; set; }

        public clsMyPropery(PropertyInfo p)
        {
            this.Name = p.Name;

            //TODO: Implement Wmi Property mapper attribute;
            this.WmiName = p.Name;

            this.GenericSetter = Reflection.CompileGenericSetMethod(p.DeclaringType, p);
        }
    }
}
