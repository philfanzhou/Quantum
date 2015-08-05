using System;
using System.ServiceModel;
using System.Threading;
using Core.Infrastructure.Crosscutting;

namespace Core.DistributedServices.WCF
{
    public class DuplexServiceClient<T> : IDuplexServiceClient<T> where T : class
    {
        public const int DefaultAutoReconnectInterval = 1000;

        public readonly int AutoReconnectInterval;

        protected const string AutoReconnectIntervalKey = "SubscriptionServiceAutoReconnectInterval";

        private DuplexChannelFactory<T> channelFactory;
        private T client;
        private readonly ILogger _logger = ContainerHelper.Resolve<ILogger>();

        public DuplexServiceClient(InstanceContext callbackInstance, string endpointConfigurationName)
        {
            var interval = ConfigurationProvider.Configuration.AppSettings.Settings[AutoReconnectIntervalKey];

            this.AutoReconnectInterval = interval == null ? DefaultAutoReconnectInterval : int.Parse(interval.Value);

            this.channelFactory = new CustomDuplexChannelFactory<T>(callbackInstance, endpointConfigurationName);
            this.client = this.channelFactory.CreateChannel();

            var duplexClent = this.client as ICommunicationObject;
            if (duplexClent == null)
            {
                throw new ArgumentException(string.Format("The type {0} is not a communication objec", typeof(T).FullName));
            }

            duplexClent.Open();
        }

        public event EventHandler OnProxyFaulted;

        /// <summary>
        /// Gets of proxy of the service contract.
        /// </summary>
        public T Invoker
        {
            get
            {
                return this.client;
            }
        }

        protected virtual void OnReconnected()
        {
        }

        protected void ReconnectServer()
        {
            ICommunicationObject duplexClient = this.client as ICommunicationObject;
            if (duplexClient != null)
            {
                try
                {
                    duplexClient.Close();
                }
                finally
                {
                    duplexClient.Abort();
                }
            }

            this.ConnectionRetry();
        }

        protected virtual void ConnectionRetry()
        {
            try
            {
                _logger.LogInfo(string.Format("Reconnecting service {0}", this.client.GetType()));
                
                this.client = this.channelFactory.CreateChannel();
                ICommunicationObject duplexClient = this.client as ICommunicationObject;
                duplexClient.Open();
                _logger.LogInfo(string.Format("Reconnected service {0}", duplexClient.GetType()));

                this.OnReconnected();
            }
            catch (Exception)
            {
                Thread.Sleep(this.AutoReconnectInterval);
                this.ConnectionRetry();
            }
        }

        private void Proxy_Faulted(object sender, EventArgs e)
        {
            if (this.OnProxyFaulted != null)
            {
                this.OnProxyFaulted(sender, e);
            }

            this.ReconnectServer();
        }
    }
}