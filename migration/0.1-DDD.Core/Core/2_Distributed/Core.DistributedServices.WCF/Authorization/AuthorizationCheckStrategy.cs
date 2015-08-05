using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Linq;
using Core.Infrastructure.Crosscutting;
using Core.DistributedServices.OAuth2;

namespace Core.DistributedServices.WCF
{
    public class AuthorizationCheckStrategy : IAuthorizationCheckStrategy
    {
        private IOAuth2AuthorizationServer oauth2;

        public AuthorizationCheckStrategy()
        {
            oauth2 = ContainerHelper.Resolve<IOAuth2AuthorizationServer>();
        }

        public bool IsAuthorized(string service, string method, WindowsPrincipal principal)
        {
            if (WebOperationContext.Current == null)
            {
                return true;
            }
            else
            {
                var methodInfo = CurrentEndpoint.Contract.ContractType.GetMethod(method);
                var attrs = methodInfo.GetCustomAttributes(typeof(TokenRequiredAttribute), false);
                if (attrs == null || attrs.Length == 0)
                {
                    return true;
                }
                else
                {
                    string token = WebOperationContext.Current.IncomingRequest.Headers.Get("Authorization-OAuth2");

                    if (string.IsNullOrEmpty(token))
                    {
                        return false;
                    }
                    else
                    {
                        return oauth2.VerifyAccessToken(token);
                    }
                }
            }
        }

        private ServiceEndpoint CurrentEndpoint
        {
            get
            {
                return OperationContext.Current.Host.Description.Endpoints.Find(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri);
            }
        }

    }
}
