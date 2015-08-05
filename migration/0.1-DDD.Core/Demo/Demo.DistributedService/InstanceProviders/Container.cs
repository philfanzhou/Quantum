

namespace Demo.DistributedService
{
    using Application;
    using Application.Dto;
    using AutoMapper;
    using Core.Infrastructure.Crosscutting;
    using Core.Infrastructure.Crosscutting.Implementation;
    using Domain.UserContext;
    using Extension.Application.UserContext;
    using Extension.Domain.UserContext;
    using Extension.Infrastructure.Data.UserContext;
    using Infrastructure.Data.UserContext;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// DI container accessor
    /// </summary>
    public static class Container
    {
        #region Properties

        static IUnityContainer _currentContainer;

        /// <summary>
        /// Get the current configured container
        /// </summary>
        /// <returns>Configured container</returns>
        public static IUnityContainer Current
        {
            get
            {
                return _currentContainer;
            }
        }

        #endregion

        #region Constructor

        static Container()
        {
            ConfigureFactories();
            ConfigureContainer();
        }

        #endregion

        #region Methods

        static void ConfigureContainer()
        {
            /*
             * Add here the code configuration or the call to configure the container 
             * using the application configuration file
             */

            _currentContainer = new UnityContainer();

            // Factories
            _currentContainer.RegisterType<IUserFactory<DemoUser>, DemoUserFactory>();

            // Unit of Work
            _currentContainer.RegisterType<UserUnitOfWork, DemoUserUnitOfWork>(new PerResolveLifetimeManager());

            // Repositories
            _currentContainer.RegisterType<IUserRepository<DemoUser>, UserRepository<DemoUser>>();

            // Domain Services
            //_currentContainer.RegisterType<IBankTransferService, BankTransferService>();

            // Application services
            _currentContainer.RegisterType<IUserLoginAppService<DemoUserDto>, UserLoginAppService<DemoUserDto, DemoUser>>();
            _currentContainer.RegisterType<IUserRegisterAppService<DemoUserDto>, UserRegisterAppService<DemoUserDto, DemoUser>>();

            // Distributed Services
            _currentContainer.RegisterType<IUserService, UserService>();
        }

        static void ConfigureFactories()
        {
            FactoryHandler<IEntityValidator>.CurrentFactory
                 = new OperatorFactory<DataAnnotationsEntityValidator>();

            FactoryHandler<ITypeAdapter>.CurrentFactory
                = new OperatorFactory<AutomapperTypeAdapter>();

            FactoryHandler<IIdentityGenerator>.CurrentFactory
                = new OperatorFactory<IdentityGenerator>();

            Mapper.Initialize(x =>
            {
                //x.AddProfile<UserDtoToEntityProfile<DemoUserDto, DemoUser>>();
                x.AddProfile<UserEntityToDtoProfile<DemoUser, DemoUserDto>>();
            });
        }

        #endregion
    }
}