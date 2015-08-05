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
    public class UserSecurityAppServiceTest
    {
        private static UserLoginAppService<UserDto, User> _loginService;
        private static UserRegisterAppService<UserDto, User> _registerService;
        private static UserSecurityAppService _securityService;
        private static string _userId;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _loginService = new UserLoginAppService<UserDto, User>();
            _registerService = new UserRegisterAppService<UserDto, User>();
            _securityService = new UserSecurityAppService();

            // 注册一个测试使用账号
            string username = "ChangePassword112";
            string password = "111111";
            _userId = _registerService.RegisterUser(username, ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(password)).Id;
        }

        #region EncryptPassword

        [TestMethod]
        public void EncryptPasswordTest()
        {
            IMd5Encryptor encryptor = ContainerHelper.Resolve<IMd5Encryptor>();
            string pwd = encryptor.Encrypt("1");
            Assert.AreEqual("C4CA4238A0B923820DCC509A6F75849B", pwd);

            pwd = encryptor.Encrypt("helloworld");
            Assert.AreEqual("FC5E038D38A57032085441E7FE7010B0", pwd);

            pwd = encryptor.Encrypt("33487343");
            Assert.AreEqual("6FCBCD2575DA2F0A54DB4110D118E523", pwd);

            pwd = encryptor.Encrypt("你好");
            Assert.AreEqual("7ECA689F0D3389D9DEA66AE112E5CFD7", pwd);
        }
        
        #endregion

        #region ChangePassword

        [TestMethod]
        public void ChangePasswordTest()
        {
            // 注册一个测试使用账号
            string username = "ChangePasswordUser";
            string password = "111111";
            _registerService.RegisterUser(username, ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(password));

            // 先进行一次登录测试
            string encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(password);
            var dto = _loginService.GetUserInfo(username, encryptedPwd);
            Assert.IsNotNull(dto);
            Assert.AreEqual(username, dto.UserMail);

            // 修改用户密码
            password = "222222";
            bool result = _securityService.ChangePassword(dto.Id.ToString(), encryptedPwd, ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(password));
            Assert.IsTrue(result);

            // 用之前的密码已经无法登录
            dto = _loginService.GetUserInfo(username, encryptedPwd);
            Assert.IsNull(dto);

            // 用新密码成功登录
            encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(password);
            dto = _loginService.GetUserInfo(username, encryptedPwd);
            Assert.IsNotNull(dto);
            Assert.AreEqual(username, dto.UserMail);
        }

        [TestMethod]
        public void ChangePasswordWithInvalidUserId()
        {
            // 用户ID不存在
            string encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("123456");
            bool result = _securityService.ChangePassword(new Guid().ToString(), encryptedPwd, "12345678");
            Assert.IsFalse(result);
        }

        [TestMethod, Ignore]
        public void ChangePasswordWithInvalidUserPassword()
        {
            // 注册一个测试使用账号
            string username = "ChangePassword111";
            string password = "111111";
            _registerService.RegisterUser(username, ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(password));

            // 先进行一次登录测试
            string encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(password);
            var dto = _loginService.GetUserInfo(username, encryptedPwd);
            Assert.IsNotNull(dto);
            Assert.AreEqual(username, dto.UserMail);

            // 使用错误的密码进行密码修改操作
            encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("1234235");
            password = "0987668";
            bool result = _securityService.ChangePassword(dto.Id.ToString(), encryptedPwd, password);
            Assert.IsFalse(result);

            // 使用未成功修改的密码，也无法进行登录
            encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt(password);
            dto = _loginService.GetUserInfo(username, encryptedPwd);
            Assert.IsNull(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangePasswordWithInvalidParam1()
        {
            _securityService.ChangePassword(string.Empty, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangePasswordWithInvalidParam2()
        {
            _securityService.ChangePassword(_userId.ToString(), string.Empty, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChangePasswordWithInvalidParam3()
        {
            _securityService.ChangePassword(_userId.ToString(), "    ", null);
        }

        [TestMethod, Ignore]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangePasswordWithInvalidParam4()
        {
            // 新密码不符合要求
            string encryptedPwd = ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("123456");
            _securityService.ChangePassword(_userId.ToString(), encryptedPwd, "123");
        }

        #endregion
    }
}
