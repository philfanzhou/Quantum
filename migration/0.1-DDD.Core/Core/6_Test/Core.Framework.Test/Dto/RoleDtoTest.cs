using Core.Application.UserContext.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Extension.Application.UserContext.Test.Dto
{
    [TestClass]
    public class RoleDtoTest
    {
        [TestMethod]
        public void ToStringTest()
        {
            RoleDto dto = new RoleDto();
            Assert.IsNotNull(dto.ToString());

            Guid id = Guid.NewGuid();
            string roleName = "test";
            dto.Id = id.ToString();
            dto.Name = roleName;
            Assert.AreEqual(roleName, dto.ToString());
        }
    }
}