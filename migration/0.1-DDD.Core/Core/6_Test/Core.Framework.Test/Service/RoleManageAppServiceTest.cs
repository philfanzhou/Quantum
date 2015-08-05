using Core.Application.UserContext;
using Core.Domain;
using Core.Domain.UserContext;
using Core.Infrastructure.Crosscutting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extension.Application.UserContext.Test.Service
{
    [TestClass]
    public class RoleManageAppServiceTest
    {
        //private static IRoleRepository _roleRepository;
        //private static IUserRoleRepository _userRoleRepository;
        //private static IUserRepository<User> _userRepository;
        //private static IUserContexDomainService _domainService;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            //UserRepositoryContext uow = new UserRepositoryContext();
            //_roleRepository = new RoleRepository(uow);
            //_userRoleRepository = new UserRoleRepository(uow);
            //_userRepository = new UserRepository<User>(uow);
            //_domainService = new UserContexDomainService();
        }

        #region AddRoleTest

        #region Parameters Test
        
        // 设置的名称不符合规范

        // 设置的名称已经存在
        
        #endregion

        [TestMethod]
        public void AddRoleTest()
        {
            var roleManageService = new RoleManageAppService();
            var role = roleManageService.AddRole("管理员", string.Empty);
            Assert.IsNotNull(role);
            Assert.AreNotEqual(string.Empty, role.Id);
        }

        #endregion

        #region DeleteRoleTest

        [TestMethod]
        public void DeleteRoleTest()
        {
            var roleManageService = new RoleManageAppService();
            var role = roleManageService.AddRole("TestRole", string.Empty); 
            Assert.IsNotNull(role);

            string roleId = role.Id;

            using (var context = RepositoryContext.Create())
            {
                var roleRepository = context.GetRepository<RoleRepository>();
                var roleEntity = roleRepository.Get(roleId);
                Assert.IsNotNull(roleEntity);
                Assert.AreNotEqual(string.Empty, roleEntity.Id);
            }

            using (var context = RepositoryContext.Create())
            {
                var roleRepository = context.GetRepository<RoleRepository>();
                roleManageService.DeleteRole(roleId);
                var testRoleEntity = roleRepository.Get(roleId);
                Assert.IsNull(testRoleEntity);
            }
        }

        #endregion
    }
}