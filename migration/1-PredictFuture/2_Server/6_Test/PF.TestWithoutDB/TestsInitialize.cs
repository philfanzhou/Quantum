using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.DistributedService.Hosting;
using Core.Domain.UserContext;
using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Impl.Crosscutting;
using Core.Infrastructure.Impl.Test.DbConfig;
using Core.Infrastructure.Impl.Test.DtoAdapter;

namespace PF.Test
{
    [TestClass]
    public class TestsInitialize
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {            
            ServiceInitialize.Init();

            IContainer container = ContainerHelper.Instance;
            container.RegisterType<IUserFactory<User>, UserFactory>();

            DbConfigInitialize.Init();
            TypeMappingInitialize.Init();
        }
    }
}
