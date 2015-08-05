namespace PF.Infrastructure.Impl.DbConfig
{
    using Core.Infrastructure.Impl.Repository.EntityFramework;
    using System.Data.Entity;

    public class PfDbContext : MySqlDbContext
    {
        private static readonly DbConnection MySqlDbConnection = new DbConnection
        {
            Server = "localhost",
            Port = "3306",
            Database = "StockData",
            Uid = "root",
            Password = "123456"
        };

        public PfDbContext()
            : base(MySqlDbConnection)
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PfDbContext>());
            //Configuration.LazyLoadingEnabled = true;

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // StockData
            modelBuilder.Configurations.Add(new DailyPriceDataItemConfig());
            modelBuilder.Configurations.Add(new DividendDataItemConfig());
            modelBuilder.Configurations.Add(new StockConfig());

            modelBuilder.Configurations.Add(new FilterTaskConfig());
            modelBuilder.Configurations.Add(new FilterResultConfig());
            modelBuilder.Configurations.Add(new DraftFilterTaskConfig());
            modelBuilder.Configurations.Add(new FilterScheduleConfig());
            modelBuilder.Configurations.Add(new ScheduledFilterTaskConfig());

            modelBuilder.Configurations.Add(new FilterConditionConfig());
            modelBuilder.Configurations.Add(new FilterExpressionConfig());

            // To do: other data config
        }
    }
}

