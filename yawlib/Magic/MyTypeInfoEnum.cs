using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yawlib.Magic
{
    /// <summary>
    /// WMI uses string as placeholder for more complex types.
    /// Suct as GUID, DateTime(Old), etc
    /// </summary>
    internal enum MyTypeInfoEnum
    {
        //TODO: Find a better name for this enum.
        Invalid,
        // Actually a string.
        String,

        Guid,
        DateTime,
    }
}
