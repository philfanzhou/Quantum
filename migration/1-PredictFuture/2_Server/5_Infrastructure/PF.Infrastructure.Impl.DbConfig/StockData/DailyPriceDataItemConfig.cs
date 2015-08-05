namespace PF.Infrastructure.Impl.DbConfig
{
    using PF.Domain.StockData.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class DailyPriceDataItemConfig : EntityTypeConfiguration<DailyPriceDataItem>
    {
        public DailyPriceDataItemConfig()
        {
            Map(m => m.MapInheritedProperties());
            HasKey(dpdi => new {dpdi.StockId, dpdi.Date});
            HasRequired(dpdi => dpdi.Stock).WithMany().HasForeignKey(dpdi => dpdi.StockId);
            Ignore(dpdi => dpdi.Id);
        }
    }
}