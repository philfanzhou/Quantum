using Core.Application.UserContext;
using Core.Application.UserContext.Dto;
using Core.Domain.UserContext;
using Core.Infrastructure.Crosscutting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Extension.Application.UserContext.Test.Service
{
    [TestClass]
    public class UserManageAppServiceTest
    {
        private static UserLoginAppService<UserDto, User> _loginService;
        private static UserRegisterAppService<UserDto, User> _registerService;
        private static UserManageAppService _manageService;
        private const string UserName = "testActiveloginname";
        private const string Password = "password123~";
        private static string _userId;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _loginService = new UserLoginAppService<UserDto, User>();

            IUserFactory<User> factory = new UserFactory();
            _registerService = new UserRegisterAppService<UserDto, User>();
            var dto = _registerService.RegisterUser(UserName, ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(Password));
            _userId = dto.Id;

            _manageService = new UserManageAppService();
        }

        [TestMethod]
        public void DeactivateUserTest()
        {
            _manageService.DeactivateUser(_userId.ToString());

            // 虽然能通过用户名与密码的验证，但是无法获取用户信息
            string encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(Password);
            bool loginSuccess = _loginService.Login(UserName, encryptedPwd);
            Assert.IsTrue(loginSuccess);
            var dto = _loginService.GetUserInfo(UserName, encryptedPwd);
            Assert.IsNull(dto);

            // 再次关闭已关闭账户无异常
            _manageService.DeactivateUser(_userId.ToString());
            loginSuccess = _loginService.Login(UserName, encryptedPwd);
            Assert.IsTrue(loginSuccess);
            dto = _loginService.GetUserInfo(UserName, encryptedPwd);
            Assert.IsNull(dto);
        }

        [TestMethod]
        public void ActivateUserTest()
        {
            _manageService.ActivateUser(_userId.ToString());

            // 用户激活之后可登陆
            string encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(Password);
            bool loginSuccess = _loginService.Login(UserName, encryptedPwd);
            Assert.IsTrue(loginSuccess);
            var dto = _loginService.GetUserInfo(UserName, encryptedPwd);
            Assert.IsNotNull(dto);
            Assert.AreEqual(_userId, dto.Id);
            Assert.AreEqual(UserName, dto.UserMail);

            //再次激活已激活账户无异常
            _manageService.ActivateUser(_userId.ToString());
            loginSuccess = _loginService.Login(UserName, encryptedPwd);
            Assert.IsTrue(loginSuccess);
            dto = _loginService.GetUserInfo(UserName, encryptedPwd);
            Assert.IsNotNull(dto);
            Assert.AreEqual(_userId, dto.Id);
            Assert.AreEqual(UserName, dto.UserMail);
        }

        [TestMethod]
        public void ActivateUserWithInvalidParamTest()
        {
            bool result = _manageService.ActivateUser(new Guid().ToString());
            Assert.IsFalse(result);
        }
    }
}
