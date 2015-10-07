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

        public clsMyPropery(PropertyInfo p)
        {
            this.Name = p.Name;

            //TODO: Implement Wmi Property mapper attribute;
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
