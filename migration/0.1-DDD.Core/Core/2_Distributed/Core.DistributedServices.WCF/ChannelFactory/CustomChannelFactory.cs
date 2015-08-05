using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Core.DistributedServices.WCF
{
    public class CustomChannelFactory<T> : ChannelFactory<T>
    {
        public CustomChannelFactory(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
            InitializeEndpoint(endpointConfigurationName, null);
        }

        public CustomChannelFactory(Binding binding, EndpointAddress endpoint)
            : base(binding, endpoint)
        {
            InitializeEndpoint(binding, endpoint);
        }

        public CustomChannelFactory(Binding binding)
            : base(binding)
        {
        }

        /// <summary>
        /// overrides the ApplyConfiguration() method of the channel factory
        /// to apply a new configuration file
        /// </summary>
        /// <param name="configurationName"></param>
        protected override void ApplyConfiguration(string configurationName)
        {
            CustomChannelFactoryHelper.ApplyConfiguration(ConfigurationProvider.Configuration, Endpoint, configurationName);
        }
    }
}
