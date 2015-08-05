namespace Demo.DistributedService
{
    using Microsoft.Practices.Unity;
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// The unity instance provider. This class provides
    /// an extensibility point for creating instances of wcf
    /// service.
    /// <remarks>
    /// The goal is to inject dependencies from the inception point
    /// </remarks>
    /// </summary>
    public class UnityInstanceProvider : IInstanceProvider
    {
        #region Fields

        private readonly Type _serviceType;
        private readonly IUnityContainer _container;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of unity instance provider
        /// </summary>
        /// <param name="serviceType">The service where we apply the instance provider</param>
        public UnityInstanceProvider(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            _serviceType = serviceType;
            _container = Container.Current;
        }

        #endregion

        #region IInstance Provider Members

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            //This is the only call to UNITY container in the whole solution
            return _container.Resolve(_serviceType);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion
    }
}