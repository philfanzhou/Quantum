using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Impl.Crosscutting;
using PF.Infrastructure.Impl.DbConfig;
using PF.Infrastructure.Impl.DtoAdapter;
using Core.Domain.UserContext;
using LocalDbConfig = Core.Infrastructure.Impl.Test.DbConfig;
using LocalDbAdapter = Core.Infrastructure.Impl.Test.DtoAdapter;

namespace PF.DistributedService.Hosting
{
    public class ServiceInitialize
    {
        public static void Init()
        {
            SystemInitialize.SystemInitializationEvent += Initializer_SystemInitializationEvent;
            SystemInitialize.Init();
        }

        static void Initializer_SystemInitializationEvent()
        {
            DbConfigInitialize.Init();
            TypeMappingInitialize.Init();

            IContainer container = ContainerHelper.Instance;
            container.RegisterType<IUserFactory<User>, UserFactory>();

            //LocalDbConfig.DbConfigInitialize.Init();
            //LocalDbAdapter.TypeMappingInitialize.Init();
        }
    }
}
