namespace Core.Infrastructure.Crosscutting
{
    public interface IContainer
    {
        T Resolve<T>();

        T Resolve<T>(params ConstructorParameter[] parameter);

        bool IsRegistered<T>();

        void RegisterType<TFrom, TTo>() where TTo : TFrom;
    }

    public static class ContainerHelper
    {
        public static IContainer Instance { get; private set; }

        internal static void SetInstance(IContainer container)
        {
            if (null == container)
            {
                throw new System.ArgumentNullException("container");
            }

            Instance = container;
        }

        public static T Resolve<T>()
        {
            return Instance.Resolve<T>();
        }
    }

    public class ConstructorParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
