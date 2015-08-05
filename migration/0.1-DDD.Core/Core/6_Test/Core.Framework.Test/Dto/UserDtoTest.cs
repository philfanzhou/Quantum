using Core.Application.UserContext.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Extension.Application.UserContext.Test.Dto
{
    [TestClass]
    public class UserDtoTest
    {
        [TestMethod]
        public void ToStringTest()
        {
            UserDto dto = new UserDto();
            Assert.IsNotNull(dto.ToString());

            Guid id = Guid.NewGuid();
            string userName = "test";
            dto.Id = id.ToString();
            dto.UserMail = userName;
            Assert.AreEqual(id.ToString() + " " + userName, dto.ToString());
        }
    }
}