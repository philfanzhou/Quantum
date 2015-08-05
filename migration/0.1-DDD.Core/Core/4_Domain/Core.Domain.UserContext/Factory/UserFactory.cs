namespace Core.Domain.UserContext
{
    using Infrastructure.Crosscutting;
    using System;

    public class UserFactory : IUserFactory<User>
    {
        public User CreateUser(string userMail, string password, params object[] objParam)
        {
            string errorMessage = string.Empty;

            if (!Validator.ValidateUserMail(userMail, ref errorMessage))
            {
                throw new ArgumentException(errorMessage, "userMail");
            }

            errorMessage = string.Empty;
            if (!Validator.ValidatePassword(password, ref errorMessage))
            {
                throw new ArgumentException(errorMessage, "password");
            }

            var idGenerator = ContainerHelper.Instance.Resolve<IIdentityGenerator>();
            var encryptor = ContainerHelper.Instance.Resolve<IMd5Encryptor>();

            string salt = Guid.NewGuid().ToString("N").Substring(0, 8);

            User user = new User(idGenerator.NewGuid())
            {
                UserMail = userMail,
                EncryptedPassword = encryptor.Encrypt(password + salt),
                Salt = salt,
                IsActive = false
            };

            return user;
        }
    }
}
