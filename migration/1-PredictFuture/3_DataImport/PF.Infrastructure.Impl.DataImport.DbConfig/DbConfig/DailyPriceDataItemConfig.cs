using System.Data.Entity.ModelConfiguration;
using PF.DataImport.Domain;

namespace PF.Infrastructure.Impl.DataImport.DbConfig
{
    public class DailyPriceDataItemConfig : EntityTypeConfiguration<DailyPriceDataItem>
    {
        public DailyPriceDataItemConfig()
        {
            HasKey(i => new { i.StockId, i.Date });
            Property(i => i.StockId).IsRequired().HasMaxLength(36);
            HasRequired(i => i.Stock).WithMany().HasForeignKey(i => i.StockId);
            Ignore(i => i.Id);
        }
    }
}
