using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.DistributedServices.OAuth2;

namespace Core.DistributedServices.Test
{
    [TestClass]
    public class OAuth2
    {
        public TestContext TestContext { get; set; }

        private static IOAuth2AuthorizationServer oauth;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            oauth = new OAuth2AuthorizationServer();
        }

        [TestMethod]
        public void AcceptanceTest()
        {
            string code = oauth.GetAuthorizationCode(new AuthorizationRequest() { ClientIdentifier = "Any", Scope = "all", RedirectURI = "http://somewhere", Bag = "state" });
            Assert.IsFalse(string.IsNullOrEmpty(code));
            Assert.AreNotEqual<string>(Guid.Empty.ToString("N"), code);
            AccessToken token = oauth.GetAccessToken(code, "Secret", "guest");
            Assert.IsNotNull(token);
            Assert.AreNotEqual<string>(Guid.Empty.ToString("N"), token.Token);
            Assert.AreEqual<string>("all", token.Scope);
            Assert.AreEqual<string>("guest", token.User);
        }
    }
}
