using AutoMapper;
using Core.Application.UserContext.Dto;
using Core.Domain.UserContext;

namespace Core.Infrastructure.Impl.Test.DtoAdapter
{
    public class TypeMappingInitialize
    {
        public static void Init()
        {
            // DTO Mapper
            Mapper.Initialize(x =>
            {
                x.AddProfile<UserEntityToDtoProfile<User, UserDto>>();
                x.AddProfile<RoleEntityToDtoProfile<Role, RoleDto>>();
                x.AddProfile<UserRoleEntityToDtoProfile<UserRole, UserRoleDto>>();
            });
        }
    }
}
