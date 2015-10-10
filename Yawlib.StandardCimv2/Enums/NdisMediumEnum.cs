using System;

namespace Yawlib.StandardCimv2
{
    public enum NdisMediumEnum
    {
        /// <summary>
        /// Ethernet
        /// </summary>
        Ieee802_3 = 0,
        /// <summary>
        /// Token Ring
        /// </summary>
        Ieee802_5 = 1,
        Fddi = 2,
        Wan = 3,
        LocalTalk = 4,
        Dix = 5,
        RawArcnet = 6,
        /// <summary>
        /// Arcnet
        /// </summary>
        Ieee878_2 = 7,
        Atm = 8,
        WirelessWan = 9,
        Irda = 10,
        Bpc = 11,
        ConnectionOrientedWan = 12,
        /// <summary>
        /// Firewire
        /// </summary>
        Ip1394 = 13,
        Ib = 14,
        Tunnel = 15,
        Native802_11 = 16,
        Loopback = 17,
        Wimax = 18,
        Ip = 19,

    }
}
