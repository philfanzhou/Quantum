using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ore.Infrastructure.MarketData;
using Quantum.Domain.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain.MarketData
{
    [TestClass]
    public class TestSimulationData
    {
        [TestMethod]
        public void TestGetSimulationData()
        {
            var data = Simulation.GetKLine(KLineType.Day, DateTime.Now, 100000);
            Assert.IsNotNull(data);
        }
    }
}
