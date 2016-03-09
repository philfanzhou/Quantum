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
        double Average { get; }

        /// <summary>
        /// 均价离心率 = (Average - Price) / Price * 100
        /// </summary>
        double Eccentricity { get; }

        /// <summary>
        /// 当前时刻的实际价格
        /// </summary>
        double Price { get; }

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
