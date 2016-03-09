using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// K线上各种指标数据的扩展
    /// </summary>
    public static class KLineIndicatior
    {
        private static bool IsContians(this IEnumerable<ITimeSeries> lstIndex, DateTime dt)
        {
            if (lstIndex == null)
                return false;

            var lstContians = from index in lstIndex
                              where index.Time.CompareTo(dt) == 0
                              select index;

            return (lstContians != null && lstContians.Count() > 0);
        }

        private static IEnumerable<IStockKLine> GetCycleKLine(this IEnumerable<IStockKLine> self, int index, int cycle)
        {
            if (self == null || self.Count() < 1)
                return null;

            return self.Skip((index - cycle) < 0 ? 0 : (index - cycle + 1)).Take((index - cycle) < 0 ? index + 1 : cycle);
        }

        private static Double MinPrice(this IEnumerable<IStockKLine> self)
        {
            if (self == null || self.Count() < 1)
                return 0;
            return self.Select(k => k.Low).Min();
        }

        private static Double MaxPrice(this IEnumerable<IStockKLine> self)
        {
            if (self == null || self.Count() < 1)
                return 0;
            return self.Select(k => k.High).Max();
        }

        private static IKDJ KDJ(this IEnumerable<IStockKLine> self, int index, IKDJ preKDJ = null, int N = 9)
        {
            if (self == null || self.Count() <= index)
                return null;

            var lstStockKLine = self.ToList();
            var lstSub = self.GetCycleKLine(index, N).ToList();
            var minPrice = lstSub.MinPrice();
            var maxPrice = lstSub.MaxPrice();
            if (index == 0)
            {
                return new KDJIndicator(lstStockKLine[index].Time, lstStockKLine[index].Close, minPrice, maxPrice);
            }
            else
            {
                return new KDJIndicator(lstStockKLine[index].Time, lstStockKLine[index].Close, minPrice, maxPrice, preKDJ);
            }
        }

        private static IMACD MACD(this IEnumerable<IStockKLine> self, int index, IMACD preMACD = null)
        {
            if (self == null || self.Count() <= index)
                return null;

            var lstStockKLine = self.ToList();
            if (index == 0)
            {
                return new MACDIndicator(lstStockKLine[index].Time, lstStockKLine[index].Close);
            }
            else
            {
                return new MACDIndicator(lstStockKLine[index].Time, lstStockKLine[index].Close, preMACD);
            }
        }

        private static IMA MA(this IEnumerable<IStockKLine> self, int index, int cycle)
        {
            if (self == null || self.Count() <= index)
                return null;

            var lstStockKLine = self.ToList();
            var lstSub = lstStockKLine.GetCycleKLine(index, cycle).ToList();
            double valMA = lstSub.Average(x => x.Close);
            return new MAIndicator(lstStockKLine[index].Time, cycle, valMA);
        }

        /// <summary>
        /// 获取KDJ指标
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<IKDJ> KDJ(this IEnumerable<IStockKLine> self)
        {
            if (self == null)
                return null;

            var lstStockKLine = self.ToList();
            List<IKDJ> lstKDJ = new List<IKDJ>();
            for (int i = 0; i < lstStockKLine.Count; i++)
            {
                lstKDJ.Add(lstStockKLine.KDJ(i, (i < 1 ? null : lstKDJ[i - 1])));
            }

            return lstKDJ;
        }

        /// <summary>
        /// 在指定的KDJ数据后面，补充缺少的数据，避免每次调用KDJ数据都全部重新计算
        /// </summary>
        /// <param name="self"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static IEnumerable<IKDJ> KDJ(this IEnumerable<IStockKLine> self, IEnumerable<IKDJ> current)
        {
            if(null == current)
            {
                return self.KDJ();
            }

            if (self == null)
                return current;
            // 判断两个集合持有数据的时间，如果时间全部match，就直接返回current，因为K线数据没有变化，不会影响到KDJ数据。
            // 在当前已有KDJ数据的后面，补充新的KDJ数据，因为Self里面可能加入了新的K线数据  

            var lstStockKLine = self.ToList();
            List<IKDJ> lstKDJ = current.ToList();
            for (int i = 0; i < lstStockKLine.Count; i++)
            {
                if(!lstKDJ.IsContians(lstStockKLine[i].Time))
                {
                    lstKDJ.Insert(i, lstStockKLine.KDJ(i, (i < 1 ? null : lstKDJ[i - 1])));
                }
            }

            return lstKDJ;
        }

        /// <summary>
        /// 获取MACD指标
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<IMACD> MACD(this IEnumerable<IStockKLine> self)
        {
            if (self == null)
                return null;

            var lstStockKLine = self.ToList();   
            List<IMACD> lstMACD = new List<IMACD>();
            for (int i = 0; i < lstStockKLine.Count; i++)
            {
                lstMACD.Add(lstStockKLine.MACD(i, (i < 1 ? null : lstMACD[i - 1])));
            }

            return lstMACD;
        }

        public static IEnumerable<IMACD> MACD(this IEnumerable<IStockKLine> self, IEnumerable<IMACD> current)
        {
            if (null == current)
            {
                return self.MACD();
            }

            if (self == null)
                return null;

            var lstStockKLine = self.ToList();
            var lstMACD = current.ToList();
            for (int i = 0; i < lstStockKLine.Count; i++)
            {
                if (!lstMACD.IsContians(lstStockKLine[i].Time))
                {
                    lstMACD.Insert(i, lstStockKLine.MACD(i, (i < 1 ? null : lstMACD[i - 1])));
                }
            }

            return lstMACD;
        }

        /// <summary>
        /// 获取MA指标
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cycle">均线周期，如：5日均线，10日均线， 20日均线， 60日均线</param>
        /// <returns></returns>
        public static IEnumerable<IMA> MA(this IEnumerable<IStockKLine> self, int cycle)
        {
            if(cycle <= 1)
                throw new ArgumentOutOfRangeException("Can not calculate MA due to Cycle <= 1");

            if (self == null)
                throw new ArgumentOutOfRangeException("Can not calculate MA due to no kline datas");

            if (self.Count() < cycle)
                throw new ArgumentOutOfRangeException("Can not calculate MA due to the kline data number < Cycle");

            var lstStockKLine = self.ToList();
            List<IMA> lstMA = new List<IMA>();
            int start = cycle - 1;//可以从0开始，但是前cycle个数据不准确
            for (int i = start; i < lstStockKLine.Count; i++)
            {
                lstMA.Add(lstStockKLine.MA(i, cycle));
            }
            return lstMA;
        }

        public static IEnumerable<IMA> MA(this IEnumerable<IStockKLine> self, int cycle, IEnumerable<IMA> current)
        {
            if (null == current)
            {
                return self.MA(cycle);
            }

            if (self == null)
                throw new ArgumentOutOfRangeException("Can not calculate MA due to no kline datas");

            if (self.Count() < cycle)
                throw new ArgumentOutOfRangeException("Can not calculate MA due to the kline data number < Cycle");

            var lstStockKLine = self.ToList();
            var lstMA = current.ToList();
            int start = cycle - 1;//可以从0开始，但是前cycle个数据不准确
            for (int i = start; i < lstStockKLine.Count; i++)
            {
                if (!lstMA.IsContians(lstStockKLine[i].Time))
                {
                    lstMA.Insert(i - cycle + 1, lstStockKLine.MA(i, cycle));
                }
            }
            return lstMA;
        }
    }
}
