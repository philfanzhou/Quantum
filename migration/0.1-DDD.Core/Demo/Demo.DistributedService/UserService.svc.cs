namespace Demo.DistributedService
{
    using Application.Dto;
using Core.DistributedServices;
using Extension.Application.UserContext;
using System;
using System.ServiceModel;

    [ApplicationErrorHandlerAttribute] // manage all unhandled exceptions
    [UnityInstanceProviderServiceBehavior] //create instance and inject dependencies using unity container
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class UserService : IUserService
    {
        #region Members

        private readonly IUserLoginAppService<DemoUserDto> _userLoginAppService;

        private readonly IUserRegisterAppService<DemoUserDto> _userRegisterAppService;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of user service
        /// </summary>
        public UserService(IUserLoginAppService<DemoUserDto> userLoginAppService,
            IUserRegisterAppService<DemoUserDto> userRegisterAppService)
        {
            if (userLoginAppService == null)
                throw new ArgumentNullException("userLoginAppService");

            if (userRegisterAppService == null)
                throw new ArgumentNullException("userRegisterAppService");

            _userLoginAppService = userLoginAppService;
            _userRegisterAppService = userRegisterAppService;
        }

        #endregion

        #region IUserService Members

        public DemoUserDto Login(string userName, string passWord)
        {
            return _userLoginAppService.Login(userName, passWord);
        }

        public DemoUserDto RegisterUser(string userName, string passWord)
        {
            return _userRegisterAppService.RegisterUser(userName, passWord);
        }

        //public DemoUserDto RegisterUser(DemoUserDto userDto)
        //{
        //    return _userRegisterAppService.RegisterUser(userDto);
        //}

        public bool UserExisted(string userName)
        {
            //return _userRegisterAppService.UserExisted(userName);
            throw new NotImplementedException();
        }

        #endregion

        #region IDispose Members

        public void Dispose()
        {
            //_userLoginAppService.Dispose();
        }
        
        #endregion
    }
}
