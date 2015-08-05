namespace PF.Infrastructure.Impl.DbConfig
{
    using PF.Domain.FilterTasks.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class DraftFilterTaskConfig : EntityTypeConfiguration<DraftFilterTask>
    {
        public DraftFilterTaskConfig()
        {
            Map(m => m.Requires("Type").HasValue("DraftFilterTask"));
        }
    }
}
