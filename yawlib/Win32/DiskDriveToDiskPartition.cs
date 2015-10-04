using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;

namespace yawlib.Win32
{
    [WmiClassName("Win32_DiskDriveToDiskPartition")]
    public class DiskDriveToDiskPartition : IWmiParseable
    {
        public string Antecedent { get; set; }
        public string Dependent { get; set; }

        IWmiParseable IWmiParseable.Parse(ManagementBaseObject mba)
        {
            return new DiskDriveToDiskPartition()
            {
                Antecedent = mba.GetPropertyValue(nameof(Antecedent)) as string,
                Dependent = mba.GetPropertyValue(nameof(Dependent)) as string
            };
        }
    }
}
