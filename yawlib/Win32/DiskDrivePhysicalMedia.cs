using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace yawlib.Win32
{
    [WmiClassName("Win32_DiskDrivePhysicalMedia")]
    public class DiskDrivePhysicalMedia : IWmiParseable
    {
        public string Antecedent { get; set; }
        public string Dependent { get; set; }

        IWmiParseable IWmiParseable.Parse(ManagementBaseObject mba)
        {
            return new DiskDrivePhysicalMedia()
            {
                Antecedent = mba.GetPropertyValue(nameof(Antecedent)) as string,
                Dependent = mba.GetPropertyValue(nameof(Dependent)) as string,
            };
        }
    }
}
