using System;
using System.ServiceModel.Dispatcher;

namespace Core.DistributedServices.WCF
{
    public interface IServiceUnhandledExceptionHandler
    {
        bool Handle(Exception exception, ChannelDispatcher dispatcher, WCFServiceController wcfServiceController);
    }
}
