using Framework.Infrastructure.Container;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Repository.EntityFramework
{
    public static class DatabaseHelper
    {
        public static void RegistDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            ContainerHelper.Instance
                .RegisterType<IRepositoryContext,
                EntityFrameworkRepositoryContext<TDbContext>>();
        }

        public static void RebuildDatabase(DbContext context)
        {
            /***************testCode*************************/
            context.Database.Delete();
            context.Database.Create();
            /***************testCode*************************/
        }
    }
}
