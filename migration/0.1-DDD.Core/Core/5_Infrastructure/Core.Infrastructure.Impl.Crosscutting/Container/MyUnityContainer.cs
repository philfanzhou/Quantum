namespace Core.Infrastructure.Impl.Crosscutting
{
    using Core.Infrastructure.Crosscutting;
    using Microsoft.Practices.Unity;

    internal sealed class MyUnityContainer : IContainer
    {
        private readonly IUnityContainer _unityContainer = new UnityContainer();

        public IUnityContainer UnityContainer
        {
            get
            {
                return this._unityContainer;
            }
        }

        public T Resolve<T>()
        {
            return this._unityContainer.Resolve<T>();
        }

        public T Resolve<T>(params ConstructorParameter[] parameter)
        {
            ParameterOverrides overrides = new ParameterOverrides();
            foreach (var item in parameter)
            {
                overrides.Add(item.Name, item.Value);
            }

            return this._unityContainer.Resolve<T>(overrides);
        }

        public bool IsRegistered<T>()
        {
            return this._unityContainer.IsRegistered<T>();
        }

        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            this._unityContainer.RegisterType<TFrom, TTo>();
        }
    }
}
