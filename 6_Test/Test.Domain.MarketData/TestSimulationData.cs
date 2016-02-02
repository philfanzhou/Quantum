using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ore.Infrastructure.MarketData;
using Quantum.Domain.MarketData;
using System;

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
        }
    }
}
