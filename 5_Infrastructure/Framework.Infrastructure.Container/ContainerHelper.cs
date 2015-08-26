using System.Threading;

namespace Framework.Infrastructure.Container
{
    public static class ContainerHelper
    {
        private static IContainer _instance;

        public static IContainer Instance
        {
            get
            {
                if (null == _instance)
                {
                    Interlocked.CompareExchange(ref _instance, new MyUnityContainer(), null);
                }
                return _instance;
            }
        }

        public static T Resolve<T>()
        {
            return Instance.Resolve<T>();
        }
    }
}
