using System.Data.Entity.ModelConfiguration;

namespace Quantum.Infrastructure.EF.Trading.Config
{
    internal class TradingRecordConfig : EntityTypeConfiguration<TradingRecordDbo>
    {
        public TradingRecordConfig()
        {
            this.Property(p => p.AccountId).IsRequired();
            this.Property(p => p.StockCode).IsRequired().HasMaxLength(10);
            this.Property(p => p.Price).IsRequired();
            this.Property(p => p.Quantity).IsRequired();
            this.Property(p => p.Date).IsRequired();
            this.Property(p => p.TradeType).IsRequired().HasMaxLength(10);
            this.Property(p => p.Commissions).IsRequired();
            this.Property(p => p.FeesSettlement).IsRequired();
            this.Property(p => p.StampDuty).IsRequired();
            this.Property(p => p.TransferFees).IsRequired();
        }
    }
}
