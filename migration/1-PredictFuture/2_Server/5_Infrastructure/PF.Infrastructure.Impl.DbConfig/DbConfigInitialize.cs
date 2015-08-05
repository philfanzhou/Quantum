using Core.Domain;
using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Impl.Repository.EntityFramework;

namespace PF.Infrastructure.Impl.DbConfig
{
    public class DbConfigInitialize
    {
        public static void Init()
        {
            // 映射依赖具体技术的RepositoryContext
            ContainerHelper.Instance.RegisterType<IRepositoryContext, EntityFrameworkRepositoryContext<PfDbContext>>();
        }
    }
}
