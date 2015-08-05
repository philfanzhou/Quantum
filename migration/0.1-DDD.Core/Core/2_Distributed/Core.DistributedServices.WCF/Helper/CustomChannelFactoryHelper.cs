using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace Core.DistributedServices.WCF
{
    public static class CustomChannelFactoryHelper
    {
        private static ServiceModelSectionGroup groups = null;

        public static void ApplyConfiguration(System.Configuration.Configuration config, ServiceEndpoint serviceEndpoint, string configurationName)
        {
            if (config == null)
            {
                return;
            }

            ServiceModelSectionGroup serviceModeGroup = ServiceModelSectionGroup.GetSectionGroup(config);
            LoadChannelBehaviors(serviceEndpoint, configurationName, serviceModeGroup);
        }

        /// <summary>
        /// Load the endpoint with the binding, address, behaviour etc. from the alternative config file 
        /// </summary>
        /// <param name="serviceEndpoint"></param>
        /// <param name="configurationName"></param>
        /// <param name="serviceModeGroup"></param>
        /// <returns></returns>
        public static ServiceEndpoint LoadChannelBehaviors(ServiceEndpoint serviceEndpoint, string configurationName, ServiceModelSectionGroup serviceModeGroup)
        {
            bool isWildcard = string.Equals(configurationName, "*", StringComparison.Ordinal);
            ChannelEndpointElement provider = LookupChannel(serviceModeGroup, configurationName, serviceEndpoint.Contract.ConfigurationName, isWildcard);

            if (provider == null)
            {
                return null;
            }

            if (serviceEndpoint.Binding == null)
            {
                serviceEndpoint.Binding = LookupBinding(serviceModeGroup, provider.Binding, provider.BindingConfiguration);
            }

            if (serviceEndpoint.Address == null)
            {
                serviceEndpoint.Address = new EndpointAddress(provider.Address, LookupIdentity(provider.Identity), provider.Headers.Headers);
            }

            if (serviceEndpoint.Behaviors.Count == 0 && !string.IsNullOrEmpty(provider.BehaviorConfiguration))
            {
                LoadBehaviors(serviceModeGroup, provider.BehaviorConfiguration, serviceEndpoint);
            }

            serviceEndpoint.Name = provider.Contract;

            return serviceEndpoint;
        }

        /// <summary>
        /// Find the endpoint in the alternative config file that matches the required contract and configuration
        /// </summary>
        /// <param name="serviceModeGroup"></param>
        /// <param name="configurationName"></param>
        /// <param name="contractName"></param>
        /// <param name="wildcard"></param>
        /// <returns></returns>
        private static ChannelEndpointElement LookupChannel(ServiceModelSectionGroup serviceModeGroup, string configurationName, string contractName, bool wildcard)
        {
            foreach (ChannelEndpointElement endpoint in serviceModeGroup.Client.Endpoints)
            {
                if (endpoint.Contract == contractName && (endpoint.Name == configurationName || wildcard))
                {
                    return endpoint;
                }
            }

            return null;
        }

        /// <summary>
        /// Configures the binding for the selected endpoint
        /// </summary>
        /// <param name="group"></param>
        /// <param name="bindingName"></param>
        /// <param name="configurationName"></param>
        /// <returns></returns>
        private static Binding LookupBinding(ServiceModelSectionGroup group, string bindingName, string configurationName)
        {
            IBindingConfigurationElement bindingConfigurationElement = null;

            BindingCollectionElement bindingElementCollection = group.Bindings[bindingName];
            Binding binding = GetBinding(bindingElementCollection.BindingType);

            bindingConfigurationElement = bindingElementCollection.ConfiguredBindings.FirstOrDefault(item => item.Name == configurationName);

            if (bindingConfigurationElement != null)
            {
                bindingConfigurationElement.ApplyConfiguration(binding);
            }

            return binding;
        }

        /// <summary>
        /// Gets the endpoint identity from the configuration file
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static EndpointIdentity LookupIdentity(IdentityElement element)
        {
            EndpointIdentity identity = null;
            PropertyInformationCollection properties = element.ElementInformation.Properties;
            if (properties["userPrincipalName"].ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateUpnIdentity(element.UserPrincipalName.Value);
            }

            if (properties["servicePrincipalName"].ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateSpnIdentity(element.ServicePrincipalName.Value);
            }

            if (properties["dns"].ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateDnsIdentity(element.Dns.Value);
            }

            if (properties["rsa"].ValueOrigin != PropertyValueOrigin.Default)
            {
                return EndpointIdentity.CreateRsaIdentity(element.Rsa.Value);
            }

            if (properties["certificate"].ValueOrigin != PropertyValueOrigin.Default)
            {
                X509Certificate2Collection supportingCertificates = new X509Certificate2Collection();
                supportingCertificates.Import(Convert.FromBase64String(element.Certificate.EncodedValue));
                if (supportingCertificates.Count == 0)
                {
                    throw new InvalidOperationException("UnableToLoadCertificateIdentity");
                }

                X509Certificate2 primaryCertificate = supportingCertificates[0];
                supportingCertificates.RemoveAt(0);
                return EndpointIdentity.CreateX509CertificateIdentity(primaryCertificate, supportingCertificates);
            }

            return identity;
        }

        /// <summary>
        /// Adds the configured behavior to the selected endpoint
        /// </summary>
        /// <param name="group"></param>
        /// <param name="behaviorConfiguration"></param>
        /// <param name="serviceEndpoint"></param>
        private static void LoadBehaviors(ServiceModelSectionGroup group, string behaviorConfiguration, ServiceEndpoint serviceEndpoint)
        {
            EndpointBehaviorElement behaviorElement = group.Behaviors.EndpointBehaviors[behaviorConfiguration];
            for (int i = 0; i < behaviorElement.Count; i++)
            {
                BehaviorExtensionElement behaviorExtension = behaviorElement[i];
                object extension = behaviorExtension.GetType().InvokeMember(
                 "CreateBehavior",
                 BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                 null,
                 behaviorExtension,
                 null,
                 CultureInfo.InvariantCulture);
                if (extension != null)
                {
                    serviceEndpoint.Behaviors.Add((IEndpointBehavior)extension);
                }
            }
        }

        /// <summary>
        /// Helper method to create the right binding depending on the configuration element
        /// </summary>
        /// <param name="configurationElement"></param>
        /// <returns></returns>
        private static Binding GetBinding(Type bindingType)
        {
            if (bindingType == typeof(NetTcpBinding))
            {
                return new NetTcpBinding();
            }

            if (bindingType == typeof(NetMsmqBinding))
            {
                return new NetMsmqBinding();
            }

            if (bindingType == typeof(BasicHttpBinding))
            {
                return new BasicHttpBinding();
            }

            if (bindingType == typeof(NetNamedPipeBinding))
            {
                return new NetNamedPipeBinding();
            }

            if (bindingType == typeof(NetPeerTcpBinding))
            {
                return new NetPeerTcpBinding();
            }

            if (bindingType == typeof(WSDualHttpBinding))
            {
                return new WSDualHttpBinding();
            }

            if (bindingType == typeof(WSHttpBinding))
            {
                return new WSHttpBinding();
            }

            if (bindingType == typeof(WSFederationHttpBinding))
            {
                return new WSFederationHttpBinding();
            }

            if (bindingType == typeof(CustomBinding))
            {
                return new CustomBinding();
            }

            if (bindingType == typeof(WebHttpBinding))
            {
                return new WebHttpBinding();
            }

            return null;
        }
    }
}
