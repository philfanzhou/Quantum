namespace PF.Domain.FilterTasks
{
    using Core.Domain;
    using PF.Domain.FilterTasks.Entities;

    public class FilterTaskRepository<TTask> : Repository<TTask> where TTask : FilterTask
    {
        public FilterTaskRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public virtual void UpdateFilterTask(TTask filterTask)
        {
            var task = Get(filterTask.Id);
            task.Name = filterTask.Name;
            task.Condition.Name = filterTask.Condition.Name;
            task.Condition.Description = filterTask.Condition.Description;
            task.Condition.CutoffTime = filterTask.Condition.CutoffTime;
            task.Condition.UpdateExpression(filterTask.Condition.Expression);
        }

        public void UpdateFilterTaskStatus(TTask filterTask, FilterTaskStatus status)
        {
            var task = Get(filterTask.Id);
            task.Status = status;
        }

        public void UpdateFilterTaskResult(TTask filterTask, FilterResult result)
        {
            var task = Get(filterTask.Id);
            task.UpdateResult(result);
        }
    }
}
