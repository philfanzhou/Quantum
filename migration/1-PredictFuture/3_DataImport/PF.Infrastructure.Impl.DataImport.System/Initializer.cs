using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Infrastructure.Impl.DataImport.System
{
    using Core.Domain;
    using Core.Infrastructure.Crosscutting;
    using Core.Infrastructure.Impl.Container;
    using Core.Infrastructure.Impl.Crosscutting;
    using Core.Infrastructure.Impl.EntityVlidator;
    using Core.Infrastructure.Impl.Repository.EntityFramework;
    using Core.Infrastructure.Impl.TypeAdapter;
    using Microsoft.Practices.Unity;
    using PF.Infrastructure.Impl.DataImport.DbConfig;

    public class Initializer
    {
        public static void Initialize()
        {
            //系统内有且仅有一次，指定IOC容器
            MyUnityContainer myContainer = new MyUnityContainer();

            // 将基础结构层的类型与接口进行映射
            ContainerHelper.SetInstance(myContainer);
            IUnityContainer unityContainer = myContainer.UnityContainer;
            unityContainer.RegisterType<IEntityValidator, DataAnnotationsEntityValidator>();
            unityContainer.RegisterType<ITypeAdapter, AutomapperTypeAdapter>();
            unityContainer.RegisterType<IIdentityGenerator, IdentityGenerator>();
            unityContainer.RegisterType<IMd5Encryptor, Md5Encryptor>();

            // 映射依赖具体技术的RepositoryContext
            unityContainer.RegisterType<IRepositoryContext, EntityFrameworkRepositoryContext<StockDataDbContext>>();
        }
    }
}
