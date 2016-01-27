using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Domain.Trading;
using System.Linq;

namespace Test.Domain.Trading
{
    [TestClass]
    public class TradingTest
    {
        [TestMethod]
        public void TestLoadAndSave()
        {
            IAccount account = Broker.CreateAccount("MyAccount");
            account.TransferIn(100000);
            account.Buy(DateTime.Now, "600036", 15.98, 1000);

            // remember ID
            string id = account.Id;

            // SaveData
            Broker.SaveAccountData(account);

            // ReadData
            IAccount readAccount = Broker.LoadAccountData(id);

            Assert.AreEqual(id, account.Id);
            Assert.IsTrue(account.Name == "MyAccount");
            Assert.IsTrue(account.GetAllHoldingsRecord().ToList()[0].StockCode == "600036");
            var tradingRecord = account.GetAllTradingRecord().ToList()[0];
            Assert.IsTrue(tradingRecord.StockCode == "600036");
            Assert.IsTrue(tradingRecord.Quantity == 1000);
            Assert.IsTrue(tradingRecord.Price == 15.98);
        }
    }
}
