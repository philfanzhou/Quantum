using Ore.Infrastructure.MarketData;
using Quantum.Domain.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.MarketData
{
    public static class KLineConverter
    {
        /// <summary>
        /// 转换为其他类型的K线
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<IStockKLine> ConvertTo(
            this IEnumerable<IStockKLine> self, 
            KLineType type)
        {
            switch(type)
            {
                case KLineType.Min5:
                    return Min1ToMin5(self);
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 转换为1分钟K线
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<IStockKLine> ConvertToMin1(
            this IEnumerable<IStockRealTime> self)
        {
            Min1Collections<IStockRealTime> collections = new Min1Collections<IStockRealTime>();
            var packages = collections.SplitToPackages(self);

            throw new NotImplementedException();

        }

        private static IEnumerable<IStockKLine> Min1ToMin5(
            IEnumerable<IStockKLine> min1KLines)
        {
            // 构造每个数据包内包含5分钟数据的包裹集合
            Min5Collections<IStockKLine> collections = new Min5Collections<IStockKLine>();
            var packages = collections.SplitToPackages(min1KLines);
            return packages.Select(p => p.Combine());
        }
    }

    internal static class CombineStockKLine
    {
        public static IStockKLine Combine(this ITimeSeriesPackage<IStockKLine> self)
        {
            StockKLine outputData = new StockKLine
            {
                Time = self.Zone.EndTime,

                Open = self.Items.First().Open,
                Close = self.Items.Last().Close,

                Volume = self.Items.Sum(p => p.Volume),
                Amount = self.Items.Sum(p => p.Amount),

                High = self.Items.Max(p => p.High),
                Low = self.Items.Min(p => p.Low),
            };

            return outputData;
        }
    }
}
