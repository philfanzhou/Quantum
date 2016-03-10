using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Application.Simulation;
using Ore.Infrastructure.MarketData;

namespace Test.Application.Simulation
{
    [TestClass]
    public class TestSimulation
    {
        [TestMethod]
        public void TestWorkFlow()
        {
            var security = new Security
            {
                Code = "600036",
                Market = Market.XSHG,
                ShortName = "招商银行",
                Type = SecurityType.Sotck
            };

            var battlefield = new Battlefield(security);

            battlefield.Practice(DateTime.Now.AddDays(-15), DateTime.Now);
        }
    }
}
