namespace PF.Domain.FilterTasks.Entities
{
    public class ScheduledFilterTask : FilterTask
    {
        public FilterSchedule Schedule { get; private set; }

        public ScheduledFilterTask(string id, FilterSchedule schedule)
            : base(id)
        {
            Schedule = schedule;
        }

        protected ScheduledFilterTask() { }
    }
}
