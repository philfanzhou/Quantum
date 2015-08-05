using Core.Application;
using Core.Domain;
using Core.Infrastructure.Crosscutting;
using PF.Application.Dto.FilterTask;
using PF.Domain.FilterConditions.Entities;
using PF.Domain.FilterTasks;
using PF.Domain.FilterTasks.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PF.Application.StockFilter
{
    public class FilterTaskAppService
    {
        public IEnumerable<DraftFilterTaskDto> GetAllDraftFilterTasks(int userId)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                var tasks = repository.GetAll().ToArray();
                return tasks.Select(t => t.ProjectedAs<DraftFilterTaskDto>()).ToArray();
            }
        }

        public IEnumerable<ScheduledFilterTaskDto> GetAllScheduledFilterTasks(int userId)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<ScheduledFilterTaskRepository>();
                var tasks = repository.GetAll().ToArray();
                return tasks.Select(t => t.ProjectedAs<ScheduledFilterTaskDto>()).ToArray();
            }
        }

        public IEnumerable<string> GetFilterTaskResult(DraftFilterTaskDto draftFilterTask)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                var filtertask = repository.Get(draftFilterTask.Id);
                return filtertask.Result.ConditionResult.SelectedStocks.ToArray();
            }
        }

        public IEnumerable<string> GetFilterTaskResult(ScheduledFilterTaskDto scheduledFilterTask)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                var filtertask = repository.Get(scheduledFilterTask.Id);
                return filtertask.Result.ConditionResult.SelectedStocks.ToArray();
            }
        }

        public void AddDraftFilterTask(DraftFilterTaskDto draftFilterTask, int userId)
        {
            using (var context = RepositoryContext.Create())
            {
                var filtertask = ConvertNew(draftFilterTask, userId);
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                repository.Add(filtertask);
                context.UnitOfWork.Commit();
            }
        }

        public void AddScheduledFilterTask(ScheduledFilterTaskDto scheduledFilterTask, int userId)
        {
            using (var context = RepositoryContext.Create())
            {
                var filtertask = ConvertNew(scheduledFilterTask, userId);
                var repository = context.GetRepository<ScheduledFilterTaskRepository>();
                repository.Add(filtertask);
                context.UnitOfWork.Commit();
            }
        }

        public void UpdateDraftFilterTask(DraftFilterTaskDto draftFilterTask)
        {
            using (var context = RepositoryContext.Create())
            {
                var filtertask = draftFilterTask.ProjectedAs<DraftFilterTask>();
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                repository.UpdateFilterTask(filtertask);
                context.UnitOfWork.Commit();
            }
        }

        public void UpdateScheduledFilterTask(ScheduledFilterTaskDto scheduledFilterTask)
        {
            using (var context = RepositoryContext.Create())
            {
                var filtertask = scheduledFilterTask.ProjectedAs<ScheduledFilterTask>();
                var repository = context.GetRepository<ScheduledFilterTaskRepository>();
                repository.UpdateFilterTask(filtertask);
                context.UnitOfWork.Commit();
            }
        }

        private DraftFilterTask ConvertNew(DraftFilterTaskDto draftFilterTask, int userId)
        {
            var idgenerator = ContainerHelper.Instance.Resolve<IIdentityGenerator>();
            var filtertask = draftFilterTask.ProjectedAs<DraftFilterTask>();
            var conditionid = idgenerator.NewGuid().ToString();
            var expression = new FilterExpression(conditionid, filtertask.Condition.Expression.ExpressionString, null);
            expression.UpdateFrom(filtertask.Condition.Expression);
            return new DraftFilterTask(idgenerator.NewGuid().ToString())
                {
                    Name = filtertask.Name,
                    Condition = new FilterCondition(conditionid, userId, expression)
                        {
                            Name = filtertask.Condition.Name,
                            Description = filtertask.Condition.Description,
                            CutoffTime = filtertask.Condition.CutoffTime
                        }
                };
        }

        private ScheduledFilterTask ConvertNew(ScheduledFilterTaskDto scheduledFilterTask, int userId)
        {
            var idgenerator = ContainerHelper.Instance.Resolve<IIdentityGenerator>();
            var filtertask = scheduledFilterTask.ProjectedAs<ScheduledFilterTask>();
            var conditionid = idgenerator.NewGuid().ToString();
            var expression = new FilterExpression(conditionid, filtertask.Condition.Expression.ExpressionString, null);
            expression.UpdateFrom(filtertask.Condition.Expression);
            return new ScheduledFilterTask(idgenerator.NewGuid().ToString(), filtertask.Schedule)
                {
                    Name = filtertask.Name,
                    Condition = new FilterCondition(conditionid, userId, expression)
                        {
                            Name = filtertask.Condition.Name,
                            Description = filtertask.Condition.Description,
                            CutoffTime = filtertask.Condition.CutoffTime
                        }
                };
            }
    }
}
