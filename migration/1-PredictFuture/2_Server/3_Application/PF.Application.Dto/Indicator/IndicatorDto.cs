using System.Runtime.Serialization;

namespace PF.Application.Dto.Indicator
{
    [DataContract]
    public class IndicatorDto
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
