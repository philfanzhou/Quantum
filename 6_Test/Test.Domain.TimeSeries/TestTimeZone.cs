using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ore.Infrastructure.MarketData;
using Quantum.Domain.TimeSeries;
using System.Collections.Generic;
using System.Linq;

namespace Test.Domain.TimeSeries
{
    [TestClass]
    public class TestTimeZone
    {
        [TestMethod]
        public void TestGetTimeZone()
        {
            Min1KLineCollections collections = new Min1KLineCollections();
            var zones = collections.GetZoneForTest(
                new DateTime(2016, 2, 16, 8, 30, 0),
                new DateTime(2016, 2, 16, 9, 45, 35)).ToList();

            Assert.AreEqual(
                new DateTime(2016, 2, 16, 9, 46, 0),
                zones.Last().EndTime);

            //todo：这个UT在debug模式的时候还有问题，以后处理
        }
    }

    internal class Min1KLineCollections : Min1Collections<IStockKLine>
    {
        internal IEnumerable<ITimeZone> GetZoneForTest(DateTime startTime, DateTime endTime)
        {
            return base.GetTimeZone(startTime, endTime);
        }
    }
}
