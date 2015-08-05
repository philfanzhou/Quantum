using System;
using System.Runtime.Serialization;

namespace Core.DistributedServices.WCF
{
    [DataContract]
    public class SubscribeResult
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}
