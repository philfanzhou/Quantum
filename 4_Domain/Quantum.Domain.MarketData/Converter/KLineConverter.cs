using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static IEnumerable<IStockKLine> ToType(
            this IEnumerable<IStockKLine> self, 
            KLineType type)
        {
            throw new System.NotImplementedException();
        }

        //private static IEnumerable<IStockKLine> Min1ToMin5(IEnumerable<IStockKLine> min1KLines)
        //{
        //    var packages = new List<ITimeSeriesPackage<IStockKLine>>();
        //    ITimeSeriesPackage<IStockKLine> currentPackage;

        //    foreach (var item in min1KLines)
        //    {
        //        if(currentPackage == null)
        //        {
        //            DateTime startTime = item.Time.m
        //        }
        //    }
        //}
    }
}
