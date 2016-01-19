using Framework.Infrastructure.Repository.EF.SqlServerCe;
using System.Data.Entity;

namespace Quantum.Infrastructure.EF.Trading.Config
{
    internal class TradingContext : SqlCeDbContext
    {
        public TradingContext(string fullPath)
            : base(fullPath)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountConfig());
            modelBuilder.Configurations.Add(new HoldingsRecordConfig());
            modelBuilder.Configurations.Add(new TradingRecordConfig());
        }
    }
}
