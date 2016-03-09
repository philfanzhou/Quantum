using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public static IEnumerable<IStockKLine> CreateRandomKLines(KLineType type, DateTime startTime, int count)
        {
            if(type != KLineType.Day && type != KLineType.Min1)
            {
                throw new NotSupportedException(string.Format("Only support {0}, {1}", KLineType.Day, KLineType.Min1));
            }
            if(count < 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            List<IStockKLine> result = new List<IStockKLine>();

            result.Add(CreateFirstData(type, startTime));

            for (int i = 1; i < count; i++)
            {
                AppendNewData(type, ref result);
            }

            return result;
        }

        public static IEnumerable<IStockKLine> CreateRandomKLines(KLineType type, DateTime startTime, DateTime endTime)
        {
            if (type != KLineType.Day && type != KLineType.Min1)
            {
                throw new NotSupportedException(string.Format("Only support {0}, {1}", KLineType.Day, KLineType.Min1));
            }
            if (startTime > endTime)
            {
                throw new ArgumentOutOfRangeException("endTime");
            }

            List<IStockKLine> result = new List<IStockKLine>();

            result.Add(CreateFirstData(type, startTime));

            do
            {
                AppendNewData(type, ref result);
            } while (result.Last().Time <= endTime);

            result.RemoveAt(result.Count - 1);

            return result;
        }

        #region Private Method
        private static IStockKLine CreateFirstData(KLineType type, DateTime startTime)
        {
            double preClose = GetRandomPrice();
            double preVolume = _random.Next(100000, 10000000);
            DateTime tradingTime;
            if (type == KLineType.Min1)
            {
                tradingTime = startTime.IsTradingTime() ? startTime.Date.AddHours(startTime.Hour).AddMinutes(startTime.Minute) : startTime.ToNextTradingMinute();
            }
            else
            {
                tradingTime = startTime.IsTradingDate() ? startTime.Date : startTime.ToNextTradingDate();
            }

            var kLine = CreateRandomItem(tradingTime, preClose, preVolume);
            return kLine;
        }

        private static void AppendNewData(KLineType type, ref List<IStockKLine> existData)
        {
            var preData = existData.Last();

            DateTime tradingTime;
            if (type == KLineType.Min1)
            {
                tradingTime = preData.Time.ToNextTradingMinute();
            }
            else
            {
                tradingTime = preData.Time.ToNextTradingDate();
            }
            
            var newData = CreateRandomItem(tradingTime, preData.Close, preData.Volume);

            existData.Add(newData);
        }

        private static StockKLine CreateRandomItem(DateTime tradingTime, double preClose, double preVolume, int digits = 2)
        {
            // 计算涨停和跌停价格
            double upLimit = PriceLimit.UpLimit(SecurityType.Sotck, preClose);
            double downLimit = PriceLimit.DownLimit(SecurityType.Sotck, preClose);

            var kLine = new StockKLine();
            kLine.Time = tradingTime;
            kLine.Volume = (int)_random.Next(preVolume * 0.5, preVolume * 1.5);

            // 随机涨跌平
            UpOrDown upOrDown = GetDirection(preClose);
            if (upOrDown == UpOrDown.Down)// 跌
            {
                // 3%的机会一字跌
                if (_random.Next(0, 100) > 96)
                {
                    kLine.Open = downLimit;
                    kLine.Close = downLimit;
                }
                else if (_random.Next(0, 100) > 90) //  10%机会跌停板
                {
                    kLine.Close = downLimit;
                    kLine.Open = _random.Next(downLimit, preClose, digits);
                }
                else
                {
                    kLine.Open = _random.Next(downLimit, preClose, digits);
                    kLine.Close = _random.Next(downLimit, preClose, digits);
                }
            }
            else if (upOrDown == UpOrDown.Up)// 涨
            {
                // 3%的机会一字涨
                if (_random.Next(0, 100) > 96)
                {
                    kLine.Open = upLimit;
                    kLine.Close = upLimit;
                }
                else if (_random.Next(0, 100) > 90) //  10%机会涨停板
                {
                    kLine.Close = upLimit;
                    kLine.Open = _random.Next(preClose, upLimit, digits);
                }
                else
                {
                    kLine.Open = _random.Next(preClose, upLimit, digits);
                    kLine.Close = _random.Next(preClose, upLimit, digits);
                }
            }
            else
            {
                kLine.Open = _random.Next(downLimit, upLimit, digits);
                kLine.Close = _random.Next(downLimit, upLimit, digits);
            }


            kLine.High = _random.Next(kLine.Open, upLimit, digits);
            kLine.Low = _random.Next(downLimit, kLine.Open, digits);

            // 随机一个均价，求出成交额
            double average = _random.Next(kLine.Low, kLine.High, digits);
            kLine.Amount = average * kLine.Volume;

            return kLine;
        }

        private static double GetRandomPrice()
        {
            int ret = _random.Next(0, 10);

            if(ret < 2)
            {
                return _random.Next(100.00, 350.00);
            }
            else if(ret < 7)
            {
                return _random.Next(15.00, 100.00);
            }
            else
            {
                return _random.Next(1.00, 15.00);
            }
        }

        /// <summary>
        /// 根据前收盘价格，随机取得涨跌方向。
        /// </summary>
        /// <param name="preClose"></param>
        /// <returns></returns>
        private static UpOrDown GetDirection(double preClose)
        {
            if (preClose < 5)
            {
                return _random.Next(0, 10) < 9 ? UpOrDown.Up : UpOrDown.None;
            }
            else if (preClose < 10)
            {
                return _random.Next(0, 10) < 8 ? UpOrDown.Up : UpOrDown.None;
            }
            else if (preClose < 30)
            {
                return _random.Next(0, 10) < 7 ? UpOrDown.Up : UpOrDown.None;
            }
            else if(preClose > 100)
            {
                return _random.Next(0, 10) < 7 ? UpOrDown.Down : UpOrDown.None;
            }
            else if (preClose > 150)
            {
                return _random.Next(0, 10) < 8 ? UpOrDown.Down : UpOrDown.None;
            }
            else if (preClose > 200)
            {
                return _random.Next(0, 10) < 9 ? UpOrDown.Down : UpOrDown.None;
            }
            else
            {
                return UpOrDown.None;
            }
        }

        /// <summary>
        /// 涨跌枚举
        /// </summary>
        private enum UpOrDown
        {
            Up,
            None,
            Down
        }
        #endregion
    }

    internal static class RandomExt
    {
        public static double Next(this Random self, double min, double max, int digits = 2)
        {
            int intMin = min < int.MinValue ? int.MinValue : Convert.ToInt32(min);
            int intMax = max > int.MaxValue ? int.MaxValue : Convert.ToInt32(max);

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
