namespace Core.Domain.UserContext
{
    using Core.Infrastructure.Crosscutting;
    using Domain;
    using System;


    public class User : Entity, IAggregateRoot
    {
        #region Constructor

        protected User()
        {
        }

        public User(Guid id) : base(id.ToString())
        {
        }

        #endregion

        #region Properties

        public string UserMail { get; set; }

        public string EncryptedPassword { get; internal set; }

        public string Salt { get; internal set; }

        public bool IsActive { get; set; }

        #endregion

        #region Method

        public virtual bool ChangePassword(string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword))
            {
                throw new ArgumentNullException("oldPassword");
            }

            this.Salt = Guid.NewGuid().ToString("N").Substring(0, 8);
            var encryptor = ContainerHelper.Instance.Resolve<IMd5Encryptor>();
            this.EncryptedPassword = encryptor.Encrypt(newPassword + this.Salt);
            return true;
        }

        public virtual bool CheckPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var encryptor = ContainerHelper.Instance.Resolve<IMd5Encryptor>();

            return this.EncryptedPassword.Equals(encryptor.Encrypt(password + this.Salt));
        }

        #endregion
    }
}
