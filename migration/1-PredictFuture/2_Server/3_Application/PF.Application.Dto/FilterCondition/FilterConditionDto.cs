using PF.Application.Dto.Indicator;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PF.Application.Dto.FilterCondition
{
    [DataContract]
    public class FilterConditionDto
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public DateTime CutoffTime { get; set; }

        [DataMember]
        public string ExpressionString { get; set; }

        [DataMember]
        public List<IndicatorDto> Indicators { get; set; }
    }
}
