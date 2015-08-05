namespace Core.Application.UserContext
{
    using Core.Domain.UserContext;
    using Core.Application.UserContext.Dto;

    public class RegisterService
    {
        private static UserRegisterAppService<UserDto, User> registerService = new UserRegisterAppService<UserDto,User>();
        private static UserManageAppService manageService = new UserManageAppService();

        public bool register(string userMail, string password)
        {
            try
            {
                UserDto dto = registerService.RegisterUser(userMail, password);
                if (dto != null)
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        public bool active(string userMail)
        {
            try
            {
                UserDto dto = registerService.GetRegisteredUser(userMail);
                if (dto != null)
                {
                    if (dto.IsActive)
                    {
                        return true;
                    }
                    else
                    {
                        return manageService.ActivateUser(dto.Id);
                    }
                }                
            }
            catch { }
            return false;
        }
    }
}
