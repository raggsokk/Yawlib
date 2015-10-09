using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;

namespace yawlib.Win32
{
    [WmiClassName("Win32_DiskPartition")]
    public class DiskPartition
    {
        public string DeviceID { get; set; }
        public UInt32 BlockSize { get; set; }
        public bool Bootable { get; set; }
        public bool BootPartition { get; set; }
        public string Caption { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public UInt32 DiskIndex { get; set; }
        public UInt32 Index { get; set; }
        public string Name { get; set; }
        public UInt64 NumberOfBlocks { get; set; }
        public bool PrimaryPartition { get; set; }
        public UInt64 Size { get; set; }
        public UInt64 StartingOffset { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public string Type { get; set; }
    }
}
