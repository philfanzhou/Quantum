namespace Core.Domain.UserContext
{
    using Infrastructure.Crosscutting;
    using System;

    public class UserContexDomainService
    {
        public bool AssignRole(User user, Role role)
        {
            if (null == user)
            {
                throw new ArgumentNullException("user");
            }
            if (null == role)
            {
                throw new ArgumentNullException("role");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRoleRepository>();
                bool userRoleAlreadyExists = repository.Exists(user.Id, role.Id);
                if (userRoleAlreadyExists)
                {
                    return false;
                }

                var idGenerator = ContainerHelper.Resolve<IIdentityGenerator>();
                UserRole userRole = new UserRole(idGenerator.NewGuid())
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };


                repository.Add(userRole);
                context.UnitOfWork.Commit();
                return true;
            }
        }

        public bool UnassignRole(User user, Role role)
        {
            if (null == user)
            {
                throw new ArgumentNullException("user");
            }
            if (null == role)
            {
                throw new ArgumentNullException("role");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRoleRepository>();
                var userRole = repository.GetUserRole(user.Id, role.Id);
                if (null == userRole)
                {
                    return false;
                }

                repository.Delete(userRole);
                context.UnitOfWork.Commit();
                return true;
            }
        }

        public void UnassignAllRole(User user)
        {
            if (null == user)
            {
                throw new ArgumentNullException("user");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRoleRepository>();
                var userRoles = repository.GetUserRoleByUserId(user.Id);
                foreach (UserRole userRole in userRoles)
                {
                    repository.Delete(userRole);
                }
                context.UnitOfWork.Commit();
            }
        }

        public void DeleteRole(Role role)
        {
            if (null == role)
            {
                throw new ArgumentNullException("role");
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<UserRoleRepository>();

                // first to delete the UserRole
                var userRoles = repository.GetUserRoleByRoleId(role.Id);
                foreach (var userRole in userRoles)
                {
                    repository.Delete(userRole);
                }

                // second to delete the Role
                context.UnitOfWork.RegisterDeleted(role);

                // commit change
                context.UnitOfWork.Commit();
            }
        }
    }
}