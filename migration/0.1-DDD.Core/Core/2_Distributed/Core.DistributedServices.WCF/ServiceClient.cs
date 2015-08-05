using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Core.DistributedServices.WCF
{
    public class ServiceClient<TChannel> : IServiceClient<TChannel> where TChannel : class
    {
        private CustomChannelFactory<TChannel> channelFactory;
        private TChannel channel;

        public ServiceClient(string configName)
        {
            this.channelFactory = new CustomChannelFactory<TChannel>(configName);
        }

        public ServiceClient(Binding binding, EndpointAddress endpoint)
        {
            this.channelFactory = new CustomChannelFactory<TChannel>(binding);
            this.channel = this.channelFactory.CreateChannel(endpoint);
            this.channelFactory.Endpoint.Address = endpoint;
        }

        public ServiceClient()
            : this(typeof(TChannel).FullName)
        {
        }

        public void Invoke(Action<TChannel> action)
        {
            this.channel = this.channelFactory.CreateChannel();
            ServiceInvoker.Invoke<TChannel>(action, this.channel);
        }

        public TResult Invoke<TResult>(Func<TChannel, TResult> function)
        {
            this.channel = this.channelFactory.CreateChannel();
            return ServiceInvoker.Invoke<TChannel, TResult>(function, this.channel);
        }
    }
}
