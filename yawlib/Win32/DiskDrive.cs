using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace yawlib.Win32
{
    [WmiClassName("Win32_DiskDrive")]
    public class DiskDrive : IWmiParseable
    {
        public string DeviceID { get; set; }
        public UInt32 BytesPerSector { get; set; }
        // Capabilities
        // CapabilityDescriptors.
        public string Caption { get; set; }
        public UInt32 ConfigManagerErrorCode { get; set; }
        public bool ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public string FirmwareRevision { get; set; }
        public UInt32 Index { get; set; }
        public string InterfaceType { get; set; }
        public string Manufacturer { get; set; }
        public bool MediaLoaded { get; set; }
        public string MediaType { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public UInt32 Partitions { get; set; }
        public string PNPDeviceID { get; set; }
        public UInt32 SCSIBus { get; set; }
        public UInt16 SCSILogicalUnit { get; set; }
        public UInt16 SCSIPort { get; set; }
        public UInt16 SCSITargetId { get; set; }
        public UInt32 SectorsPerTrack { get; set; }
        public string SerialNumber { get; set; }
        public UInt32 Signature { get; set; }
        public UInt64 Size { get; set; }
        public string Status { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public UInt64 TotalCylinders { get; set; }
        public UInt32 TotalHeads { get; set; }
        public UInt64 TotalSectors { get; set; }
        public UInt64 TotalTracks { get; set; }
        public UInt32 TracksPerCylinder { get; set; }

        IWmiParseable IWmiParseable.Parse(ManagementBaseObject mba)
        {
            var disk = new DiskDrive();
            
            foreach(var p in mba.Properties)
            {
                switch(p.Name)
                {
                    case nameof(DeviceID):
                        disk.DeviceID = p.Value as string;
                        break;
                    case nameof(BytesPerSector):
                        disk.BytesPerSector = (UInt32)p.Value;
                        break;
                    case nameof(Caption):
                        disk.Caption = p.Value as string;
                        break;
                    case nameof(ConfigManagerErrorCode):
                        disk.ConfigManagerErrorCode = (UInt32)p.Value;
                        break;
                    case nameof(ConfigManagerUserConfig):
                        disk.ConfigManagerUserConfig = (bool)p.Value;
                        break;
                    case nameof(CreationClassName):
                        disk.CreationClassName = p.Value as string;
                        break;
                    case nameof(Description):
                        disk.Description = p.Value as string;
                        break;
                    case nameof(FirmwareRevision):
                        disk.FirmwareRevision = p.Value as string;
                        break;
                    case nameof(Index):
                        disk.Index = (UInt32)p.Value;
                        break;
                    case nameof(InterfaceType):
                        disk.InterfaceType = p.Value as string;
                        break;
                    case nameof(Manufacturer):
                        disk.Manufacturer = p.Value as string;
                        break;
                    case nameof(MediaLoaded):
                        disk.MediaLoaded = (bool)p.Value;
                        break;
                    case nameof(MediaType):
                        disk.MediaType = p.Value as string;
                        break;
                    case nameof(Model):
                        disk.Model = p.Value as string;
                        break;
                    case nameof(Name):
                        disk.Name = p.Value as string;
                        break;
                    case nameof(Partitions):
                        disk.Partitions = (UInt32)p.Value;
                        break;
                    case nameof(PNPDeviceID):
                        disk.PNPDeviceID = p.Value as string;
                        break;
                    case nameof(SCSIBus):
                        disk.SCSIBus = (UInt32)p.Value;
                        break;
                    case nameof(SCSILogicalUnit):
                        disk.SCSILogicalUnit = (UInt16)p.Value;
                        break;
                    case nameof(SCSIPort):
                        disk.SCSIPort = (UInt16)p.Value;
                        break;
                    case nameof(SCSITargetId):
                        disk.SCSITargetId = (UInt16)p.Value;
                        break;
                    case nameof(SectorsPerTrack):
                        disk.SectorsPerTrack = (UInt32)p.Value;
                        break;
                    case nameof(SerialNumber):
                        disk.SerialNumber = p.Value as string;
                        break;
                    case nameof(Signature):
                        disk.Signature = (UInt32)p.Value;
                        break;
                    case nameof(Size):
                        disk.Size = (UInt64)p.Value;
                        break;
                    case nameof(Status):
                        disk.Status = p.Value as string;
                        break;
                    case nameof(SystemCreationClassName):
                        disk.SystemCreationClassName = p.Value as string;
                        break;
                    case nameof(SystemName):
                        disk.SystemName = p.Value as string;
                        break;
                    case nameof(TotalCylinders):
                        disk.TotalCylinders = (UInt64)p.Value;
                        break;
                    case nameof(TotalHeads):
                        disk.TotalHeads = (UInt32)p.Value;
                        break;
                    case nameof(TotalSectors):
                        disk.TotalSectors = (UInt64)p.Value;
                        break;
                    case nameof(TotalTracks):
                        disk.TotalTracks = (UInt64)p.Value;
                        break;
                    case nameof(TracksPerCylinder):
                        disk.TracksPerCylinder = (UInt32)p.Value;
                        break;

                    default:
                        break;
                }
            }

            return disk;
        }
    }
}
