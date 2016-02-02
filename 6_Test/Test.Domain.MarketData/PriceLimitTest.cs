using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Domain.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain.MarketData
{
    [TestClass]
    public class PriceLimitTest
    {
        [TestMethod]
        public void TestStockUpLimit()
        {
            Assert.IsTrue(PriceLimit.StockUpLimit(66.88) - 73.57 < 0.0000000001);
        }

        [TestMethod]
        public void TestStockDownLimit()
        {
            Assert.IsTrue(PriceLimit.StockDownLimit(66.88) - 60.19 < 0.0000000001);
        }

        [TestMethod]
        public void TestFundUpLimit()
        {
            Assert.IsTrue(PriceLimit.FundUpLimit(0.713) - 0.784 < 0.0000000001);
        }

        [TestMethod]
        public void TestFundDownLimit()
        {
            Assert.IsTrue(PriceLimit.FundDownLimit(0.713) - 0.642 < 0.0000000001);
        }
    }
}
