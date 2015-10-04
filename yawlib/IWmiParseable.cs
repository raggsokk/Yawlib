using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;

namespace yawlib
{
    /// <summary>
    /// Denotes that this class has its own parser.
    /// </summary>
    public interface IWmiParseable
    {
        IWmiParseable Parse(ManagementBaseObject mba);
    }
}
