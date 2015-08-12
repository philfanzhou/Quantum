using Framework.Infrastructure.Repository.EntityFramework;
using System.Data.Entity;

namespace Quantum.Infrastructure.EntityFramework.Configuration
{
    internal sealed class QuantumDbContext : SqlServerDbContext
    {
        private static readonly ConnectionConfig sqlServer = new ConnectionConfig
        {
            Server = @"(localdb)\projects",
            Database = "Trading",
            TrustedConnection = true
        };

        //private static readonly ConnectionConfig mySql = new ConnectionConfig
        //{
        //    Server = "localhost",
        //    Port = "3306",
        //    Database = "UserContextDb",
        //    Uid = "root",
        //    Password = "123456"
        //};

        public QuantumDbContext()
            //: base(mySql)
            : base(sqlServer)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Trading Models
            modelBuilder.Configurations.Add(new AccountDataConfig());
            modelBuilder.Configurations.Add(new HoldingsRecordDataConfig());
            modelBuilder.Configurations.Add(new TradingRecordDataConfig());

            // TO DO : Add others
            //modelBuilder.Configurations.Add(new OtherConfigType());
        }
    }
}
