using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Impl.Crosscutting;

namespace Core.Framework.Test.Service
{
    [TestClass]
    public class CaptchaTest
    {
        [TestMethod]
        public void Captcha()
        {
            ICaptcha captcha = ContainerHelper.Resolve<ICaptcha>();
            string key = "Mail_balabala@bala.com";
            string code = captcha.GenerateCaptcha(key);
            Assert.IsFalse(string.IsNullOrEmpty(code));
            Assert.IsTrue(captcha.VerifyCaptcha(key, code));
        }
    }
}
