using System;

namespace Quantum.Domain.MarketData
{
    internal class MACDIndicator : IMACD
    {
        /// <summary>
        /// 快速移动平均值周期 默认12
        /// </summary>
        public int SlowEMACycle { get; private set; }

        /// <summary>
        /// 快速移动平均值  EMA12 = 前一日EMA12 X 11/13 + 今日收盘 X 2/13
        /// </summary>
        public double SlowEMA { get; private set; }

        /// <summary>
        /// 慢速移动平均值 默认26
        /// </summary>
        public int FastEMACycle { get; private set; }

        /// <summary>
        /// 慢速移动平均值  EMA26 = 前一日EMA26 X 25/27 + 今日收盘 X 2/27
        /// </summary>
        public double FastEMA { get; private set; }

        /// <summary>
        /// 差离值   DIF = EMA12 - EMA26
        /// </summary>
        public double DIF { get; private set; }

        /// <summary>
        /// 离差平均值   DEA = （前一日DEA X 8/10 + 今日DIF X 2/10）
        /// </summary>
        public double DEA { get; private set; }

        /// <summary>
        /// MACD 指数平滑移动平均线 （DIF-DEA）*2
        /// </summary>
        public double MACD { get; private set; }

        public DateTime Time { get; private set; }

        public MACDIndicator(double ydSlowEMA, double ydFastEMA, double tdClosedIndex, double ydDEA)
        {
            // 快速移动平均值周期 默认12
            SlowEMACycle = 12;

            // 快速移动平均值  EMA12 = 前一日EMA12 X 11/13 + 今日收盘 X 2/13
            SlowEMA = ydSlowEMA * 11 / 13 + tdClosedIndex * 2 / 13;

            // 慢速移动平均值 默认26
            FastEMACycle = 26;

            // 慢速移动平均值  EMA26 = 前一日EMA26 X 25/27 + 今日收盘 X 2/27
            FastEMA = ydFastEMA * 25 / 27 + tdClosedIndex * 2 / 27;

            // 差离值   DIF = EMA12 - EMA26
            DIF = SlowEMA - FastEMA;

            // 离差平均值   DEA = （前一日DEA X 8/10 + 今日DIF X 2/10）
            DEA = ydDEA * 8 / 10 + DIF * 2 / 10;

            // MACD 指数平滑移动平均线 （DIF-DEA）*2
            MACD = (DIF - DEA) * 2;

            Time = DateTime.Now;
        }
    }
}
