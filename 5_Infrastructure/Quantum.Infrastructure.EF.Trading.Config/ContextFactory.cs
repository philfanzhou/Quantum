using Framework.Infrastructure.Repository;
using Framework.Infrastructure.Repository.EF;

namespace Quantum.Infrastructure.EF.Trading.Config
{
    public class ContextFactory
    {
        public static IRepositoryContext Create(string fullPath)
        {
            var dbContext = new TradingContext(fullPath);
            dbContext.Database.CreateIfNotExists();
            var repositoryContext = new EntityFrameworkRepositoryContext<TradingContext>(dbContext);
            return repositoryContext;
        }
    }
}
