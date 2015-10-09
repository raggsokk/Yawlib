using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;

namespace yawlib.Win32
{
    [WmiClassName("Win32_DiskDriveToDiskPartition")]
    public class DiskDriveToDiskPartition
    {
        public string Antecedent { get; set; }
        public string Dependent { get; set; }

    }
}
