namespace PF.Domain.FilterTasks.Entities
{
    using System;

    public class FilterSchedule
    {
        public DateTime ExecTime { get; set; }

        public FilterTaskRepeat Repeat { get; set; }
    }
}
