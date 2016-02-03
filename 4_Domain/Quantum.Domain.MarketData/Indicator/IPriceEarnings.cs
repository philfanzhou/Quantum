using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 市盈率接口
    /// </summary>
    public interface IPriceEarnings
    {
        /// <summary>
        /// 当前数据的时间
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// PE : 股价/最近年度报告
        /// </summary>
        double PriceEarningsLYR { get; }

        /// <summary>
        /// PE : 股价/最近12个月(四个季度)
        /// </summary>
        double PriceEarningsTTM { get; }
    }
}
