using System;
using System.Runtime.Serialization;

namespace Core.DistributedServices.WCF
{
    [DataContract]
    public class Subscriber
    {
        public Subscriber()
        {
            this.PreviousSubscribeId = Guid.Empty;
        }

        [DataMember]
        public Guid PreviousSubscribeId { get; set; }
    }
}
