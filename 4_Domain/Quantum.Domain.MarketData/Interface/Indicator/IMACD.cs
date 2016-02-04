using Ore.Infrastructure.MarketData;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 指数平滑移动平均线
    /// </summary>
    public interface IMACD : ITimeSeries
    {
        /// <summary>
        /// 快速移动平均值周期 默认12
        /// </summary>
        int SlowEMACycle { get; }

        /// <summary>
        /// 快速移动平均值  EMA12 = 前一日EMA12 X 11/13 + 今日收盘 X 2/13
        /// </summary>
        double SlowEMA { get; }

        /// <summary>
        /// 慢速移动平均值 默认26
        /// </summary>
        int FastEMACycle { get; }

        /// <summary>
        /// 慢速移动平均值  EMA26 = 前一日EMA26 X 25/27 + 今日收盘 X 2/27
        /// </summary>
        double FastEMA { get; }

        /// <summary>
        /// 差离值   DIF = EMA12 - EMA26
        /// </summary>
        double DIF { get; }

        /// <summary>
        /// 离差平均值   DEA = （前一日DEA X 8/10 + 今日DIF X 2/10）
        /// </summary>
        double DEA { get; }

        /// <summary>
        /// MACD 指数平滑移动平均线 （DIF-DEA）*2
        /// </summary>
        double MACD { get; }
    }
}
