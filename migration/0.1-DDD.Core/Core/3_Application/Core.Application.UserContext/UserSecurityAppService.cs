namespace Core.Application.UserContext
{
    using Core.Domain;
    using Domain.UserContext;
    using System;

    public class UserSecurityAppService
    {
        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRepository<User>>();
                User user = repository.Get(userId);

                if (null != user)
                {
                    /*
                    string errorMsg = string.Empty;
                    bool result = Validator.ValidatePassword(newPassword, ref errorMsg);
                    if (result == false)
                    {
                        throw new ArgumentException(errorMsg, "newPassword");
                    }
                    */

                    if (user.ChangePassword(oldPassword, newPassword))
                    {
                        repository.Update(user);
                        context.UnitOfWork.Commit();
                        return true;
                    }
                }

                return false;
            }
        }
    }
}