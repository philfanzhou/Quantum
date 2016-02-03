using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 多空指标
    /// </summary>
    public interface IBuyAgainstSell
    {
        /// <summary>
        /// 当前数据的时间
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// 委卖
        /// </summary>
        double SellVolume { get; }

        /// <summary>
        /// 委买
        /// </summary>
        double BuyVolume { get; }

        /// <summary>
        /// 委差
        /// </summary>
        double CommissionDiff { get; }

        /// <summary>
        /// 委比
        /// </summary>
        double CommissionRatio { get; }

        /// <summary>
        /// 多空比
        /// </summary>
        double MultiEmptyRatio { get; }
    }
}
