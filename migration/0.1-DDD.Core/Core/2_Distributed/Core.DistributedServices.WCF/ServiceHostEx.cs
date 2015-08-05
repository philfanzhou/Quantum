using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Core.DistributedServices.WCF
{
    public class ServiceHostEx : ServiceHost, IServiceHostFaultHandler
    {
        public ServiceHostEx(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
        }

        public ServiceHostEx(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        public event EventHandler<ServiceHostFaultEventArgs> ServiceHostFault;

        public IDictionary<string, ContractDescription> ImplementedContracts
        {
            get { return base.ImplementedContracts; }
        }

        public virtual bool OnServiceHostFault(ChannelDispatcher channelDispatcher, Exception error)
        {
            var serviceHostFault = this.ServiceHostFault;
            if (serviceHostFault == null)
            {
                return false;
            }

            bool handled = false;
            var eventArgs = new ServiceHostFaultEventArgs(channelDispatcher, error);
            try
            {
                serviceHostFault(this, eventArgs);
                handled = eventArgs.Handled;
            }
            catch
            {
                handled = false;
            }

            return handled;
        }
    }
}
