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
    public static class StockKLineExt
    {
        /// <summary>
        /// 获取KDJ指标
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<IKDJ> KDJ(this IEnumerable<IStockKLine> self)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取MACD指标
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<IMACD> MACD(this IEnumerable<IStockKLine> self)
        {
            throw new NotImplementedException();
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
                throw new ArgumentOutOfRangeException("cycle");
            }

            throw new NotImplementedException();
        }
    }
}
