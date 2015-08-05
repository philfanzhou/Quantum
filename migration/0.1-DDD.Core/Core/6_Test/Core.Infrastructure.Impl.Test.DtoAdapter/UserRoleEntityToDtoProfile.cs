namespace Core.Infrastructure.Impl.Test.DtoAdapter
{
    using AutoMapper;
    using Core.Application.UserContext.Dto;
    using Core.Domain.UserContext;

    public class UserRoleEntityToDtoProfile<TUserRoleEntity, TUserRoleDto> : Profile
        where TUserRoleEntity : UserRole
        where TUserRoleDto : UserRoleDto
    {
        protected override void Configure()
        {
            this.CreateMap<TUserRoleEntity, TUserRoleDto>();
        }
    }
}