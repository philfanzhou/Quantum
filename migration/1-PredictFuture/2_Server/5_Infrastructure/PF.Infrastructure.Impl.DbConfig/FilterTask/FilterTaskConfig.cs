namespace PF.Infrastructure.Impl.DbConfig
{
    using PF.Domain.FilterTasks.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class FilterTaskConfig : EntityTypeConfiguration<FilterTask>
    {
        public FilterTaskConfig()
        {
            Map(m => m.MapInheritedProperties());
            HasOptional(t => t.Condition).WithMany().HasForeignKey(t => t.ConditionId);
            HasOptional(t => t.Result).WithOptionalPrincipal().WillCascadeOnDelete(true);
        }
    }
}