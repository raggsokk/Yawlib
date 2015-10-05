using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Diagnostics;

using yawlib.Magic;

namespace yawlib
{
    public abstract class WmiParsable : IWmiParseable
    {

        public IWmiParseable Parse2(ManagementBaseObject mba)
        {
            return (this as IWmiParseable).Parse(mba);
        }

        IWmiParseable IWmiParseable.Parse(ManagementBaseObject mba)
        {
            //if(mba.Properties.Count == 0)
            //    return new 
            var objType = this.GetType();
            var myType = Reflection.Instance.TryGetMyType(objType.FullName, objType);

            var instance = myType.CreateObject();

            foreach(var p in mba.Properties)
            {
                if (!p.IsArray)
                {
                    clsMyPropery myprop = null;

                    if (myType.WmiProperties.TryGetValue(p.Name, out myprop))
                    {
                        instance = myprop.GenericSetter(instance, p.Value);
                    }
                }
                else
                    throw new NotImplementedException("Not implemented Generic mbo.IsArray handling!");
            }

            return (IWmiParseable)instance;            
        }

        internal List<WmiParsable> Parse(ManagementObjectCollection data)
        {
            //TODO: use TypeSystem directly to speed up this process.

            var list = new List<WmiParsable>();

            var p = this as IWmiParseable;

            foreach(var w in data)
            {
                var c = (WmiParsable)p.Parse(w);

                list.Add(c);
            }

            return list;
        }
    }
}
