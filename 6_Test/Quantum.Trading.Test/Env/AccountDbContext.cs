using Framework.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Trading.Test.Env
{
    public sealed class AccountDbContext : SqlServerDbContext
    {
        private static readonly DbConnection sqlServer = new DbConnection
        {
            Server = @"(localdb)\projects",
            Database = "Trading",
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

        public AccountDbContext()
            //: base(mySql)
            : base(sqlServer)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountDataConfig());
        }
    }
}
