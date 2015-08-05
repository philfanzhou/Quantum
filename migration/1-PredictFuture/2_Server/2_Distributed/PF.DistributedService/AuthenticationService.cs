namespace PF.DistributedService
{
    using Core.Infrastructure.Crosscutting.Service;
    using PF.Application.UserContext;
    using PF.DistributedService.Contracts.UserContext;

    public class AuthenticationService : WCFServiceBase, IAuthenticationService
    {
        private readonly AuthenticationAppService appService
            = new AuthenticationAppService();

        public bool validate(string identity, string encryptedPassword)
        {
            return appService.validate(identity, encryptedPassword);
        }

        public bool register(string identity, string encryptedPassword)
        {
            return appService.register(identity, encryptedPassword);
        }

        public bool active(string identity)
        {
            return appService.active(identity);
        }
    }
}
