using System;
using System.Runtime.Serialization;
using PF.Application.Dto.FilterCondition;

namespace PF.Application.Dto.FilterTask
{
    [DataContract]
    public abstract class FilterTaskDtoBase
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public DateTime ExecStartTime { get; set; }

        [DataMember]
        public DateTime ExecEndTime { get; set; }

        [DataMember]
        public FilterConditionDto Condition { get; set; }
    }
}