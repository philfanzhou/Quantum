namespace Framework.Infrastructure.Container
{
    public interface IContainer
    {
        T Resolve<T>();

        T Resolve<T>(params ConstructorParameter[] parameter);

        bool IsRegistered<T>();

        void RegisterType<TFrom, TTo>() where TTo : TFrom;
    }
}
