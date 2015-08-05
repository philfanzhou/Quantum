using System;

namespace Core.DistributedServices.OAuth2
{
    [Serializable]
    public class AuthorizationRequest
    {
        public string ClientIdentifier { get; set; }
        public string Scope { get; set; }
        public string RedirectURI { get; set; }
        public string Bag { get; set; }
    }
}
