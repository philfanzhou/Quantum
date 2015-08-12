using Quantum.Infrastructure.Trading.Repository;
using System.Data.Entity.ModelConfiguration;

namespace Quantum.Infrastructure.EntityFramework.Configuration
{
    internal class TradingRecordDataConfig : EntityTypeConfiguration<TradingRecordData>
    {
        public TradingRecordDataConfig()
        {
            this.HasKey(data => data.Id);

            this.Property(data => data.AccountId)
                .IsRequired();

            this.Property(data => data.Date)
                .IsRequired();

            this.Property(data => data.Type)
                .IsRequired();

            this.Property(data => data.StockCode)
                .IsRequired();

            this.Property(data => data.Quantity)
                .IsRequired();

            this.Property(data => data.Price)
                .IsRequired();

            this.Property(data => data.Commissions)
                .IsRequired();

            this.Property(data => data.StampDuty)
                .IsRequired();

            this.Property(data => data.TransferFees)
                .IsRequired();

            this.Property(data => data.FeesSettlement)
                .IsRequired();
        }
    }
}
