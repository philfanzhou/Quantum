using Framework.Infrastructure.Container;
using System.Data.Entity;

namespace Framework.Infrastructure.Repository.EntityFramework
{
    public static class DbContextHelper
    {
        public static void RegisterDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            ContainerHelper.Instance
                .RegisterType<IRepositoryContext,
                EntityFrameworkRepositoryContext<TDbContext>>();
        }
    }
}
