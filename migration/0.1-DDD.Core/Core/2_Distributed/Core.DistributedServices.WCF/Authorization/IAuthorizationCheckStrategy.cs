using System.Security.Principal;

namespace Core.DistributedServices.WCF
{
    public interface IAuthorizationCheckStrategy
    {
        bool IsAuthorized(string service, string method, WindowsPrincipal principal);
    }
}
