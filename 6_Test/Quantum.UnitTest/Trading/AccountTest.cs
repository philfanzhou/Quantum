using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Domain.Trading;
using Quantum.Infrastructure.Trading.Repository;
using Framework.Infrastructure.Repository;

namespace Quantum.UnitTest.Trading
{
    [TestClass]
    public class AccountTest
    {
        private string accountId;

        [TestInitialize]
        public void Env()
        {
            AccountData account = Broker.CreateAccount("AccountTest");
            this.accountId = account.Id;
        }

        [TestMethod]
        public void TransferInTest()
        {
            decimal balance = GetAccountBalance();
            IAccount account = new Account(this.accountId);
            decimal amount = 100000m;
            account.TransferIn(amount);

            Assert.AreEqual(balance + amount, GetAccountBalance());
        }

        [TestMethod]
        public void AvailableQuantityToBuyTest()
        {
            MakeSureBalanceMoreThan100000();
            decimal balance = GetAccountBalance();

            IAccount account = new Account(this.accountId);
            string code = "600036";
            decimal price = 17.78m;
            int quantity = account.AvailableQuantityToBuy(code, price);
            Assert.IsTrue(quantity % 100 == 0);
            Assert.IsTrue(quantity * price < balance);

            decimal amount = (quantity + 100) * price;
            decimal commission = TradeCost.GetCommission(price, quantity);
            decimal transferFees = TradeCost.GetTransferFees(code, price, quantity);
            amount = amount + commission + transferFees;
            Assert.IsTrue(amount > balance);
        }

        [TestMethod]
        public void BuyTest()
        {
            MakeSureBalanceMoreThan100000();

            IAccount account = new Account(this.accountId);
            string code = "600036";
            decimal price = 17.78m;
            int quantity = account.AvailableQuantityToBuy(code, price);

            Assert.IsTrue(account.Buy(code, price, quantity));

            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var holdingsRecord = context.GetRepository<HoldingsRecordRepository>()
                    .GetByAccountAndCode(this.accountId, code);
                Assert.IsNotNull(holdingsRecord);
                Assert.IsTrue(holdingsRecord.AccountId == this.accountId);
                Assert.IsTrue(holdingsRecord.StockCode == code);
                Assert.IsTrue(holdingsRecord.Quantity >= quantity);

                var tradingRecord = context.GetRepository<TradingRecordDataRepository>()
                    .GetByAccountAndCode(this.accountId, code).ToList();
                Assert.IsNotNull(tradingRecord);
                var lastRecord = tradingRecord[tradingRecord.Count - 1];
                Assert.IsTrue(lastRecord.AccountId == this.accountId);
                Assert.IsTrue(lastRecord.StockCode == code);
                Assert.IsTrue(lastRecord.Quantity == quantity);
                Assert.IsTrue(lastRecord.Price == price);
                Assert.IsTrue(lastRecord.Type == TradeType.Buy);
                DateTime expected = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                DateTime recordDate = lastRecord.Date;
                DateTime actual = new DateTime(recordDate.Year, recordDate.Month, recordDate.Day, recordDate.Hour, 0, 0);
                Assert.AreEqual(expected, actual);
                Assert.IsTrue(lastRecord.Commissions > 0);
                Assert.IsTrue(lastRecord.StampDuty == 0);
                Assert.IsTrue(lastRecord.TransferFees > 0);
            }
        }

        [TestMethod]
        public void AvailableQuantityToSellTest()
        {
            MakeSureBalanceMoreThan100000();

            string code = "600036";
            decimal price = 17.78m;
            int yesterdayQuantity = 3000;
            int todayQuantity1 = 500;
            int todayQuantity2 = 1000;

            IAccount account = new Account(this.accountId);

            Market.IsVirtual = true;
            Market.SetVirtualMarketTime(DateTime.Now.AddDays(-1));
            account.Buy(code, price, yesterdayQuantity);

            Market.SetVirtualMarketTime(DateTime.Now);
            account.Buy(code, price, todayQuantity1);
            account.Buy(code, price, todayQuantity2);

            int expected = yesterdayQuantity;
            int actual = account.AvailableQuantityToSell(code);

            Assert.AreEqual(expected, actual);
        }

        private decimal GetAccountBalance()
        {
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<Repository<AccountData>>();
                AccountData accountData = repository.Get(this.accountId);
                return accountData.Balance;
            }
        }

        private void MakeSureBalanceMoreThan100000()
        {
            decimal balance = GetAccountBalance();
            IAccount account = new Account(this.accountId);
            if (balance < 100000m)
            {
                account.TransferIn(100000m - balance);
            }
        }
    }
}
