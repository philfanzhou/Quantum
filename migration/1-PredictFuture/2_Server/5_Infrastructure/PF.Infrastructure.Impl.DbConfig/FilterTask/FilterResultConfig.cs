namespace PF.Infrastructure.Impl.DbConfig
{
    using PF.Domain.FilterTasks.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class FilterResultConfig : EntityTypeConfiguration<FilterResult>
    {
        public FilterResultConfig()
        {
            Map(m => m.MapInheritedProperties());
            Ignore(r => r.ConditionResult);
        }
    }
}
