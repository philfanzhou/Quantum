using System.Threading;

namespace Framework.Infrastructure.Container
{
    public static class ContainerHelper
    {
        private static IContainer instance;

        public static IContainer Instance
        {
            get
            {
                if (null == instance)
                {
                    Interlocked.CompareExchange(ref instance, new MyUnityContainer(), null);
                }
                return instance;
            }
        }

        public static T Resolve<T>()
        {
            return Instance.Resolve<T>();
        }
    }
}
