namespace PF.Infrastructure.Impl.DbConfig
{
    using PF.Domain.FilterTasks.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class FilterScheduleConfig : ComplexTypeConfiguration<FilterSchedule>
    {
        public FilterScheduleConfig()
        {
            Property(s => s.ExecTime).HasColumnName("ExecTime");
            Property(s => s.Repeat).HasColumnName("Repeat");
        }
    }
}
