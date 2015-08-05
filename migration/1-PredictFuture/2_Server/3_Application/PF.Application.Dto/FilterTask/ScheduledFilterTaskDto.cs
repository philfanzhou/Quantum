using System;
using System.Runtime.Serialization;

namespace PF.Application.Dto.FilterTask
{
    [DataContract]
    public class ScheduledFilterTaskDto : FilterTaskDtoBase
    {
        [DataMember]
        public DateTime ScheduleExecTime { get; set; }

        [DataMember]
        public string ScheduleRepeat { get; set; }
    }
}
