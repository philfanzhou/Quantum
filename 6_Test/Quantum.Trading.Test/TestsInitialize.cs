using Framework.Infrastructure.Container;
using Framework.Infrastructure.Repository;
using Framework.Infrastructure.Repository.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Trading.Test.Env;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Trading.Test
{
    [TestClass]
    public class TestsInitialize
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            // 映射依赖具体技术的RepositoryContext
            ContainerHelper.Instance.RegisterType<IRepositoryContext, EntityFrameworkRepositoryContext<AccountDbContext>>();

            /***************testCode*************************/
            AccountDbContext dbContext = new AccountDbContext();
            dbContext.Database.Delete();
            dbContext.Database.Create();
            /***************testCode*************************/
        }
    }
}
