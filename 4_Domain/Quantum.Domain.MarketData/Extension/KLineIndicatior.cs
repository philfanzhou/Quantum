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
            int N = 9;
            for (int i = 0; i < lstStockKLine.Count; i++)
            {
                var lstSub = lstStockKLine.Skip((i - N) < 0 ? 0 : (i - N + 1)).Take((i - N) < 0 ? i + 1 : N).ToList();
                var minPrice = (lstSub != null && lstSub.Count != 0) ? lstSub.Select(k => k.Low).Min() : lstStockKLine[i].Low;
                var maxPrice = (lstSub != null && lstSub.Count != 0) ? lstSub.Select(k => k.High).Max() : lstStockKLine[i].High;

                if (i == 0)
                {
                    lstKDJ.Add(new KDJIndicator(lstStockKLine[i].Time, lstStockKLine[i].Close, minPrice, maxPrice));
                }
                else
                {
                    lstKDJ.Add(new KDJIndicator(lstStockKLine[i].Time, lstStockKLine[i].Close, minPrice, maxPrice, lstKDJ[i - 1]));
                }
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
            int N = 9;
            for (int i = 0; i < lstStockKLine.Count; i++)
            {
                var lstContians = from kdj in lstKDJ
                                  where kdj.Time.CompareTo(lstStockKLine[i].Time) == 0
                                  select kdj;
                if (lstContians == null || lstContians.Count() < 1)
                {
                    var lstSub = lstStockKLine.Skip((i - N) < 0 ? 0 : (i - N + 1)).Take((i - N) < 0 ? i + 1 : N).ToList();
                    var minPrice = (lstSub != null && lstSub.Count != 0) ? lstSub.Select(k => k.Low).Min() : lstStockKLine[i].Low;
                    var maxPrice = (lstSub != null && lstSub.Count != 0) ? lstSub.Select(k => k.High).Max() : lstStockKLine[i].High;

                    if (i == 0)
                    {
                        lstKDJ.Insert(i, new KDJIndicator(lstStockKLine[i].Time, lstStockKLine[i].Close, minPrice, maxPrice));
                    }
                    else
                    {
                        lstKDJ.Insert(i, new KDJIndicator(lstStockKLine[i].Time, lstStockKLine[i].Close, minPrice, maxPrice, lstKDJ[i - 1]));
                    }
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
                if (i == 0)
                {
                    lstMACD.Add(new MACDIndicator(lstStockKLine[i].Time, lstStockKLine[i].Close));
                }
                else
                {
                    lstMACD.Add(new MACDIndicator(lstStockKLine[i].Time, lstStockKLine[i].Close, lstMACD[i - 1]));
                }
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
                var lstContians = from macd in lstMACD
                                  where macd.Time.CompareTo(lstStockKLine[i].Time) == 0
                                  select macd;
                if (lstContians == null || lstContians.Count() < 1)
                {
                    if (i == 0)
                    {
                        lstMACD.Insert(i, new MACDIndicator(lstStockKLine[i].Time, lstStockKLine[i].Close));
                    }
                    else
                    {
                        lstMACD.Insert(i, new MACDIndicator(lstStockKLine[i].Time, lstStockKLine[i].Close, lstMACD[i - 1]));
                    }
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
            {
                throw new ArgumentOutOfRangeException("Can not calculate MA due to Cycle <= 1");
            }

            if (self == null)
                throw new ArgumentOutOfRangeException("Can not calculate MA due to no kline datas");

            var lstStockKLine = self.ToList();
            if (lstStockKLine.Count < cycle)
                throw new ArgumentOutOfRangeException("Can not calculate MA due to the kline data number < Cycle");

            List<IMA> lstMA = new List<IMA>();
            for (int i = cycle - 1; i < lstStockKLine.Count; i++)
            {
                var kline = lstStockKLine[i];
                var lstSub = lstStockKLine.Skip(i - cycle + 1).Take(cycle).ToList();
                double valMA = lstSub.Average(x => x.Close);
                lstMA.Add(new MAIndicator(kline.Time, cycle, valMA));
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

            var lstStockKLine = self.ToList();
            if (lstStockKLine.Count < cycle)
                throw new ArgumentOutOfRangeException("Can not calculate MA due to the kline data number < Cycle");

            var lstMA = current.ToList();
            for (int i = cycle - 1; i < lstStockKLine.Count; i++)
            {
                var lstContians = from ma in lstMA
                                  where ma.Time.CompareTo(lstStockKLine[i].Time) == 0
                                  select ma;
                if (lstContians == null || lstContians.Count() < 1)
                {
                    var skipCount = i - cycle + 1;
                    var lstSub = lstStockKLine.Skip(skipCount).Take(cycle).ToList();
                    double valMA = lstSub.Average(x => x.Close);
                    lstMA.Insert(skipCount, new MAIndicator(lstStockKLine[i].Time, cycle, valMA));
                }
            }
            return lstMA;
        }
    }
}
