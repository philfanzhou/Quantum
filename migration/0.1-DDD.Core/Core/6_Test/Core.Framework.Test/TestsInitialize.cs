using Core.Domain.UserContext;
using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Impl.Crosscutting;
using Core.Infrastructure.Impl.Test.DbConfig;
using Core.Infrastructure.Impl.Test.DtoAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extension.Application.UserContext.Test
{
    [TestClass]
    public class TestsInitialize
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            SystemInitialize.SystemInitializationEvent += Initializer_SystemInitializationEvent;
            SystemInitialize.Init();
        }

        static void Initializer_SystemInitializationEvent()
        {
            IContainer container = ContainerHelper.Instance;
            container.RegisterType<IUserFactory<User>, UserFactory>();

            DbConfigInitialize.Init();
            TypeMappingInitialize.Init();
        }
    }
}
