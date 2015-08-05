namespace Core.Infrastructure.Impl.Repository.EntityFramework
{
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;

    [DbConfigurationType(typeof(SqlServerDbConfiguration))] 
    public class SqlServerDbContext : DbContext
    {
        public SqlServerDbContext(DbConnection dbConnection)
            : base(dbConnection.ToString())
        {
        }
    }

    public class SqlServerDbConfiguration : DbConfiguration
    {
        public SqlServerDbConfiguration()
        {
            SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
            //SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => (IDbExecutionStrategy) new DefaultExecutionStrategy());

            //SetDefaultConnectionFactory(
            //    new SqlConnectionFactory(
            //       @"Server=(localdb)\projects; Database=UserContextDb;Trusted_Connection=true"));
        }
    }
}
