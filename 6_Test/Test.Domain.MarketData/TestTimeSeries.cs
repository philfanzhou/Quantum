using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Domain.MarketData;
using Ore.Infrastructure.MarketData;
using System.Linq;

namespace Test.Domain.MarketData
{
    [TestClass]
    public class TestTimeSeries
    {
        [TestMethod]
        public void TestMin5Packages()
        {
            // 构造从2月1日 -- 2月4日的数据
            // 每小时60条，每天4个小时，4天总计960条数据
            int totalCount = 60 * 4 * 4;
            var min1KLines = Simulation.CreateRandomKLines(
                KLineType.Min1, new DateTime(2016, 2, 1), totalCount).ToList();
            Assert.AreEqual(new DateTime(2016, 2, 4, 15, 0, 0), min1KLines.Last().Time);

            // 转换为min5之后，数量等于min1 / 5
            var min5KLine = min1KLines.ConvertTo(KLineType.Min5).ToList();
            Assert.AreEqual(totalCount / 5, min5KLine.Count);

            // 找出9：30 - 9：35的数据
            var kLines930_935 = min1KLines.Where(p => 
                p.Time.Date == new DateTime(2016, 2, 1).Date &&
                p.Time.TimeOfDay >= new TimeSpan(9, 30, 0) &&
                p.Time.TimeOfDay <= new TimeSpan(9, 35, 0)).ToList();
            var kLine935 = min5KLine.First();

            Assert.AreEqual(new DateTime(2016, 2, 1, 9, 35, 0), kLine935.Time);

            double volume = 0;
            double amount = 0;
            double open = 0;
            double close = 0;
            double high = 0;
            double low = double.MaxValue;

            #region
            for (int i = 0; i < kLines930_935.Count; i++)
            {
                var item = kLines930_935[i];

                volume += item.Volume;
                amount += item.Amount;

                if(high < item.High)
                {
                    high = item.High;
                }

                if(low > item.Low)
                {
                    low = item.Low;
                }

                if(i == 0)
                {
                    open = item.Open;
                }

                if(i == kLines930_935.Count -1)
                {
                    close = item.Close;
                }
            }
            #endregion

            Assert.IsTrue(volume - kLine935.Volume < 0.00000000000001);
            Assert.IsTrue(amount - kLine935.Amount < 0.00000000000001);
            Assert.IsTrue(open - kLine935.Open < 0.00000000000001);
            Assert.IsTrue(close - kLine935.Close < 0.00000000000001);
            Assert.IsTrue(high - kLine935.High < 0.00000000000001);
            Assert.IsTrue(low - kLine935.Low < 0.00000000000001);
        }
    }
}
