namespace PF.Domain.FilterConditions
{
    using Core.Domain;
    using PF.Domain.FilterConditions.Entities;

    public class FilterConditionRepository : Repository<FilterCondition>
    {
        public FilterConditionRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}