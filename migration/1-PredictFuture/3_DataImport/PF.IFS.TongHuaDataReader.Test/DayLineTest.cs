using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.IFS.TongHua.DataReader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PF.IFS.TongHuaDataReader.Test
{
    [TestClass]
    public class DayLineTest
    {
        [TestMethod]
        public void GetDayLineTest()
        {
            DataReader dataReader = new DataReader();
            dataReader.AnalyseDayLineFiles(new[] { TestData.ShanghaiDay, TestData.ShenzhenDay });
            IKlineData klineData = dataReader.GetDaylineData("432534", DateTime.MinValue);
            Assert.IsNull(klineData);
        }

        [TestMethod]
        public void GetKlineDataTest1()
        {
            DataReader dataReader = new DataReader();
            dataReader.AnalyseDayLineFiles(new[] { TestData.ShanghaiDay, TestData.ShenzhenDay });
            IKlineData klineData = dataReader.GetDaylineData("600036", DateTime.MinValue);

            Assert.AreEqual("600036", klineData.Symbol);
            List<IKlineItem> kLineItems = klineData.Items.ToList();

            IKlineItem data20130422 = kLineItems[0];
            Assert.AreEqual(742446960.0, data20130422.Amount);
            Assert.AreEqual(12.59, data20130422.Close);
            Assert.AreEqual(new DateTime(2013, 4, 22), data20130422.Date);
            Assert.AreEqual(12.64, data20130422.High);
            Assert.AreEqual(12.52, data20130422.Low);
            Assert.AreEqual(12.53, data20130422.Open);
            Assert.AreEqual(0.0, data20130422.Turnover);
            Assert.AreEqual(59032937.0, data20130422.Volume);

            IKlineItem data20130502 = kLineItems[5];
            Assert.AreEqual(565173920.0, data20130502.Amount);
            Assert.AreEqual(12.1, data20130502.Close);
            Assert.AreEqual(new DateTime(2013, 5, 2), data20130502.Date);
            Assert.AreEqual(12.12, data20130502.High);
            Assert.AreEqual(11.95, data20130502.Low);
            Assert.AreEqual(12.1, data20130502.Open);
            Assert.AreEqual(0.0, data20130502.Turnover);
            Assert.AreEqual(46973420.0, data20130502.Volume);

            IKlineItem data20130515 = kLineItems[14];
            Assert.AreEqual(1476330800.0, data20130515.Amount);
            Assert.AreEqual(13.59, data20130515.Close);
            Assert.AreEqual(new DateTime(2013, 5, 15), data20130515.Date);
            Assert.AreEqual(13.65, data20130515.High);
            Assert.AreEqual(13.25, data20130515.Low);
            Assert.AreEqual(13.32, data20130515.Open);
            Assert.AreEqual(0.0, data20130515.Turnover);
            Assert.AreEqual(109663142.0, data20130515.Volume);
        }

        [TestMethod]
        public void GetKlineDataTest2()
        {
            DataReader dataReader = new DataReader();
            dataReader.AnalyseDayLineFiles(new[] { TestData.ShanghaiDay, TestData.ShenzhenDay });
            IKlineData klineData = dataReader.GetDaylineData("1A0001", DateTime.MinValue);

            Assert.AreEqual("1A0001", klineData.Symbol);
            List<IKlineItem> kLineitems = klineData.Items.ToList();

            IKlineItem data20121225 = kLineitems[0];
            Assert.AreEqual(114873959000.0, data20121225.Amount);
            Assert.AreEqual(2213.61, data20121225.Close);
            Assert.AreEqual(new DateTime(2012, 12, 25), data20121225.Date);
            Assert.AreEqual(2220.263, data20121225.High);
            Assert.AreEqual(2146.862, data20121225.Low);
            Assert.AreEqual(2154.32, data20121225.Open);
            Assert.AreEqual(0.0, data20121225.Turnover);
            Assert.AreEqual(14332770000.0, data20121225.Volume);

            IKlineItem data20130104 = kLineitems[5];
            Assert.AreEqual(114627763000.0, data20130104.Amount);
            Assert.AreEqual(2276.992, data20130104.Close);
            Assert.AreEqual(new DateTime(2013, 1, 4), data20130104.Date);
            Assert.AreEqual(2296.112, data20130104.High);
            Assert.AreEqual(2256.56, data20130104.Low);
            Assert.AreEqual(2289.51, data20130104.Open);
            Assert.AreEqual(0.0, data20130104.Turnover);
            Assert.AreEqual(13946993000.0, data20130104.Volume);

            IKlineItem data20130117 = kLineitems[14];
            Assert.AreEqual(103214052000.0, data20130117.Amount);
            Assert.AreEqual(2284.909, data20130117.Close);
            Assert.AreEqual(new DateTime(2013, 1, 17), data20130117.Date);
            Assert.AreEqual(2305.601, data20130117.High);
            Assert.AreEqual(2275.877, data20130117.Low);
            Assert.AreEqual(2305.139, data20130117.Open);
            Assert.AreEqual(0.0, data20130117.Turnover);
            Assert.AreEqual(11910183500.0, data20130117.Volume);

            klineData = dataReader.GetDaylineData("002176", DateTime.MinValue);
            Assert.AreEqual("002176", klineData.Symbol);
            kLineitems = klineData.Items.ToList();

            IKlineItem data20130423 = kLineitems[1];
            Assert.AreEqual(58835558, data20130423.Amount);
            Assert.AreEqual(8.63, data20130423.Close);
            Assert.AreEqual(new DateTime(2013, 4, 23), data20130423.Date);
            Assert.AreEqual(9.13, data20130423.High);
            Assert.AreEqual(8.61, data20130423.Low);
            Assert.AreEqual(9.12, data20130423.Open);
            Assert.AreEqual(0.0, data20130423.Turnover);
            Assert.AreEqual(6671219.0, data20130423.Volume);
        }
    }
}
