using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Domain.Trading;
using Quantum.Infrastructure.Trading.Repository;

namespace Quantum.Trading.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateAccountTest()
        {
            Broker broker = new Broker();
            AccountData account = broker.CreateAccount("test");

            Assert.IsNotNull(account.Id);
            Assert.AreEqual("test", account.Name);
        }
    }
}
