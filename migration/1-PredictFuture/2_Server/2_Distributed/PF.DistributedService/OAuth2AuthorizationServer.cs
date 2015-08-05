namespace PF.DistributedService
{
    using Core.Infrastructure.Crosscutting.Service;
    using OAuth2 = Core.DistributedServices.OAuth2;

    public class OAuth2AuthorizationServer : WCFServiceBase, OAuth2.IOAuth2AuthorizationServer
    {
        private readonly OAuth2.OAuth2AuthorizationServer appService
            = new OAuth2.OAuth2AuthorizationServer();

        public string GetAuthorizationCode(OAuth2.AuthorizationRequest request)
        {
            return appService.GetAuthorizationCode(request);
        }

        public OAuth2.AccessToken GetAccessToken(string code, string secret, string user)
        {
            return appService.GetAccessToken(code, secret, user);
        }

        public bool VerifyAccessToken(OAuth2.AccessToken token)
        {
            return appService.VerifyAccessToken(token);
        }
    }
}
