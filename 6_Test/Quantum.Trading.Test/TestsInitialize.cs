using Framework.Infrastructure.Repository.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Infrastructure.EntityFramework.Configuration;

namespace Quantum.Trading.Test
{
    [TestClass]
    public class TestsInitialize
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            // 映射依赖具体技术的RepositoryContext
            DatabaseHelper.RegistDbContext<QuantumDbContext>();

            /***************testCode*************************/
            QuantumDbContext dbContext = new QuantumDbContext();
            DatabaseHelper.RebuildDatabase(dbContext);
            /***************testCode*************************/
        }
    }
}
