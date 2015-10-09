using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yawlib.Magic
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

        Bool,
        Int8,
        Int16,
        Int32,
        Int64,
        UInt8,
        UInt16,
        UInt32,
        UInt64,
        Char,
        Float,
        Double,
    }
}
