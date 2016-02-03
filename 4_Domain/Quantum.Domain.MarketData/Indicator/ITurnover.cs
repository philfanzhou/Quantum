using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 换手率接口定义
    /// </summary>
    public interface ITurnover
    {
        /// <summary>
        /// 当前数据的时间
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// 换手率
        /// </summary>
        double TurnoverRatio { get; }
    }
}
