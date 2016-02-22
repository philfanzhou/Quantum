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

        [TestMethod]
        public void TestTrading()
        {
            IAccount account = Broker.CreateAccount("MyAccount");
            account.TransferIn(100000);
            DateTime time = new DateTime(2016, 2, 22, 9, 35, 0);
            string stockCode = "600036";
            ITradingRecord tradingRecord = null;
            IHoldingsRecord holdingsRecord = null;

            account.Buy(time, stockCode, 10, 100);
            tradingRecord = account.GetAllTradingRecord().Last();
            Assert.AreEqual(tradingRecord.StampDuty, 0m);
            //Assert.AreEqual(tradingRecord.TransferFees, 0.002m);
            Assert.AreEqual(tradingRecord.Commissions, 5m);
            //Assert.AreEqual(tradingRecord.GetAmount(), 5.0002m);
            holdingsRecord = account.GetHoldingsRecord(stockCode);
            Assert.AreEqual(holdingsRecord.Quantity, 100);
            Assert.AreEqual(holdingsRecord.GetAvailableQuantity(time), 0);
            Assert.AreEqual(holdingsRecord.GetFrozenQuantity(time), 100);
        }
    }
}
