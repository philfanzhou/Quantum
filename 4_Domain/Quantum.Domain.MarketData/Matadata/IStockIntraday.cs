using Ore.Infrastructure.MarketData;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 分时数据定义
    /// </summary>
    public interface IStockIntraday : ITimeSeries
    {
        /// <summary>
        /// 前一交易日收盘价
        /// </summary>
        double YesterdayClose { get; }
    }
}
