using Core.Domain;
using PF.Application.Dto.FilterTask;
using PF.Domain.FilterEngine;
using PF.Domain.FilterTasks;

namespace PF.Application.StockFilter
{
    public class FilterEngineAppService
    {
        private readonly Engine _engine;

        public FilterEngineAppService()
        {
            _engine = Engine.Instance;
        }

        public void StartEngine()
        {
            Engine.StartEngine(new FilterTaskService());
        }

        public void StopEngine()
        {
            Engine.StopEngine();
        }

        public void ExecFilterTaskImmediatly(DraftFilterTaskDto draftFilterTaskDto)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                var filtertask = repository.Get(draftFilterTaskDto.Id);
                _engine.ExecFilterTaskImmediatly(filtertask);
            }
        }

        public void ExecFilterTaskImmediatly(ScheduledFilterTaskDto scheduledFilterTaskDto)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<ScheduledFilterTaskRepository>();
                var filtertask = repository.Get(scheduledFilterTaskDto.Id);
                _engine.ExecFilterTaskImmediatly(filtertask);
            }
        }
    }
}
