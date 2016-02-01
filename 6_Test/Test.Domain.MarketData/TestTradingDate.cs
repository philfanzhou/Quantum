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
    public class TestTradingDate
    {
        [TestMethod]
        public void TestSaturdayAndSunday()
        {
            // 周六
            Assert.IsFalse(new DateTime(2016, 1, 30).IsTradingDate());
            // 周日
            Assert.IsFalse(new DateTime(2016, 1, 31).IsTradingDate());
            // 周一
            Assert.IsTrue(new DateTime(2016, 2, 1).IsTradingDate());
            // 周二
            Assert.IsTrue(new DateTime(2016, 2, 2).IsTradingDate());
            // 周三
            Assert.IsTrue(new DateTime(2016, 2, 3).IsTradingDate());
            // 周四
            Assert.IsTrue(new DateTime(2016, 2, 4).IsTradingDate());
            // 周五
            Assert.IsTrue(new DateTime(2016, 2, 5).IsTradingDate());
        }

        [TestMethod]
        public void TestGetTradingDate()
        {
            DateTime date = new DateTime(2016, 1, 30).GetNearestTradingDate();

            Assert.AreEqual(new DateTime(2016, 2, 1), date);
            Assert.IsTrue(date.IsTradingDate());
        }
    }
}
