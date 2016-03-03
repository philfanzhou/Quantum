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
            throw new NotImplementedException();
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
            else
            {
                // 判断两个集合持有数据的时间，如果时间全部match，就直接返回current，因为K线数据没有变化，不会影响到KDJ数据。

                // 在当前已有KDJ数据的后面，补充新的KDJ数据，因为Self里面可能加入了新的K线数据
                throw new NotImplementedException();
            }

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

        public static IEnumerable<IMACD> MACD(this IEnumerable<IStockKLine> self, IEnumerable<IMACD> current)
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

        public static IEnumerable<IMA> MA(this IEnumerable<IStockKLine> self, int cycle, IEnumerable<IMA> current)
        {
            if(null == current)
            {
                return self.MA(cycle);
            }

            throw new NotImplementedException();
        }
    }
}
