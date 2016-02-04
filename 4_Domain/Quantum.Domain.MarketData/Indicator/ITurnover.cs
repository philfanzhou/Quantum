using Ore.Infrastructure.MarketData;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 换手率接口定义
    /// </summary>
    public interface ITurnover : ITimeSeries
    {
        /// <summary>
        /// 换手率
        /// </summary>
        double TurnoverRatio { get; }
    }
}
