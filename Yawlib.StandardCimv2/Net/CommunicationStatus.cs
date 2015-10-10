using System;

namespace Yawlib.StandardCimv2
{
    public enum CommunicationStatus
    {
        Unknown = 0,
        NotAvailable = 1,
        CommunicationOk = 2,
        LostCommunication = 3,
        NoContact = 4,
    }
}
