namespace Core.Application.UserContext
{
    using Core.Domain;
    using Domain.UserContext;
    using Dto;
    using Infrastructure.Crosscutting;
    using System;

    public class UserRegisterAppService<TUserDto, TUserEntity>
        where TUserDto : UserDto, new()
        where TUserEntity : User
    {
        public virtual bool UserNameExists(string userMail)
        {
            if (string.IsNullOrWhiteSpace(userMail))
            {
                throw new ArgumentNullException("userMail");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRepository<TUserEntity>>();
                return repository.UserMailExists(userMail);
            }
        }

        public virtual TUserDto GetRegisteredUser(string userMail)
        {
            if (string.IsNullOrWhiteSpace(userMail))
            {
                throw new ArgumentNullException("userMail");
            }


            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRepository<TUserEntity>>();
                if (!repository.UserMailExists(userMail))
                {
                    return null;
                }
                TUserEntity user = repository.GetByMail(userMail);
                TUserDto userDto = user.ProjectedAs<TUserDto>();
                return userDto;
            }
        }

        public virtual TUserDto RegisterUser(string userMail, string password)
        {
            if (string.IsNullOrWhiteSpace(userMail))
            {
                throw new ArgumentException("The user e-mail cannot be blank.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("password");
            }

            if (UserNameExists(userMail))
            {
                throw new ArgumentException("The user e-mail already exists.", "userName");
            }

            using (var context = RepositoryContext.Create())
            {
                var userFactory = ContainerHelper.Instance.Resolve<IUserFactory<TUserEntity>>();
                TUserEntity user = userFactory.CreateUser(userMail, password);

                context.UnitOfWork.RegisterNew(user);
                context.UnitOfWork.Commit();
                return user.ProjectedAs<TUserDto>();
            }
        }
    }
}