using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 涨跌接口定义
    /// </summary>
    public interface IRiseAndFall
    {
        /// <summary>
        /// 当前数据的时间
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// 涨跌值
        /// </summary>
        double RiseValue { get; }

        /// <summary>
        /// 涨跌幅
        /// </summary>
        double RiseRatio { get; }

        /// <summary>
        /// 震幅
        /// </summary>
        double Amplitude { get; }
    }
}
