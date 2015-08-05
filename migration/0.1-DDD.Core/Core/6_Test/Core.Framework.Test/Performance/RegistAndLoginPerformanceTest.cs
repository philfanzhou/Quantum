//using Core.Application.UserContext;
//using Core.Application.UserContext.Dto;
//using Core.Domain.UserContext;
//using Core.Domain.UserContext.Repository;
//using Core.Infrastructure.Crosscutting.Implementation;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Extension.Application.UserContext.Test.Performance
//{
//    [TestClass]
//    [Ignore]
//    public class RegistAndLoginPerformanceTest
//    {
//        private static readonly IUserFactory<User> Factory = new UserFactory();
//        private static IUserRepository<User> _userRepository;
//        private static UserLoginAppService<UserDto, User> _loginService;
//        private static UserRegisterAppService<UserDto, User> _registerService;

//        [ClassInitialize]
//        public static void Init(TestContext context)
//        {
//            UserRepositoryContext uow = new UserRepositoryContext();
//            _userRepository = new UserRepository<User>(uow);
//            IUserRoleRepository userRoleRepository = new UserRoleRepository(uow);
//            IRoleRepository roleRepository = new RoleRepository(uow);
//            IUserContexDomainService<User> _domainService = new UserContexDomainService<User>(userRoleRepository, roleRepository, _userRepository);
//            _loginService = new UserLoginAppService<UserDto, User>(_userRepository, _domainService);

//            _registerService = new UserRegisterAppService<UserDto, User>(_userRepository, Factory, _domainService);
//        }

//        [TestMethod]
//        public void RegistrationPerformanceTest()
//        {
//            // 先注册一个用户，完成EF的表结构自动创建
//            _registerService.RegisterUser("testPerformance", "testPerformance");

//            var watch = Stopwatch.StartNew();

//            List<Task> tasks = new List<Task>();
//            // 注册器个数
//            const int registerCount = 100;
//            // 每个注册器注册用户数量的步长
//            const int step = 100;
//            for (int i = 0; i < registerCount; i++)
//            {
//                UserRegister register = new UserRegister();
//                int tmpSeed = i;
//                var t = Task.Factory.StartNew(() => register.RegistUsers(tmpSeed*step, step));
//                tasks.Add(t);
//            }

//            // 注册任务完成之后，获取所消耗的时间
//            long time;
//            Task endtask = new Task(delegate
//                {
//                    time = watch.ElapsedMilliseconds;
//                });

//            Task.Factory.ContinueWhenAll(tasks.ToArray(),
//              result => endtask.Start());

//            // 等待注册用户线程完成注册
//            endtask.Wait();

//            // 检查已注册的用户是否全部成功,通过用户数量来判断
//            int usersCount = _userRepository.GetAll().Count();
//            // 注册器数量*步长 = 用户应有总数
//            Assert.IsTrue(usersCount >= registerCount * step);

//            // 随机取一个用户，测试登陆是否成功
//            Random random = new Random();
//            int usernameNum = random.Next(0, registerCount * step);
//            string username = string.Format("username{0}", usernameNum);
//            string password = string.Format("password{0}", usernameNum);
//            string encryptedPwd = new Md5Encryptor().Encrypt(password);
//            var dto = _loginService.GetUserInfo(username, encryptedPwd);
//            Assert.IsNotNull(dto);
//            Assert.AreEqual(username, dto.Username);
//            Assert.IsTrue(dto.Id != Guid.Empty.ToString());
//        }
//    }
    
//    internal class UserRegister
//    {
//        private readonly IUserFactory<User> _factory = new UserFactory();
//        private readonly IUserRepository<User> _repository;
//        private readonly UserRegisterAppService<UserDto, User> _registerService;

//        internal UserRegister()
//        {
//            UserRepositoryContext uow = new UserRepositoryContext();
//            _repository = new UserRepository<User>(uow);

//            _registerService = new UserRegisterAppService<UserDto, User>(_repository, _factory);
//        }

//        internal void RegistUsers(int seed, int maxCount)
//        {
//            for (int i = seed; i < seed + maxCount; i++)
//            {
//                string username = string.Format("username{0}", i);
//                string password = string.Format("password{0}", i);

//                _registerService.RegisterUser(username, password);
//            }
//        }
//    }
//}
