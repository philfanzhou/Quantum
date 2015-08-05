using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Core.Application.UserContext.Test.Service
{
    [TestClass]
    public class RegisterServiceTest
    {
        private static RegisterService service = new RegisterService();

        [TestMethod]
        public void TestRegistration()
        {
            bool result = service.register("guset@bala.com", "123456");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestActivation()
        {            
            bool result = service.register("guset@bala.com", "123456");
            Assert.IsTrue(result);
            result = service.active("guset@bala.com");
            Assert.IsTrue(result);
        }

    }
}
