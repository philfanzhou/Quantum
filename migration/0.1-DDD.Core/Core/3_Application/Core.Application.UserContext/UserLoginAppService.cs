namespace Core.Application.UserContext
{
    using Core.Domain;
    using Domain.UserContext;
    using Dto;
    using System;

    public class UserLoginAppService<TUserDto, TUserEntity>
        where TUserDto : UserDto, new()
        where TUserEntity : User
    {
        public virtual bool Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRepository<TUserEntity>>();
                return repository.CheckPassword(userName, password);
            }
        }

        public virtual TUserDto GetUserInfo(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRepository<TUserEntity>>();
                if (!repository.UserMailExists(userName))
                {
                    return null;
                }
                if (repository.CheckPassword(userName, password) == false)
                {
                    return null;
                }

                TUserEntity user = repository.GetByMail(userName);
                if (user.IsActive == false)
                {
                    return null;
                }

                TUserDto userDto = user.ProjectedAs<TUserDto>();
                return userDto;
            }
        }
    }
}