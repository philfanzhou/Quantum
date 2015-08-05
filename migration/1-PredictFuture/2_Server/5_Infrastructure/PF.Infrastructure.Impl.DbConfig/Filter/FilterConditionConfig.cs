using System.Data.Entity.ModelConfiguration;
using PF.Domain.FilterConditions.Entities;

namespace PF.Infrastructure.Impl.DbConfig
{
    public class FilterConditionConfig : EntityTypeConfiguration<FilterCondition>
    {
        public FilterConditionConfig()
        {
            Map(m => m.MapInheritedProperties());
            HasRequired(fc => fc.Expression).WithRequiredPrincipal().WillCascadeOnDelete(true);
            Ignore(fc => fc.Result);
        }
    }
}
