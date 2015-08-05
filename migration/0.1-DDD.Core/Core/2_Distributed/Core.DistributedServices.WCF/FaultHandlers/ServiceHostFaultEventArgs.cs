using System;
using System.ServiceModel.Dispatcher;

namespace Core.DistributedServices.WCF
{
    public class ServiceHostFaultEventArgs : EventArgs
    {
        public ServiceHostFaultEventArgs(ChannelDispatcher channelDispatcher, Exception ex)
        {
            Handled = false;
            ChannelDispatcher = channelDispatcher;
            Exception = ex;
        }

        public bool Handled { get; set; }

        public ChannelDispatcher ChannelDispatcher { get; private set; }

        public Exception Exception { get; private set; }
    }
}
