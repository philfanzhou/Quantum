using System;

namespace Quantum.Domain.MarketData
{
    internal class MACDIndicator : IMACD
    {
        private const int SHORT = 12;
        private const int LONG = 26;
        private const int M = 9;

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

        public MACDIndicator(DateTime time, double closePrice)
        {
            // 快速移动平均值周期 默认12
            SlowEMACycle = SHORT;

            // 快速移动平均值  EMA12 = 前一日EMA12 X 11/13 + 今日收盘 X 2/13
            SlowEMA = closePrice;

            // 慢速移动平均值 默认26
            FastEMACycle = LONG;

            // 慢速移动平均值  EMA26 = 前一日EMA26 X 25/27 + 今日收盘 X 2/27
            FastEMA = closePrice;

            // 差离值   DIF = EMA12 - EMA26
            DIF = SlowEMA - FastEMA;

            // 离差平均值   DEA = （前一日DEA X 8/10 + 今日DIF X 2/10）
            DEA = DIF;

            // MACD 指数平滑移动平均线 （DIF-DEA）*2
            MACD = 2.0 * (DIF - DEA);

            Time = time;
        }

        public MACDIndicator(DateTime time, double closePrice, IMACD preMACD)
        {
            // 快速移动平均值周期 默认12
            SlowEMACycle = SHORT;

            // 快速移动平均值  EMA12 = 前一日EMA12 X 11/13 + 今日收盘 X 2/13
            SlowEMA = (2 * closePrice + (SHORT - 1) * preMACD.SlowEMA) / (SHORT + 1);

            // 慢速移动平均值 默认26
            FastEMACycle = LONG;

            // 慢速移动平均值  EMA26 = 前一日EMA26 X 25/27 + 今日收盘 X 2/27
            FastEMA = (2 * closePrice + (LONG - 1) * preMACD.FastEMA) / (LONG + 1);

            // 差离值   DIF = EMA12 - EMA26
            DIF = SlowEMA - FastEMA;

            // 离差平均值   DEA = （前一日DEA X 8/10 + 今日DIF X 2/10）
            DEA = (2 * DIF + (M - 1) * preMACD.DEA) / (M + 1);

            // MACD 指数平滑移动平均线 （DIF-DEA）*2
            MACD = 2.0 * (DIF - DEA);

            Time = time;
        }
        
    }
}
