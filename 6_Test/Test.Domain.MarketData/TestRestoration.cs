using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ore.Infrastructure.MarketData;
using Pitman.RESTful.Client;
using Quantum.Domain.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain.MarketData
{
    [TestClass]
    public class TestRestoration
    {
        [TestMethod]
        public void TestPreRestoration()
        {
            ClientApi clientApi = new ClientApi("http://quantum1234.cloudapp.net:6688/api");
            IEnumerable<IStockBonus> bonus = clientApi.GetStockBonus("002498");
            IEnumerable<IStockKLine> kLines = clientApi.GetStockKLine(KLineType.Day, "002498", new DateTime(2013, 8, 1), new DateTime(2016, 2, 1));
            IStockKLine testDayKLine = kLines.FirstOrDefault(p => (p.Time.Year == 2013 && p.Time.Month == 8 && p.Time.Day == 27));
            IStockKLine newStockKLine = testDayKLine.KLinePreRestoration(bonus);
            Assert.AreEqual(1.84, newStockKLine.Close);
        }
    }
}
