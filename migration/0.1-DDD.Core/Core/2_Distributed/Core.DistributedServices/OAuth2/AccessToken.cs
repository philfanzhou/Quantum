using System;

namespace Core.DistributedServices.OAuth2
{
    [Serializable]
    public class AccessToken
    {
        public string ClientIdentifier { get; private set; }
        public string Scope { get; private set; }
        public string Token { get; private set; }
        public string User { get; private set; }
        public DateTime IssuedUtcTime { get; private set; }
        public DateTime ExpireUtcTime { get; private set; }

        private AccessToken()
        {}

        public AccessToken(string client, string scope, string user)
            : this(client, scope, user, new TimeSpan(1, 0, 0, 0))
        {}

        public AccessToken(string client, string scope, string user, TimeSpan expire)
        {
            this.ClientIdentifier = client;
            this.Scope = scope;
            this.User = user;
            this.Token = Guid.NewGuid().ToString("N");
            this.IssuedUtcTime = DateTime.UtcNow;
            this.ExpireUtcTime = this.IssuedUtcTime.Add(expire);
        }
    }
}
