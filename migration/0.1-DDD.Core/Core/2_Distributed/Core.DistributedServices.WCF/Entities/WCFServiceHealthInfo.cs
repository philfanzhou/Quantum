using System;
using System.Runtime.Serialization;

namespace Core.DistributedServices.WCF
{
    [DataContract]
    public class WCFServiceHealthInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public ServiceStatus Status { get; set; }
        [DataMember]
        public DateTime? StartedTime { get; set; }
    }
}
