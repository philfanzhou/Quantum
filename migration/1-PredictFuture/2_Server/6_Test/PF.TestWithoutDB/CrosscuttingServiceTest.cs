using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.DistributedService;

namespace PF.TestWithoutDB
{
    [TestClass]
    public class CrosscuttingServiceTest
    {

        [TestMethod, Ignore]
        public void SeanMailTest()
        {
            CrosscuttingService service = new CrosscuttingService();
            Assert.IsTrue(service.SendMail("i@techotaku.net", "CrosscuttingService", "CrosscuttingServiceTest.SeanMailTest without SSL"));
        }

        [TestMethod]
        public void CaptchaTest()
        {
            CrosscuttingService sevice = new CrosscuttingService();
            string key = "MAil_g@bala.com";
            string code = sevice.GenerateCaptcha(key);
            Assert.IsFalse(string.IsNullOrEmpty(code));
            Assert.IsTrue(sevice.VerifyCaptcha(key, code));
        }
    }
}
