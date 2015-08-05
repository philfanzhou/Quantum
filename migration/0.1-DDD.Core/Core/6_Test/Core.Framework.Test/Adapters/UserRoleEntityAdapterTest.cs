using Core.Application;
using Core.Application.UserContext.Dto;
using Core.Domain.UserContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Extension.Application.UserContext.Test.Adapters
{
    [TestClass]
    public class UserRoleEntityAdapterTest
    {
        [TestMethod]
        public void UserRoleEntityToUserRoleDto()
        {
            UserRole userRole = new UserRole(Guid.NewGuid());
            userRole.UserId = Guid.NewGuid().ToString();
            userRole.RoleId = Guid.NewGuid().ToString();

            UserRoleDto dto = userRole.ProjectedAs<UserRoleDto>();

            Assert.AreEqual(userRole.Id, dto.Id);
            Assert.AreEqual(userRole.UserId, dto.UserId);
            Assert.AreEqual(userRole.RoleId, dto.RoleId);
        }
    }
}