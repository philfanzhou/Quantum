using Ore.Infrastructure.MarketData;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 市盈率接口
    /// </summary>
    public interface IPriceEarnings : ITimeSeries
    {
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
