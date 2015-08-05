using Core.Infrastructure.Crosscutting;
using System;

namespace Core.DistributedServices.WCF
{
    public class WCFServiceController
    {
        private volatile ServiceStatus status = ServiceStatus.Created;
        private object statusSyncRoot = new object();
        private CreateServiceHostHandler createServiceHost;
        private readonly ILogger _logger = ContainerHelper.Resolve<ILogger>();

        public WCFServiceController(ServiceManager serviceManager, Type serviceType)
        {
            if (serviceManager == null)
            {
                throw new ArgumentNullException("serviceManager");
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.ServiceManager = serviceManager;

            this.createServiceHost = delegate
            {
                return new ServiceHostEx(serviceType);
            };
        }

        public WCFServiceController(ServiceManager serviceManager, object singletonInstance)
        {
            if (serviceManager == null)
            {
                throw new ArgumentNullException("serviceManager");
            }

            if (singletonInstance == null)
            {
                throw new ArgumentNullException("singletonInstance");
            }

            this.ServiceManager = serviceManager;

            this.createServiceHost = delegate
            {
                return new ServiceHostEx(singletonInstance);
            };
        }

        public delegate void ServiceHostCreatedHandler(WCFServiceController controller);

        private delegate ServiceHostEx CreateServiceHostHandler();

        public ServiceHostCreatedHandler ServiceHostCreated { get; set; }

        public ServiceManager ServiceManager { get; private set; }

        public string ServiceName { get; private set; }

        public DateTime? StartedTime { get; private set; }

        public ServiceHostEx ServiceHost { get; private set; }

        public ServiceStatus Status
        {
            get
            {
                return this.status;
            }
        }

        public void Open()
        {
            Open(false);
        }

        public virtual void Restart()
        {
            Open(true);
        }

        public void Stop()
        {
            if (this.ServiceHost == null || this.status == ServiceStatus.Closing)
            {
                return;
            }

            lock (this.statusSyncRoot)
            {
                if (this.status != ServiceStatus.Opened)
                {
                    throw new InvalidOperationException("Stop can be called only when the current status is Opened");
                }

                this.status = ServiceStatus.Closing;
            }

            ServiceStatus resultStatus = ServiceStatus.Closed;

            try
            {
                this.ServiceHost.Close();
                this.ServiceHost = null;
            }
            catch (Exception ex)
            {
                resultStatus = ServiceStatus.Faulted;
                _logger.LogInfo(string.Format("{0} Service Stop error", this.ServiceName), ex);
            }
            finally
            {
                this.status = resultStatus;
            }
        }

        private void AttachServiceHostEventHandler(ServiceHostEx serviceHostEx)
        {
            serviceHostEx.ServiceHostFault += new EventHandler<ServiceHostFaultEventArgs>(this.HandleServiceHostFault);
        }

        private void HandleServiceHostFault(object sender, ServiceHostFaultEventArgs e)
        {
            var handlerList = this.ServiceManager.UnhandledExceptionHandlerList;
            foreach (var handler in handlerList)
            {
                try
                {
                    if (!handler.Handle(e.Exception, e.ChannelDispatcher, this))
                    {
                        lock (this.statusSyncRoot)
                        {
                            this.status = ServiceStatus.Faulted;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        string.Format("Exception handler {0}, raise exception", handler.GetType().FullName), ex);
                }
            }
        }

        private void Open(bool isRestart)
        {
            if (!isRestart)
            {
                lock (this.statusSyncRoot)
                {
                    if (this.status == ServiceStatus.Opening)
                    {
                        return;
                    }
                    else if (this.status != ServiceStatus.Created)
                    {
                        throw new InvalidOperationException("Open can be called only when the current status is Created");
                    }

                    this.status = ServiceStatus.Opening;
                }
            }

            ServiceStatus resultStatus = ServiceStatus.Opened;

            try
            {
                var oldServiceHost = this.ServiceHost;
                this.ServiceHost = this.createServiceHost();
                this.ServiceName = this.ServiceHost.Description.Name;
                this.AttachServiceHostEventHandler(this.ServiceHost);
                if (this.ServiceHostCreated != null)
                {
                    this.ServiceHostCreated(this);
                }

                if (oldServiceHost != null)
                {
                    try
                    {
                        oldServiceHost.Close();
                    }
                    catch
                    {
                    }
                }

                this.ServiceHost.Open();
                this.StartedTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                resultStatus = ServiceStatus.Faulted;
                _logger.LogError(string.Format("Cannot start WCF Service: {0}", this.ServiceName), ex);
            }
            finally
            {
                lock (this.statusSyncRoot)
                {
                    this.status = resultStatus;
                }
            }
        }
    }
}
