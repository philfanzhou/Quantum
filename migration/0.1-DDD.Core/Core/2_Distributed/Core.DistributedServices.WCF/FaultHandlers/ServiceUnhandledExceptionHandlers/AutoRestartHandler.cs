using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Crosscutting.Util.RetryMechanism.Strategy;
using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace Core.DistributedServices.WCF
{
    public class AutoRestartHandler : IServiceUnhandledExceptionHandler
    {
        public readonly TimeSpan MaxRestartTimespan;
        public readonly TimeSpan RestartErrorTimespan;

        protected readonly int MaxRestartCount;
        protected readonly int RestartErrorCount;

        private const string WcfServiceRestartErrorCountKey = "WCFService_Restart_ErrorCount";
        private const string WcfServiceRestartErrorTimespanKey = "WCFService_Restart_ErrorTimespan";
        private const int DefaultRestartErrorCount = 3;
        private readonly TimeSpan DefaulrRestartErrorTimespan = new TimeSpan(0, 0, 30);
        private const string WcfServiceMaxRestartCountKey = "WCFService_Max_Restart_Count";
        private const string WcfServiceMaxRestartTimespanKey = "WCFService_Max_Restart_Timespan";
        private const int DefaultMaxRestartCount = 3;
        private readonly TimeSpan DefaultMaxRestartTimespan = new TimeSpan(0, 2, 0);
        private readonly ILogger _logger = ContainerHelper.Resolve<ILogger>();

        private bool pendingRestart = false;

        public AutoRestartHandler()
        {
            var restartCount = ConfigurationProvider.Configuration.AppSettings.Settings[WcfServiceMaxRestartCountKey];
            var restartTimespan = ConfigurationProvider.Configuration.AppSettings.Settings[WcfServiceMaxRestartTimespanKey];
            var restartErrorCount = ConfigurationProvider.Configuration.AppSettings.Settings[WcfServiceRestartErrorCountKey];
            var restartErrorTimespan = ConfigurationProvider.Configuration.AppSettings.Settings[WcfServiceRestartErrorTimespanKey];

            this.MaxRestartCount = restartCount != null ? int.Parse(restartCount.Value) : DefaultMaxRestartCount;
            this.MaxRestartTimespan = restartTimespan != null ? TimeSpan.Parse(restartTimespan.Value) : this.DefaultMaxRestartTimespan;
            this.RestartErrorCount = restartErrorCount != null ? int.Parse(restartErrorCount.Value) : DefaultRestartErrorCount;
            this.RestartErrorTimespan = restartErrorTimespan != null ? TimeSpan.Parse(restartErrorTimespan.Value) : this.DefaulrRestartErrorTimespan;

            AcceptedErrorLimitation = new TimespanAndTimesLimitedStrategy<Exception>(this.RestartErrorTimespan, this.RestartErrorCount);
            AutoRestartLimitation = new TimespanAndTimesLimitedStrategy<Exception>(this.MaxRestartTimespan, this.MaxRestartCount);
        }

        public static TimespanAndTimesLimitedStrategy<Exception> AcceptedErrorLimitation { get; set; }

        public static TimespanAndTimesLimitedStrategy<Exception> AutoRestartLimitation { get; set; }

        public bool Handle(Exception exception, ChannelDispatcher dispatcher, WCFServiceController wcfServiceController)
        {
            if (this.pendingRestart)
            {
                return false;
            }

            if (!(exception is CommunicationException))
            {
                AcceptedErrorLimitation.Add(exception);
                if (AcceptedErrorLimitation.ExceedLimit())
                {
                    AutoRestartLimitation.Add(exception);
                    if (AutoRestartLimitation.ExceedLimit())
                    {
                        _logger.LogWarning("Unable solve exception with configured max-times of WCF Service restart." +
                                           string.Format(
                                               "WCF Servuce {0}, Times of Auto-Restart has exceed the max value within configued time range, waiting window service restart",
                                               wcfServiceController.ServiceName));
                        this.pendingRestart = true;
                        return false;
                    }
                    else
                    {
                        AcceptedErrorLimitation.ResetCount();
                        return Handle(wcfServiceController);
                    }
                }
            }

            return true;
        }

        private bool Handle(WCFServiceController wcfServiceController)
        {
            _logger.LogInfo("Queue up restart WCF Service" +
                            string.Format("Queue to restarting WCF Service, Service name: {0}",
                                wcfServiceController.ServiceName));

            ThreadPool.QueueUserWorkItem(
                delegate(object state)
                {
                    wcfServiceController.Restart();
                    _logger.LogInfo("Restarted WCF Service" +
                                    string.Format("Restarted WCF Service, Service name: {0}",
                                        wcfServiceController.ServiceName));
                });
            return true;
        }
    }
}
