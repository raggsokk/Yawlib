using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace yawlib.Win32
{
    [WmiClassName("Win32_DiskDrive")]
    public class DiskDrive
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

    }
}
