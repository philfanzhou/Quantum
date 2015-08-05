namespace PF.Infrastructure.Impl.DbConfig
{
    using PF.Domain.StockData.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class DividendDataItemConfig : EntityTypeConfiguration<DividendDataItem>
    {
        public DividendDataItemConfig()
        {
            Map(m => m.MapInheritedProperties());
            HasKey(ddi => new { ddi.StockId, ddi.Date });
            HasRequired(ddi => ddi.Stock).WithMany().HasForeignKey(ddi => ddi.StockId);
            Ignore(ddi => ddi.Id);
        }
    }
}
