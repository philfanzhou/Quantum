using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.DistributedService.Hosting;
using PF.Infrastructure.Impl.DbConfig;

namespace PF.Test
{
    [TestClass]
    public class TestsInitialize
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            using (var ctx = new PfDbContext())
            {
                if (ctx.Database.Exists())
                {
                    ctx.Database.Delete();
                }
                ctx.Database.Create();
            }


            ServiceInitialize.Init();
        }
    }
}
