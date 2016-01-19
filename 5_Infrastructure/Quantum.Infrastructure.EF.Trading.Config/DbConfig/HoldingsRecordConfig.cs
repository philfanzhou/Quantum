using System.Data.Entity.ModelConfiguration;

namespace Quantum.Infrastructure.EF.Trading.Config
{
    internal class HoldingsRecordConfig : EntityTypeConfiguration<HoldingsRecordDbo>
    {
        public HoldingsRecordConfig()
        {
            this.Property(p => p.AccountId).IsRequired();
            this.Property(p => p.StockCode).IsRequired().HasMaxLength(10);
            this.Property(p => p.Quantity).IsRequired();
        }
    }
}
