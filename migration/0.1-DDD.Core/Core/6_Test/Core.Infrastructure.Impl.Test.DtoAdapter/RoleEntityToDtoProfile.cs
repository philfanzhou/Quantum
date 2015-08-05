namespace Core.Infrastructure.Impl.Test.DtoAdapter
{
    using AutoMapper;
    using Core.Application.UserContext.Dto;
    using Core.Domain.UserContext;

    public class RoleEntityToDtoProfile<TRoleEntity, TRoleDto> : Profile
        where TRoleEntity : Role
        where TRoleDto : RoleDto
    {
        protected override void Configure()
        {
            this.CreateMap<TRoleEntity, TRoleDto>();
        }
    }
}