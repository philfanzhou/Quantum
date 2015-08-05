namespace Core.Application.UserContext
{
    using Core.Domain;
    using Domain.UserContext;
    using System;

    public class UserManageAppService
    {
        public bool DeactivateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRepository<User>>();
                User user = repository.Get(userId);
                if (null == user)
                {
                    return false;
                }

                user.IsActive = false;
                repository.Update(user);

                context.UnitOfWork.Commit();

                return user.IsActive.Equals(false);
            }
        }

        public bool ActivateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRepository<User>>();
                User user = repository.Get(userId);
                if (null == user)
                {
                    return false;
                }

                user.IsActive = true;
                repository.Update(user);

                context.UnitOfWork.Commit();

                return user.IsActive;
            }
        }

        public bool AssignRole(string userId, string roleId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId");
            }
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentNullException("roleId");
            }

            using (var context = RepositoryContext.Create())
            {
                var roleRepository = context.GetRepository<RoleRepository>(); 
                var userRepository = context.GetRepository<UserRepository<User>>();
                User user = userRepository.Get(userId);
                Role role = roleRepository.Get(roleId);

                var domainService = new UserContexDomainService();
                return domainService.AssignRole(user, role);
            }
        }

        public bool UnassignRole(string userId, string roleId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId");
            }
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentNullException("roleId");
            }

            using (var context = RepositoryContext.Create())
            {
                var roleRepository = context.GetRepository<RoleRepository>();
                var userRepository = context.GetRepository<UserRepository<User>>();
                User user = userRepository.Get(userId);
                Role role = roleRepository.Get(roleId);

                var domainService = new UserContexDomainService();
                return domainService.UnassignRole(user, role);
            }
        }

        public void UnassignAllRole(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRepository<User>>();
                User user = repository.Get(userId);

                var domainService = new UserContexDomainService();
                domainService.UnassignAllRole(user);
            }
        }
    }
}