namespace Core.Application.UserContext
{
    using Core.Domain;
    using Domain.UserContext;
    using Dto;
    using Infrastructure.Crosscutting;
    using System;

    public class RoleManageAppService
    {
        public RoleDto AddRole(string roleName, string description)
        {
            var idGenerator = ContainerHelper.Resolve<IIdentityGenerator>();
            Role role = new Role(idGenerator.NewGuid())
                {
                    Name = roleName,
                    Description = description
                };

            var validator = ContainerHelper.Resolve<IEntityValidator>();
            if (validator.IsValid(role) == false)
            {
                throw new ApplicationValidationException(validator.GetInvalidMessages(role));
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<RoleRepository>();
                bool existedSameNameRole = repository.Exists(roleName);
                if (existedSameNameRole)
                {
                    throw new ApplicationOperationException("Already existed same name role.");
                }

                repository.Add(role);
                context.UnitOfWork.Commit();
                return role.ProjectedAs<RoleDto>();
            }
        }

        public void DeleteRole(string roleId)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<RoleRepository>();
                Role role = repository.Get(roleId);
                var userContextDomainService = new UserContexDomainService();
                userContextDomainService.DeleteRole(role);
            }
        }

        public void UpdateRole(RoleDto roleDto)
        {
            throw new NotImplementedException();
        }
    }
}