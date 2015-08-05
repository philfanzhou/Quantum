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
    public class UserRegisterAppServiceTest
    {
        private static UserRegisterAppService<UserDto, User> _service;
        private const string UserName = "username";

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _service = new UserRegisterAppService<UserDto, User>();

            UserDto dto = _service.RegisterUser(UserName, ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("password"));
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Id.ToString());
            Assert.AreEqual(UserName, dto.UserMail);
            Assert.AreEqual(string.Empty, dto.EncryptedPassword);
        }

        #region UserRegister

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UserRegisterNameExisted()
        {
            _service.RegisterUser(UserName, ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("password"));
        }

        [TestMethod]
        public void UserRegister()
        {
            UserDto dto = _service.RegisterUser("admin", ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("password"));
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Id.ToString());
            Assert.AreEqual("admin", dto.UserMail);
            Assert.AreEqual(string.Empty, dto.EncryptedPassword);

            dto = _service.RegisterUser("中文", ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("password"));
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Id.ToString());
            Assert.AreEqual("中文", dto.UserMail);
            Assert.AreEqual(string.Empty, dto.EncryptedPassword);

            dto = _service.RegisterUser("admin123", ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("admin123"));
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Id.ToString());
            Assert.AreEqual("admin123", dto.UserMail);
            Assert.AreEqual(string.Empty, dto.EncryptedPassword);

            dto = _service.RegisterUser("admin12345", ContainerHelper.Resolve<IMd5Encryptor>().Encrypt("2233@#$"));
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Id.ToString());
            Assert.AreEqual("admin12345", dto.UserMail);
            Assert.AreEqual(string.Empty, dto.EncryptedPassword);
        }
        
        #endregion

        #region InvalidUserName

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserWithNullParam1()
        {
            _service.RegisterUser(null, "password");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserWithEmptyParam1()
        {
            _service.RegisterUser(string.Empty, "password");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserWithWhiteSpaceParam1()
        {
            _service.RegisterUser("     ", "password");
        }

        [TestMethod, Ignore]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidUserName1()
        {
            _service.RegisterUser("张三疯％", "password");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidUserName2()
        {
            _service.RegisterUser("1", "password");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidUserName3()
        {
            _service.RegisterUser("中", "password");
        }

        [TestMethod, Ignore]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidUserName4()
        {
            _service.RegisterUser("123456789012345678901", "password");
        }

        #endregion

        #region InvalidPassword

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserWithNullParam2()
        {
            _service.RegisterUser("name", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserWithEmptyParam2()
        {
            _service.RegisterUser("name", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserWithWhiteSpaceParam2()
        {
            _service.RegisterUser("name", "      ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidPassword1()
        {
            _service.RegisterUser("admin1", "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidPassword2()
        {
            // length
            _service.RegisterUser("admin2", "123456789012345678901234567890123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidPassword3()
        {
            _service.RegisterUser("admin3", "中文中文中文");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidPassword4()
        {
            // 全角字符
            _service.RegisterUser("admin4", "｛｝");
        }

        #endregion
    }
}
