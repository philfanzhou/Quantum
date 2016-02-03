using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 市净率接口
    /// </summary>
    public interface IPriceBookvalue
    {
        /// <summary>
        /// 当前数据的时间
        /// </summary>
        DateTime Time { get; }

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
