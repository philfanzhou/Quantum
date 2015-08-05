using System;
using System.ServiceModel.Dispatcher;
using Core.Infrastructure.Crosscutting;

namespace Core.DistributedServices.WCF
{
    public class LoggingHandler : IServiceUnhandledExceptionHandler
    {
        private readonly ILogger _logger = ContainerHelper.Resolve<ILogger>();
        public bool Handle(Exception exception, ChannelDispatcher dispatcher, WCFServiceController wcfServiceController)
        {

            _logger.LogError("WCF Service unhandled exception", exception);
            return true;
        }
    }
}
