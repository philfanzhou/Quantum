using System.ServiceModel;

namespace Core.DistributedServices.WCF
{
    public class CustomDuplexChannelFactory<T> : DuplexChannelFactory<T>
    {
        public CustomDuplexChannelFactory(InstanceContext callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
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
