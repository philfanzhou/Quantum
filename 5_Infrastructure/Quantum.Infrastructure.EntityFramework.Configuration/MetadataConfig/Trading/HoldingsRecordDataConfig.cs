using Quantum.Infrastructure.Trading.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.EntityFramework.Configuration
{
    internal class HoldingsRecordDataConfig : EntityTypeConfiguration<HoldingsRecordData>
    {
        public HoldingsRecordDataConfig()
        {
            this.HasKey(data => data.Id);

            this.Property(data => data.AccountId)
                .IsRequired();

            this.Property(data => data.StockCode)
                .IsRequired();

            this.Property(data => data.Quantity)
                .IsRequired();
        }
    }
}
