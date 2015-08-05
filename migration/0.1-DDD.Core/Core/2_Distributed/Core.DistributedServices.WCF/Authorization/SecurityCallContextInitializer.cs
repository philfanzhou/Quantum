using Core.Infrastructure.Crosscutting;
using System;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using System.Text;

namespace Core.DistributedServices.WCF
{
    public class SecurityCallContextInitializer : ICallContextInitializer
    {
        private IAuthorizationCheckStrategy authorizationCheckStrategy;
        private ILogger _logger = ContainerHelper.Resolve<ILogger>();

        public SecurityCallContextInitializer(IAuthorizationCheckStrategy strategy)
        {
            this.authorizationCheckStrategy = strategy;
        }

        #region ICallContextInitializer Members

        public void AfterInvoke(object correlationState)
        {
            var ticket = correlationState as Guid?;
            _logger.Debug(string.Format("{0} AfterInvoke {1}", DateTime.Now, ticket));
        }

        public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, System.ServiceModel.Channels.Message message)
        {
            WindowsPrincipal currentlyPrincipal = null;
            if (ServiceSecurityContext.Current != null && ServiceSecurityContext.Current.WindowsIdentity != null)
            {
                currentlyPrincipal = new WindowsPrincipal(ServiceSecurityContext.Current.WindowsIdentity);
            }

            string action = OperationContext.Current.IncomingMessageHeaders.Action;
            if (!string.IsNullOrEmpty(action))
            {
                var actions = action.Split('/');
                string methodName = actions[actions.Length - 1];
                string serviceName = actions[actions.Length - 2];

                if (!this.authorizationCheckStrategy.IsAuthorized(serviceName, methodName, currentlyPrincipal))
                {
                    var logMessage = new StringBuilder();
                    var endpointMsg = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                    logMessage.Append("Access Denied");
                    logMessage.AppendFormat("Service: {0}, Method:{1}", serviceName, methodName).AppendLine();
                    logMessage.AppendFormat("Client IP: {0}:{1}", endpointMsg.Address, endpointMsg.Port).AppendLine();
                    logMessage.AppendFormat("Currently Principal: {0}", currentlyPrincipal.Identity.Name).AppendLine();
                    var accessDeniedException = new SecurityAccessDeniedException();
                    _logger.LogWarning(logMessage.ToString());
                    throw accessDeniedException;
                }
            }

            var ticket = Guid.NewGuid();
            _logger.Debug(string.Format("{0} BeforeInvoke {1}, Method {2}", DateTime.Now, ticket, message.Headers.Action));
            return ticket;
        }
        #endregion
    }
}
