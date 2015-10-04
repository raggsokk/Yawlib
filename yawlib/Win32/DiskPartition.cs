using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;

namespace yawlib.Win32
{
    [WmiClassName("Win32_DiskPartition")]
    public class DiskPartition : IWmiParseable
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

        IWmiParseable IWmiParseable.Parse(ManagementBaseObject mba)
        {
            var part = new DiskPartition();

            foreach(var p in mba.Properties)
            {
                switch(p.Name)
                {
                    case nameof(DeviceID):
                        part.DeviceID = p.Value as string;
                        break;
                    case nameof(BlockSize):
                        part.BlockSize = (UInt32)p.Value;
                        break;
                    case nameof(Bootable):
                        part.Bootable = (bool)p.Value;
                        break;
                    case nameof(BootPartition):
                        part.BootPartition = (bool)p.Value;
                        break;
                    case nameof(Caption):
                        part.Caption = p.Value as string;
                        break;
                    case nameof(CreationClassName):
                        part.CreationClassName = p.Value as string;
                        break;
                    case nameof(Description):
                        part.Description = p.Value as string;
                        break;
                    case nameof(DiskIndex):
                        part.DiskIndex = (UInt32)p.Value;
                        break;
                    case nameof(Index):
                        part.Index = (UInt32)p.Value;
                        break;
                    case nameof(Name):
                        part.Name = p.Value as string;
                        break;
                    case nameof(NumberOfBlocks):
                        part.NumberOfBlocks = (UInt64)p.Value;
                        break;
                    case nameof(PrimaryPartition):
                        part.PrimaryPartition = (bool)p.Value;
                        break;
                    case nameof(Size):
                        part.Size = (UInt64)p.Value;
                        break;
                    case nameof(StartingOffset):
                        part.StartingOffset = (UInt64)p.Value;
                        break;
                    case nameof(SystemCreationClassName):
                        part.SystemCreationClassName = p.Value as string;
                        break;
                    case nameof(SystemName):
                        part.SystemName = p.Value as string;
                        break;
                    case nameof(Type):
                        part.Type = p.Value as string;
                        break;
                    default:
                        break;
                }
            }


            return part;
        }
    }
}
