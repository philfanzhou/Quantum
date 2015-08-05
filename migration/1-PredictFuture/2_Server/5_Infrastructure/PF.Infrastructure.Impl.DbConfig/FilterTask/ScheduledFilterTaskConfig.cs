namespace PF.Infrastructure.Impl.DbConfig
{
    using PF.Domain.FilterTasks.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class ScheduledFilterTaskConfig : EntityTypeConfiguration<ScheduledFilterTask>
    {
        public ScheduledFilterTaskConfig()
        {
            Map(m => { m.Requires("Type").HasValue("ScheduledFilterTask"); m.Properties(t => t.Schedule); m.ToTable("FilterSchedule"); });
        }
    }
}
