using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Core.DistributedServices.WCF
{
    public class AuthorizationCheckBehavior : IServiceBehavior
    {
        private ServiceAuthorizationTypes authType = ServiceAuthorizationTypes.Domain;
       
        private Func<IAuthorizationCheckStrategy> createAuthorizationCheckStrategy;

        public AuthorizationCheckBehavior(Func<IAuthorizationCheckStrategy> createAuthorizationCheckStrategy)
        {
            this.createAuthorizationCheckStrategy = createAuthorizationCheckStrategy;
        }

        public void Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription description, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {
            switch (this.authType)
            {
                case ServiceAuthorizationTypes.Domain:
                    {
                        ConfigureDomain(endpoints);
                        break;
                    }

                default:
                    {
                        throw new InvalidOperationException("unknow authorization type");
                    }
            }
        }

        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpoint in dispatcher.Endpoints)
                {
                    foreach (DispatchOperation operation in endpoint.DispatchRuntime.Operations)
                    {
                        operation.CallContextInitializers.Add(new SecurityCallContextInitializer(this.createAuthorizationCheckStrategy()));
                    }
                }
            }
        }

        internal static void ConfigureDomain(Collection<ServiceEndpoint> endpoints)
        {
            foreach (ServiceEndpoint endpoint in endpoints)
            {
                Binding binding = endpoint.Binding;

                if (binding is NetTcpBinding)
                {
                    NetTcpBinding tcpBinding = (NetTcpBinding)binding;
                    tcpBinding.Security.Mode = SecurityMode.Transport;
                    tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                    tcpBinding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
                    continue;
                }

                if (binding is NetNamedPipeBinding)
                {
                    NetNamedPipeBinding pipeBinding = (NetNamedPipeBinding)binding;
                    pipeBinding.Security.Mode = NetNamedPipeSecurityMode.Transport;
                    pipeBinding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
                    continue;
                }

                if (binding is NetMsmqBinding)
                {
                    NetMsmqBinding msmqBinding = (NetMsmqBinding)binding;
                    msmqBinding.Security.Mode = NetMsmqSecurityMode.Transport;
                    msmqBinding.Security.Transport.MsmqAuthenticationMode = MsmqAuthenticationMode.WindowsDomain;
                    msmqBinding.Security.Transport.MsmqProtectionLevel = ProtectionLevel.EncryptAndSign;
                    continue;
                }

                if (binding is CustomBinding && endpoint.Contract.ContractType == typeof(IMetadataExchange))
                {
                    Trace.WriteLine("No declarative security for MEX endpoint when adding it programmatically");
                    continue;
                }

                if (binding is WSHttpBinding)
                {
                    WSHttpBinding httpBinding = (WSHttpBinding)binding;
                    httpBinding.Security.Mode = SecurityMode.None;
                    httpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
                    continue;
                }

                if (binding is BasicHttpBinding)
                {
                    BasicHttpBinding basicHttpBinding = (BasicHttpBinding)binding;
                    basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                    continue;
                }

                if (binding is WebHttpBinding)
                {
                    WebHttpBinding webHttpBinding = (WebHttpBinding)binding;
                    webHttpBinding.Security.Mode = WebHttpSecurityMode.None;
                    continue;
                }

                throw new InvalidOperationException(binding.GetType() + "is unsupported with ServiceSecurity.Intranet");
            }
        }
    }
}
