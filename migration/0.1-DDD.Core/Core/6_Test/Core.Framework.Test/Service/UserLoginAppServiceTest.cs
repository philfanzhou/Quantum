using Core.Application.UserContext;
using Core.Application.UserContext.Dto;
using Core.Domain.UserContext;
using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Impl.Crosscutting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Extension.Application.UserContext.Test.Service
{
    [TestClass]
    public class UserLoginAppServiceTest
    {
        private static UserLoginAppService<UserDto, User> _loginService;
        private static UserRegisterAppService<UserDto, User> _registerService;
        private const string UserName = "loginname";
        private const string Password = "password";

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _loginService = new UserLoginAppService<UserDto, User>();

            _registerService = new UserRegisterAppService<UserDto, User>();
            _registerService.RegisterUser(UserName, ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(Password));
        }

        [TestMethod]
        public void UserLoginTest()
        {
            string pwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(Password);
            bool loginSuccess = _loginService.Login(UserName, pwd);
            Assert.IsTrue(loginSuccess);
            var dto = _loginService.GetUserInfo(UserName, pwd);
            Assert.IsNotNull(dto);
            Assert.AreEqual(UserName, dto.UserMail);
            Assert.AreEqual(string.Empty, dto.EncryptedPassword);
        }

        [TestMethod]
        public void UserLoginWithWrongUserNameTest()
        {
            string pwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("wrongPassWord");
            bool loginSuccess = _loginService.Login(UserName, pwd);
            Assert.IsFalse(loginSuccess);
            var dto = _loginService.GetUserInfo("wrongUserName", pwd);
            Assert.IsNull(dto);
        }

        [TestMethod]
        public void UserLoginWithWrongPasswordTest()
        {
            string pwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("wrongPassWord");
            bool loginSuccess = _loginService.Login(UserName, pwd);
            Assert.IsFalse(loginSuccess);
            var dto = _loginService.GetUserInfo(UserName, pwd);
            Assert.IsNull(dto);
        }

        #region Parameters test

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserLoginWithNullParam1()
        {
            _loginService.Login(null, "password");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserLoginWithNullParam2()
        {
            _loginService.Login("name", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserLoginWithEmptyParam1()
        {
            _loginService.Login(string.Empty, "password");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserLoginWithEmptyParam2()
        {
            _loginService.Login("name", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserLoginWithWhiteSpaceParam1()
        {
            _loginService.Login("     ", "password");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserLoginWithWhiteSpaceParam2()
        {
            _loginService.Login("name", "      ");
        }
        
        #endregion
    }
}
