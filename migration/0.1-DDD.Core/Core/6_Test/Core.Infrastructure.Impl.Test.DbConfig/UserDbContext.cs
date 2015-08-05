namespace Core.Infrastructure.Impl.Test.DbConfig
{
    using Core.Domain.UserContext;
    using Core.Infrastructure.Impl.Repository.EntityFramework;
    using System.Data.Entity;

    //public sealed class UserDbContext : MySqlDbContext
    public sealed class UserDbContext : SqlServerDbContext
    {
        private static readonly DbConnection sqlServer = new DbConnection
        {
            Server = @"(localdb)\projects",
            Database = "UserContextDb",
            TrustedConnection = true
        };

        private static readonly DbConnection mySql = new DbConnection
        {
            Server = "localhost",
            Port = "3306",
            Database = "UserContextDb",
            Uid = "root",
            Password = "123456"
        };

        public UserDbContext()
            //: base(mySql)
            : base(sqlServer)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfig<User>());
            modelBuilder.Configurations.Add(new RoleConfig());
            modelBuilder.Configurations.Add(new PermissionConfig());
            modelBuilder.Configurations.Add(new UserRoleConfig());
        }
    }
}
