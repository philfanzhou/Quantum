using PF.Domain.FilterConditions.Entities;
using System.Data.Entity.ModelConfiguration;

namespace PF.Infrastructure.Impl.DbConfig
{
    public class FilterExpressionConfig : EntityTypeConfiguration<FilterExpression>
    {
        public FilterExpressionConfig()
        {
            Map(m => m.MapInheritedProperties());
            Ignore(fe => fe.Evaluator);
            Ignore(fe => fe.Indicators);
        }
    }
}
