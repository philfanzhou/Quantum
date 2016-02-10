using Ore.Infrastructure.MarketData;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 均价线
    /// </summary>
    public interface IAveragePrice : ITimeSeries
    {
        /// <summary>
        /// 均价 = 当前时刻总成交额 / 当前时刻总成交量
        /// </summary>
        double AveragePrice { get; }

        /// <summary>
        /// 成交量
        /// </summary>
        double Volume { get; }

        /// <summary>
        /// 成交额
        /// </summary>
        double Amount { get; }
    }
}
