namespace Core.Infrastructure.Impl.Crosscutting
{
    using Core.Infrastructure.Crosscutting;
    using System;

    public class SystemInitialize
    {
        public static event Action SystemInitializationEvent;

        public static void Init()
        {
            // 系统内有且仅有一次，指定IOC容器
            MyUnityContainer myContainer = new MyUnityContainer();
            ContainerHelper.SetInstance(myContainer);

            myContainer.RegisterType<IEntityValidator, DataAnnotationsEntityValidator>();
            myContainer.RegisterType<ITypeAdapter, AutomapperTypeAdapter>();
            myContainer.RegisterType<IIdentityGenerator, IdentityGenerator>();
            myContainer.RegisterType<IMd5Encryptor, Md5Encryptor>();
            myContainer.RegisterType<ILogger, TraceSourceLog>();
            myContainer.RegisterType<ISerializer, Serializer>();
            myContainer.RegisterType<IMailService, MailService>();
            myContainer.RegisterType<ICaptcha, Captcha>();
            myContainer.RegisterType<ISystemConfig, SystemConfig>();

            OnSystemInit();
        }

        private static void OnSystemInit()
        {
            // Copy to a temporary variable to be thread-safe.
            Action temp = SystemInitializationEvent;
            if (temp != null)
            {
                temp();
            }
        }
    }
}
