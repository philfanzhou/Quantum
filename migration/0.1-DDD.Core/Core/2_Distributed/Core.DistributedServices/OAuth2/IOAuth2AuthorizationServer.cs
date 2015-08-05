using System.ServiceModel;

namespace Core.DistributedServices.OAuth2
{
    [ServiceContract]
    public interface IOAuth2AuthorizationServer
    {
        /// <summary>
        /// Generate Authorization Code
        /// </summary>
        [OperationContract]
        string GetAuthorizationCode(AuthorizationRequest request);

        /// <summary>
        /// Generate Access Token
        /// </summary>
        [OperationContract]
        AccessToken GetAccessToken(string code, string secret, string user);

        /// <summary>
        /// Verify Access Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [OperationContract]
        bool VerifyAccessToken(string token);
    }
}
