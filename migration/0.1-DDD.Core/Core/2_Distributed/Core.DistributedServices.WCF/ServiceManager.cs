using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Crosscutting.Util.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Core.DistributedServices.WCF
{
    public class ServiceManager
    {
        private IList<WCFServiceController> serviceControllerList = new List<WCFServiceController>();

        private IList<IServiceUnhandledExceptionHandler> serviceUnhandledExceptionHandlerList =
            new List<IServiceUnhandledExceptionHandler>();

        private IList<object> serviceEntryList = new List<object>();

        private volatile ServiceManagerStatus status = ServiceManagerStatus.Created;

        private object statusSyncRoot = new object();

        private readonly ILogger _logger = ContainerHelper.Resolve<ILogger>();

        private Func<Type, bool> contractFilter = (type) =>
        {
            if (type == typeof (IHealthManagementService) || type == typeof (IWCFService))
            {
                return false;
            }
            else if (type.IsGenericType &&
                     type.GetGenericTypeDefinition() == typeof (ISubscriptionService<,,>))
            {
                return false;
            }

            return true;
        };

        public ServiceManager(IList<object> serviceEntryList)
        {
            this.ServiceEntryList = serviceEntryList;
        }

        public IList<object> ServiceEntryList
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("ServiceEntryList");
                }

                lock (this.statusSyncRoot)
                {
                    this.serviceEntryList = value;
                    var healthManagementService = new HealthManagementService();

                    healthManagementService.GetAllWCFServiceStatus = this.GetAllWCFServiceStatus;
                    this.serviceEntryList.Insert(0, healthManagementService);
                    this.InitServiceControllerList(this.serviceEntryList);
                }
            }
        }

        public IList<IServiceUnhandledExceptionHandler> UnhandledExceptionHandlerList
        {
            get
            {
                return this.serviceUnhandledExceptionHandlerList;
            }
        }

        public ServiceManagerStatus Status
        {
            get
            {
                return this.status;
            }
        }

        public IList<WCFServiceHealthInfo> GetAllWCFServiceStatus()
        {
            IList<WCFServiceHealthInfo> hostedWCFServiceInfo = new List<WCFServiceHealthInfo>();

            foreach (var controller in this.serviceControllerList)
            {
                ServiceStatus status;

                var firstServiceType =
                    controller.ServiceHost.ImplementedContracts.FirstOrDefault(
                        c => this.contractFilter(c.Value.ContractType));
                if (!string.IsNullOrEmpty(firstServiceType.Key))
                {
                    try
                    {
                        this.CheckingServiceHealth(firstServiceType.Value, controller);
                        status = controller.Status;
                    }
                    catch (Exception ex)
                    {
                        status = ServiceStatus.Faulted;
                        _logger.LogError("Checking Service Health Error", ex);
                    }

                    controller.ServiceHost.ImplementedContracts.ForEach(
                        c =>
                        {
                            if (this.contractFilter(c.Value.ContractType))
                            {
                                hostedWCFServiceInfo.Add(
                                    new WCFServiceHealthInfo()
                                    {
                                        Name = c.Value.ContractType.FullName,
                                        Status = status,
                                        StartedTime = controller.StartedTime
                                    });
                            }
                        });
                }
            }

            return hostedWCFServiceInfo;
        }

        public virtual void Open()
        {
            this.serviceUnhandledExceptionHandlerList.Clear();
            this.serviceUnhandledExceptionHandlerList.Add(new LoggingHandler());
            this.serviceUnhandledExceptionHandlerList.Add(new AutoRestartHandler());
            lock (this.statusSyncRoot)
            {
                if (this.status == ServiceManagerStatus.Opening)
                {
                    return;
                }
                else if (this.status != ServiceManagerStatus.Created)
                {
                    throw new InvalidOperationException("Open can be called only when the current status is Created");
                }

                this.status = ServiceManagerStatus.Opening;
            }

            try
            {
                foreach (var serviceController in this.serviceControllerList)
                {
                    try
                    {
                        serviceController.Open();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            string.Format("Service Manager start {0} service error", serviceController.ServiceName), ex);
                        throw ex;
                    }
                }
            }
            finally
            {
                lock (this.statusSyncRoot)
                {
                    this.status = ServiceManagerStatus.Opened;
                }
            }
        }

        public virtual void Stop()
        {
            this.serviceUnhandledExceptionHandlerList.Clear();
            lock (this.statusSyncRoot)
            {
                if (this.status == ServiceManagerStatus.Closing)
                {
                    return;
                }
                else if (this.status != ServiceManagerStatus.Opened)
                {
                    throw new InvalidOperationException("Stop can be called only when the current status is Opened");
                }

                this.status = ServiceManagerStatus.Closing;
            }

            try
            {
                foreach (var serviceController in this.serviceControllerList)
                {
                    try
                    {
                        serviceController.Stop();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            string.Format("Service Manager stop {0} service error", serviceController.ServiceName), ex);
                    }
                }
            }
            finally
            {
                lock (this.statusSyncRoot)
                {
                    this.status = ServiceManagerStatus.Closed;
                }
            }
        }

        public virtual IAuthorizationCheckStrategy CreateAuthorizationCheckStrategy()
        {
            return new AuthorizationCheckStrategy();
        }

        private bool CheckingServiceHealth(ContractDescription firstServiceType, WCFServiceController controller)
        {
            var serviceContracts = firstServiceType.ContractType.GetCustomAttributes(typeof (ServiceContractAttribute),
                false);
            var serviceContract = serviceContracts[0] as ServiceContractAttribute;
            ServiceEndpoint firstEndPoint = controller.ServiceHost.Description.Endpoints[0];
            IWCFService serviceClient = null;
            if (serviceContract.CallbackContract != null)
            {
                serviceClient = this.GetDuplexChannelServiceClient(firstServiceType.ContractType,
                    serviceContract.CallbackContract, firstEndPoint);
            }
            else
            {
                serviceClient = this.GetServiceClient(firstServiceType.ContractType, firstEndPoint);
            }

            return ServiceInvoker.Invoke<IWCFService, bool>(c => c.IsAlive(), serviceClient);
        }

        private IWCFService GetServiceClient(Type contractType, ServiceEndpoint endPoint)
        {
            Type constructedType = typeof (ChannelFactory<>).MakeGenericType(contractType);
            var channelFactory = Activator.CreateInstance(constructedType);
            var method = constructedType.GetMethod("CreateChannel",
                new Type[2] {typeof (Binding), typeof (EndpointAddress)});
            return method.Invoke(channelFactory, new object[] {endPoint.Binding, endPoint.Address}) as IWCFService;
        }

        private IWCFService GetDuplexChannelServiceClient(Type contractType, Type callbackType, ServiceEndpoint endPoint)
        {
            Type constructedType = typeof (DuplexChannelFactory<>).MakeGenericType(contractType);
            var callback = DummyCallbackImplementCreator.Create(callbackType);
            var channelFactory = Activator.CreateInstance(constructedType, callback, endPoint.Binding);
            var method = constructedType.GetMethod("CreateChannel", new Type[1] {typeof (EndpointAddress)});
            return method.Invoke(channelFactory, new object[] {endPoint.Address}) as IWCFService;
        }

        private void InitServiceControllerList(IList<object> serviceEntryList)
        {
            if (serviceEntryList == null || serviceEntryList.Count == 0)
            {
                throw new ArgumentNullException("serviceEntryList");
            }

            foreach (var serviceEntry in serviceEntryList)
            {
                Type serviceType = serviceEntry as Type;

                WCFServiceController serviceController = null;
                if (serviceType != null)
                {
                    serviceController = new WCFServiceController(this, serviceType);
                }
                else
                {
                    serviceController = new WCFServiceController(this, serviceEntry);
                }

                serviceController.ServiceHostCreated = this.ApplySecurityCheckCallContext;

                this.serviceControllerList.Add(serviceController);
            }
        }

        private void ApplySecurityCheckCallContext(WCFServiceController serviceController)
        {
            serviceController.ServiceHost.Description.Behaviors.Add(
                new AuthorizationCheckBehavior(this.CreateAuthorizationCheckStrategy));
        }
    }
}
