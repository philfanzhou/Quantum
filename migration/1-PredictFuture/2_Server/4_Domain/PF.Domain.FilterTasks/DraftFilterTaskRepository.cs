namespace PF.Domain.FilterTasks
{
    using Core.Domain;
    using PF.Domain.FilterTasks.Entities;

    public class DraftFilterTaskRepository : FilterTaskRepository<DraftFilterTask>
    {
        public DraftFilterTaskRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
