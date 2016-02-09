using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ore.Infrastructure.MarketData;
using Quantum.Domain.MarketData;
using System;
using System.Linq;

namespace Test.Domain.MarketData
{
    [TestClass]
    public class TestSimulationData
    {
        [TestMethod]
        public void TestGetSimulationData()
        {
            var data = Simulation.CreateRandomKLines(KLineType.Day, DateTime.Now, 100000);
            Assert.IsNotNull(data);

            data = Simulation.CreateRandomKLines(KLineType.Min1, DateTime.Now, 100000);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void TestGetMin1Data()
        {
            // 构造从2月1日 -- 2月4日的数据
            // 每小时60条，每天4个小时，4天总计960条数据
            int totalCount = 60 * 4 * 4;
            var min1KLines = Simulation.CreateRandomKLines(KLineType.Min1, new DateTime(2016, 2, 1), totalCount).ToList();
            Assert.AreEqual(new DateTime(2016, 2, 4, 15, 0, 0), min1KLines.Last().Time);
        }
    }
}
