using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Domain.Trading;
using Quantum.Infrastructure.Trading.Repository;

namespace Quantum.UnitTest.Trading
{
    [TestClass]
    public class BrokerTest
    {
        [TestMethod]
        public void CreateAccountTest()
        {
            string name = "test";
            AccountData account = Broker.CreateAccount(name);

            Assert.IsNotNull(account.Id);
            Assert.AreEqual(name, account.Name);
        }

        [TestMethod]
        public void GetAccountTest()
        {
            string name = "test2";
            AccountData account = Broker.CreateAccount("test2");
            AccountData accountFromDb = Broker.GetAccount(account.Id);

            Assert.IsNotNull(accountFromDb);
            Assert.AreEqual(account.Id, accountFromDb.Id);
            Assert.AreEqual(name, accountFromDb.Name);
        }
    }
}
