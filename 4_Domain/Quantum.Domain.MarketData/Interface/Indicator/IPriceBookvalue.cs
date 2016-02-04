using Ore.Infrastructure.MarketData;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 市净率接口
    /// </summary>
    public interface IPriceBookvalue : ITimeSeries
    {
        /// <summary>
        /// PB LYR
        /// </summary>
        double PriceBookValueLYR { get; }

        /// <summary>
        /// PB TTM
        /// </summary>
        double PriceBookValueTTM { get; }
    }
}
