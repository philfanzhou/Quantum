namespace Core.Infrastructure.Impl.Repository.EntityFramework
{
    using MySql.Data.Entity;
    using System.Data.Entity;

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbConnection dbConnection)
            : base(dbConnection.ToString())
        {
        }
    }
}
