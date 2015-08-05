using System;
using System.ServiceModel.Dispatcher;

namespace Core.DistributedServices.WCF
{
    public interface IServiceHostFaultHandler
    {
        bool OnServiceHostFault(ChannelDispatcher channelDispatcher, Exception error);
    }
}
