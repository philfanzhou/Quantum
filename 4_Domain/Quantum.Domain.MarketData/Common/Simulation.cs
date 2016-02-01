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
        /// <summary>
        /// 构造一条模拟K线的数据
        /// </summary>
        /// <param name="type">只支持Day, Min1, Min5</param>
        /// <param name="startTime"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<IStockKLine> GetKLine(KLineType type, DateTime startTime, int count)
        {
            if(type != KLineType.Day
                || type != KLineType.Min1
                || type != KLineType.Min5)
            {
                throw new NotSupportedException(string.Format("Only support {0}, {1}, {2}", KLineType.Day, KLineType.Min1, KLineType.Min5));
            }

            if(count < 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            List<StockKLine> result = new List<StockKLine>();
            DateTime tradingTime = GetStartTime(type, startTime);
            StockKLine previousData = null;

            // 添加第一条数据
            var firstOne = new StockKLine
            {
                Open = 
            };

            for (int i = 1; i < count; i++)
            {

            }

            throw new NotImplementedException();
        }

        private static DateTime GetStartTime(KLineType type, DateTime startTime)
        {
            // 如果起始时间是处于未开盘时间，将起始时间后移到最近的一个交易日
            DateTime tradingTime = startTime.GetNearestTradingDate();
            
            if(type == KLineType.Min1)
            {
                return new DateTime(tradingTime.Year, tradingTime.Month, tradingTime.Day, 9, 31, 0);
            }
            else if(type == KLineType.Min5)
            {
                return new DateTime(tradingTime.Year, tradingTime.Month, tradingTime.Day, 9, 35, 0);
            }
            else if(type == KLineType.Day)
            {
                return tradingTime.Date;
            }
            else
            {
                throw new NotSupportedException(string.Format("Only support {0}, {1}, {2}", KLineType.Day, KLineType.Min1, KLineType.Min5));
            }
        }
    }
}
