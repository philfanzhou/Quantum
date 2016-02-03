using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ore.Infrastructure.MarketData;
using Quantum.Domain.MarketData;

namespace Test.Domain.MarketData
{
    [TestClass]
    public class PriceLimitTest
    {
        [TestMethod]
        public void TestStockUpLimit()
        {
            Assert.IsTrue(PriceLimit.UpLimit(SecurityType.Sotck, 66.88) - 73.57 < 0.0000000001);
        }

        [TestMethod]
        public void TestStockDownLimit()
        {
            Assert.IsTrue(PriceLimit.DownLimit(SecurityType.Sotck, 66.88) - 60.19 < 0.0000000001);
        }

        [TestMethod]
        public void TestFundUpLimit()
        {
            Assert.IsTrue(PriceLimit.UpLimit(SecurityType.Fund, 0.713) - 0.784 < 0.0000000001);
        }

        [TestMethod]
        public void TestFundDownLimit()
        {
            Assert.IsTrue(PriceLimit.DownLimit(SecurityType.Fund, 0.713) - 0.642 < 0.0000000001);
        }
    }
}
