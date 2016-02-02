using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.MarketData
{
    public static class KLineTypeExt
    {
        /// <summary>
        /// 获取K线的类型
        /// </summary>
        /// <param name="self"></param>
        /// <param name="checkDataCount"></param>
        /// <returns></returns>
        public static KLineType GetType(this IEnumerable<IStockKLine> self, int checkDataCount)
        {
            KLineType? type = null;
            if(self.TryGetType(out type, checkDataCount) 
                && type.HasValue)
            {
                return type.Value;
            }
            else
            {
                throw new InvalidOperationException("Can not get KLine type");
            }
        }

        public static bool TryGetType(
            this IEnumerable<IStockKLine> self,
            out KLineType? type,
            int checkDataCount)
        {
            type = null;

            if (self == null)
            {
                return false;
            }

            List<IStockKLine> kLineList = self.ToList();
            if (kLineList.Count < 2)
            {
                return false;
            }

            // 取得最前面两条数据之间的时间差
            TimeSpan span = kLineList[1].Time - kLineList[0].Time;

            /*
            此方法还未完成：
            1：遇到分钟数据跨天的情况，如何判断？
            2：日线或者周线，遇到放假的时候，如何判断？
            */

            // 确保前N条数据的时间差相同
            int count = kLineList.Count > checkDataCount ? checkDataCount : kLineList.Count;
            for (int i = 2; i < count; i++)
            {
                DateTime time1 = kLineList[i - 1].Time;
                DateTime time2 = kLineList[i].Time;

                if (span.Equals(time2 - time1) == false)
                {
                    return false;
                }
            }

            // 根据时间差判断出K线类型
            if (span.Equals(new TimeSpan(0, 1, 0)))
            {
                type = KLineType.Min1;
            }
            else if (span.Equals(new TimeSpan(0, 5, 0)))
            {
                type = KLineType.Min5;
            }
            else if (span.Equals(new TimeSpan(0, 15, 0)))
            {
                type = KLineType.Min15;
            }
            else if (span.Equals(new TimeSpan(0, 30, 0)))
            {
                type = KLineType.Min30;
            }
            else if (span.Equals(new TimeSpan(0, 60, 0)))
            {
                type = KLineType.Min60;
            }
            else if (span.Equals(new TimeSpan(1, 0, 0, 0)))
            {
                type = KLineType.Day;
            }
            else if (span.Equals(new TimeSpan(7, 0, 0, 0)))
            {
                type = KLineType.Week;
            }
            else if (kLineList[1].Time.Year - kLineList[0].Time.Year == 1)
            {
                type = KLineType.Year;
            }
            else if (kLineList[1].Time.Month - kLineList[0].Time.Year == 1)
            {
                type = KLineType.Year;
            }

            throw new NotImplementedException();
        }
    }
}
