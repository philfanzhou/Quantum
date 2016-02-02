using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Domain.MarketData;
using System;

namespace Test.Domain.MarketData
{
    [TestClass]
    public class TestTradingDate
    {
        [TestMethod]
        public void TestIsTradingDate()
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
            // 周六
            Assert.IsFalse(new DateTime(2016, 1, 30).IsTradingTime());
            // 周日
            Assert.IsFalse(new DateTime(2016, 1, 31).IsTradingTime());

            // 周一
            Assert.IsFalse(new DateTime(2016, 2, 1, 9, 29, 59).IsTradingTime());

            Assert.IsTrue(new DateTime(2016, 2, 1, 9, 30, 0).IsTradingTime());
            Assert.IsTrue(new DateTime(2016, 2, 1, 9, 35, 0).IsTradingTime());
            Assert.IsTrue(new DateTime(2016, 2, 1, 11, 30, 0).IsTradingTime());

            Assert.IsFalse(new DateTime(2016, 2, 1, 11, 30, 1).IsTradingTime());
            Assert.IsFalse(new DateTime(2016, 2, 1, 12, 59, 59).IsTradingTime());

            Assert.IsTrue(new DateTime(2016, 2, 1, 13, 0, 0).IsTradingTime());
            Assert.IsTrue(new DateTime(2016, 2, 1, 14, 59, 0).IsTradingTime());
            Assert.IsTrue(new DateTime(2016, 2, 1, 15, 0, 0).IsTradingTime());

            Assert.IsFalse(new DateTime(2016, 2, 1, 15, 0, 1).IsTradingTime());
        }

        [TestMethod]
        public void TestToNextTradingDate()
        {
            Assert.AreEqual(
                new DateTime(1990, 12, 19),
                new DateTime().ToNextTradingDate());
            Assert.AreEqual(
                new DateTime(2016, 2, 1),
                new DateTime(2016, 1, 29).ToNextTradingDate());
            Assert.AreEqual(
                new DateTime(2016, 2, 1),
                new DateTime(2016, 1, 30).ToNextTradingDate());
            Assert.AreEqual(
                new DateTime(2016, 2, 1),
                new DateTime(2016, 1, 31).ToNextTradingDate());
            Assert.AreEqual(
                new DateTime(2016, 2, 2),
                new DateTime(2016, 2, 1).ToNextTradingDate());
        }

        [TestMethod]
        public void TestToNextTradingMinute()
        {
            Assert.AreEqual(
                new DateTime(1990, 12, 19, 9, 31, 0),
                new DateTime().ToNextTradingMinute());

            // 周五
            Assert.AreEqual(
                new DateTime(2016, 1, 29, 9, 31, 0),
                new DateTime(2016, 1, 29).ToNextTradingMinute());
            Assert.AreEqual(
                new DateTime(2016, 2, 1, 9, 31, 0),
                new DateTime(2016, 1, 29, 15, 0, 0).ToNextTradingMinute());

            // 周六
            Assert.AreEqual(
                new DateTime(2016, 2, 1, 9, 31, 0),
                new DateTime(2016, 1, 30).ToNextTradingMinute());

            // 周日
            Assert.AreEqual(
                new DateTime(2016, 2, 1, 9, 31, 0),
                new DateTime(2016, 1, 31).ToNextTradingMinute());

            Assert.AreEqual(
                new DateTime(2016, 2, 1, 9, 31, 0),
                new DateTime(2016, 2, 1, 9, 29, 59).ToNextTradingMinute());
            Assert.AreEqual(
                new DateTime(2016, 2, 1, 9, 32, 0),
                new DateTime(2016, 2, 1, 9, 31, 0).ToNextTradingMinute());
            Assert.AreEqual(
                new DateTime(2016, 2, 1, 13, 1, 0),
                new DateTime(2016, 2, 1, 11, 30, 0).ToNextTradingMinute());
            Assert.AreEqual(
                new DateTime(2016, 2, 2, 9, 31, 0),
                new DateTime(2016, 2, 1, 15, 0, 0).ToNextTradingMinute());
        }
    }
}
