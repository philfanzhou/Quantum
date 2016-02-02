using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 为测试代码构造数据提供支持的类
    /// </summary>
    public static class Simulation
    {
        private static Random _random = new Random();

        /// <summary>
        /// 构造一条模拟K线的数据
        /// </summary>
        /// <param name="type">只支持Day, Min1, Min5</param>
        /// <param name="startTime"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<IStockKLine> GetKLine(KLineType type, DateTime startTime, int count)
        {
            if(type != KLineType.Day && type != KLineType.Min1)
            {
                throw new NotSupportedException(string.Format("Only support {0}, {1}", KLineType.Day, KLineType.Min1));
            }
            if(count < 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            List<StockKLine> result = new List<StockKLine>();
            
            for (int i = 0; i < count; i++)
            {
                // 取得昨收
                double preClose;
                double preVolume;
                DateTime tradingTime;

                if (i == 0)
                {
                    preClose = _random.Next(0.95, 350.99);
                    preVolume = _random.Next(100000, 10000000);

                    if (type == KLineType.Min1)
                    {
                        tradingTime = startTime.IsTradingTime() ? startTime : startTime.ToNextTradingMinute();
                    }
                    else // 只有分钟线和日线的情况
                    {
                        tradingTime = startTime.IsTradingDate() ? startTime.Date : startTime.ToNextTradingDate();
                    }
                }
                else
                {
                    preClose = result[i - 1].Close;
                    preVolume = result[i - 1].Volume;

                    if (type == KLineType.Min1)
                    {
                        tradingTime = result[i - 1].Time.ToNextTradingMinute();
                    }
                    else
                    {
                        tradingTime = result[i - 1].Time.ToNextTradingDate();
                    }
                }

                var kLine = CreateRandomKLine(tradingTime, preClose, preVolume);
                result.Add(kLine);
            }

            return result;
        }

        private static StockKLine CreateRandomKLine(DateTime time, double preClose, double preVolume, int digits = 2)
        {
            // 计算涨停和跌停价格
            double upLimit = PriceLimit.StockUpLimit(preClose);
            double downLimit = PriceLimit.StockDownLimit(preClose);

            var kLine = new StockKLine();
            kLine.Time = time;

            // 随机涨跌平
            int ret = _random.Next(1, 3);
            if(ret == 1)// 跌
            {
                kLine.Open = _random.Next(downLimit, preClose, digits);
                kLine.Close = _random.Next(downLimit, upLimit, digits);
                kLine.Volume = (int)_random.Next(preVolume * 0.85, preVolume * 1.15);
            }
            else if(ret == 2)// 平
            {
                kLine.Open = _random.Next(downLimit, upLimit, digits);
                kLine.Close = _random.Next(downLimit, upLimit, digits);
                kLine.Volume = (int)_random.Next(preVolume * 0.9, preVolume * 1.2);
            }
            else if(ret == 3)// 涨
            {
                kLine.Open = _random.Next(preClose, upLimit, digits);
                kLine.Close = _random.Next(downLimit, upLimit, digits);
                kLine.Volume = (int)_random.Next(preVolume * 0.95, preVolume * 1.25);
            }

         
            kLine.High = _random.Next(kLine.Open, upLimit, digits);
            kLine.Low = _random.Next(downLimit, kLine.Open, digits);

            // 随机一个均价，求出成交额
            double average = _random.Next(kLine.Low, kLine.High, digits);
            kLine.Amount = average * kLine.Volume;

            return kLine;
        }
    }

    internal static class RandomExt
    {
        public static double Next(this Random self, double min, double max, int digits = 2)
        {
            int intMin = (int)Math.Round(min, 0, MidpointRounding.AwayFromZero);
            int intMax = (int)Math.Round(max, 0, MidpointRounding.ToEven);

            int intValue = self.Next(intMin, intMax);
            double doubleValue = Math.Round(self.NextDouble(), digits, MidpointRounding.AwayFromZero);

            double value = intValue + doubleValue;
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }
    }
}
