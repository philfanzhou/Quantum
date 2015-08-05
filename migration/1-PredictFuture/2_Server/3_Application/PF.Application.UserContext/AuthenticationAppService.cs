namespace PF.Application.UserContext
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Core.Application.UserContext;

    public class AuthenticationAppService
    {
        private AuthenticationService AuthService = new AuthenticationService();
        private RegisterService RegService = new RegisterService();

        public bool validate(string identity, string encryptedPassword)
        {
            return AuthService.validate(identity, encryptedPassword);
        }

        public bool register(string identity, string encryptedPassword)
        {
            return RegService.register(identity, encryptedPassword);
        }

        public bool active(string identity)
        {
            return RegService.active(identity);
        }

    }
}
