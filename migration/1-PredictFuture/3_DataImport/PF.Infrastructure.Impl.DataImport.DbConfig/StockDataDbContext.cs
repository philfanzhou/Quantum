using MySql.Data.Entity;
using System.Configuration;
using System.Data.Entity;

namespace PF.Infrastructure.Impl.DataImport.DbConfig
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class StockDataDbContext : DbContext
    {
        private static readonly string _sqlConnection = ConfigurationManager.ConnectionStrings["StockDataContext"].ConnectionString;

        public StockDataDbContext()
            : base(_sqlConnection)
        {
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StockConfig());
            modelBuilder.Configurations.Add(new StockBaseConfig());
            modelBuilder.Configurations.Add(new DailyPriceDataItemConfig());
            modelBuilder.Configurations.Add(new DividendDataItemConfig());
        }
    }
}
