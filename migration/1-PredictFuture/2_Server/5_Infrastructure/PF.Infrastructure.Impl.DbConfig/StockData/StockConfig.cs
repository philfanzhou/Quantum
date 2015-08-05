namespace PF.Infrastructure.Impl.DbConfig
{
    using PF.Domain.StockData.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class StockConfig : EntityTypeConfiguration<Stock>
    {
        public StockConfig()
        {
            Map(m => m.MapInheritedProperties());
        }
    }
}
