namespace Core.Infrastructure.Impl.Test.DtoAdapter
{
    using AutoMapper;
    using Core.Application.UserContext.Dto;
    using Core.Domain.UserContext;

    public class UserEntityToDtoProfile<TUserEntity, TUserDto> : Profile
        where TUserEntity : User
        where TUserDto : UserDto
    {
        //public override string ProfileName
        //{
        //    get
        //    {
        //        return "UserEntityToDtoMappings";
        //    }
        //}

        protected override void Configure()
        {
            this.CreateMap<TUserEntity, TUserDto>()
                  .ForMember(dto => dto.EncryptedPassword, opt => opt.UseValue(string.Empty));
        } 
    }
}