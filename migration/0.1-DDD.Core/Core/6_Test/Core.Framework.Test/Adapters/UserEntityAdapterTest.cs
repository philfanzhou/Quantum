using Core.Application;
using Core.Application.UserContext.Dto;
using Core.Domain.UserContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extension.Application.UserContext.Test.Adapters
{
    [TestClass]
    public class UserEntityAdapterTest
    {
        [TestMethod]
        public void UserEntityToUserDto()
        {
            UserFactory factory = new UserFactory();
            User user = factory.CreateUser("username", "password");

            UserDto dto = user.ProjectedAs<UserDto>();

            Assert.AreEqual(user.Id, dto.Id);
            Assert.AreEqual(user.UserMail, dto.UserMail);
            Assert.AreEqual(string.Empty, dto.EncryptedPassword);
        }
    }
}
