namespace PF.Domain.FilterTasks
{
    using Core.Domain;
    using PF.Domain.FilterTasks.Entities;
    using System;
    using System.Collections.Generic;

    public class ScheduledFilterTaskRepository : FilterTaskRepository<ScheduledFilterTask>
    {
        public ScheduledFilterTaskRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public override void UpdateFilterTask(ScheduledFilterTask filterTask)
        {
            base.UpdateFilterTask(filterTask);

            var task = Get(filterTask.Id);
            task.Schedule.ExecTime = filterTask.Schedule.ExecTime;
            task.Schedule.Repeat = filterTask.Schedule.Repeat;
        }

        public IEnumerable<ScheduledFilterTask> GetTasks(DateTime start, DateTime end)
        {
            var spec = Specification<ScheduledFilterTask>.Eval(t => t.Schedule.ExecTime >= start && t.Schedule.ExecTime < end);
            return GetAll(spec);
        }
    }
}
