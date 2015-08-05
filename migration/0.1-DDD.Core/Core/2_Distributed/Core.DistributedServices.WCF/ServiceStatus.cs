using System.Runtime.Serialization;

namespace Core.DistributedServices.WCF
{
    [DataContract]
    public enum ServiceStatus
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        Created = 1,
        [EnumMember]
        Opening = 2,
        [EnumMember]
        Opened = 3,
        [EnumMember]
        Closing = 4,
        [EnumMember]
        Closed = 5,
        [EnumMember]
        Faulted = 6
    }
}
