namespace PF.Domain.FilterTasks
{
    using Core.Domain;
    using PF.Domain.FilterTasks.Entities;
    using System;
    using System.Collections.Generic;

    public class FilterTaskService
    {
        /// <summary>
        /// 根据时间区间获取待执行任务
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<ScheduledFilterTask> QueryFilterTasksByTime(DateTime start, DateTime end)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<ScheduledFilterTaskRepository>();
                return repository.GetTasks(start, end);
            }
        }

        /// <summary>
        /// 执行过滤任务
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="filterTask"></param>
        public void ExecFilterTask<TTask>(TTask filterTask) where TTask : FilterTask
        {
            filterTask.ApplyCondition();
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="filterTask"></param>
        /// <param name="status"></param>
        public void UpdateFilterTaskStatus<TTask>(TTask filterTask, FilterTaskStatus status) where TTask : FilterTask
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<FilterTaskRepository<TTask>>();
                repository.UpdateFilterTaskStatus(filterTask, status);
                context.UnitOfWork.Commit();
            }
        }

        /// <summary>
        /// 更新任务结果
        /// </summary>
        /// <param name="filterTask"></param>
        /// <param name="result"></param>
        public void UpdateFilterTaskResult<TTask>(TTask filterTask, FilterResult result) where TTask : FilterTask
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<FilterTaskRepository<TTask>>();
                repository.UpdateFilterTaskResult(filterTask, result);
                context.UnitOfWork.Commit();
            }
        }
    }
}
