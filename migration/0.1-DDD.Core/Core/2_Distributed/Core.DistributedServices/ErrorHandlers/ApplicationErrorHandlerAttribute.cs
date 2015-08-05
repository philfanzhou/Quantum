//namespace Core.DistributedServices
//{
//    using System;
//    using System.Linq;
//    using System.ServiceModel;
//    using System.ServiceModel.Description;
//    using System.ServiceModel.Dispatcher;

//    /// <summary>
//    /// Service behavior for add DefaultErrorHandler to all dispatcher in
//    /// Windows Communication Foundation 
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
//    public sealed class ApplicationErrorHandlerAttribute
//        : Attribute, IServiceBehavior
//    {
//        /// <summary>
//        ///Provides the ability to pass custom data to binding elements to support the contract implementation
//        /// </summary>
//        /// <param name="serviceDescription">The service description of the service.</param>
//        /// <param name="serviceHostBase">The host of the service.</param>
//        /// <param name="endpoints">The service endpoints.</param>
//        /// <param name="bindingParameters">The service endpoints.</param>
//        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
//        {
//            //not apply for this sample
//        }
//        /// <summary>
//        /// Provides the ability to change run-time property values or insert custom
//        /// extension objects such as error handlers, message or parameter interceptors,
//        /// security extensions, and other custom extension objects.
//        /// </summary>
//        /// <param name="serviceDescription">The service description.</param>
//        /// <param name="serviceHostBase">The host that is currently being built.</param>
//        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
//        {
//            if (serviceHostBase.ChannelDispatchers.Any())
//            {
//                //add default error handler to all dispatcher in wcf services
//                foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
//                    dispatcher.ErrorHandlers.Add(new ApplicationErrorHandler());
//            }
//        }
//        /// <summary>
//        ///  Provides the ability to inspect the service host and the service description
//        ///  to confirm that the service can run successfully.
//        /// </summary>
//        /// <param name="serviceDescription">The service description.</param>
//        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
//        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
//        {
//            //not apply for this sample
//        }
//    }
//}
