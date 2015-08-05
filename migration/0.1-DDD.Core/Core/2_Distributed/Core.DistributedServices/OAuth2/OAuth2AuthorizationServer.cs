using System;
using System.Collections.Generic;
namespace Core.DistributedServices.OAuth2
{
    public class OAuth2AuthorizationServer : IOAuth2AuthorizationServer
    {
        private static Dictionary<string, AuthorizationRequest> requests = new Dictionary<string, AuthorizationRequest>();
        private static Dictionary<string, AccessToken> tokens = new Dictionary<string, AccessToken>();

        public string GetAuthorizationCode(AuthorizationRequest request)
        {
            string code = Guid.NewGuid().ToString("N");
            if (!requests.ContainsKey(code))
            {
                lock (requests)
                {
                    requests.Add(code, request);
                }
                return code;
            }
            else
            {
                return string.Empty;
            }
        }

        public AccessToken GetAccessToken(string code, string secret, string user)
        {
            if (requests.ContainsKey(code))
            {
                AuthorizationRequest request = requests[code];
                lock (requests)
                {
                    requests.Remove(code);
                }
                AccessToken token = new AccessToken(request.ClientIdentifier, request.Scope, user);
                lock (tokens)
                {
                    if (tokens.ContainsKey(token.Token))
                    {
                        tokens.Remove(token.Token);
                    }
                    tokens.Add(token.Token, token);
                }
                return token;
            }
            else
            {
                return null;
            }
        }

        public bool VerifyAccessToken(string token)
        {
            if (tokens.ContainsKey(token))
            {
                AccessToken storedToken = tokens[token];
                if (storedToken.ExpireUtcTime > DateTime.UtcNow)
                {
                    return true;
                }
                else
                {
                    lock (tokens)
                    {
                        tokens.Remove(storedToken.Token);
                    }
                }
            }
            return false;
        }
    }

}
