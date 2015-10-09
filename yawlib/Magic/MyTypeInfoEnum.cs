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
    //TODO: Find a better name for this enum.
    internal enum MyTypeInfoEnum
    {        
        Invalid, // default when not set.

        // Actually a string.
        String,

        Guid,
        DateTime,
        TimeSpan,
    }
}
