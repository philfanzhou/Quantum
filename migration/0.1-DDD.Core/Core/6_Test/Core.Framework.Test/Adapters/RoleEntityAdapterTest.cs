using Core.Application;
using Core.Application.UserContext.Dto;
using Core.Domain.UserContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Extension.Application.UserContext.Test.Adapters
{
    [TestClass]
    public class RoleEntityAdapterTest
    {
        [TestMethod]
        public void RoleEntityToRoleDto()
        {
            Role role = new Role(Guid.NewGuid());
            role.Name = "11111111111111";
            role.Description = "222222222222";

            RoleDto dto = role.ProjectedAs<RoleDto>();

            Assert.AreEqual(role.Id, dto.Id);
            Assert.AreEqual(role.Name, dto.Name);
            Assert.AreEqual(role.Description, dto.Description);
        }
    }
}