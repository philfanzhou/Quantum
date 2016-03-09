using System;

namespace Quantum.Domain.MarketData
{
    internal class KDJIndicator : IKDJ
    {
        private const int N = 9;
        private const int M1 = 3;
        private const int M2 = 3;

        /// <summary>
        /// 周期（n日、n周等）的RSV值，即未成熟随机指标值
        /// 计算公式: n日RSV =（Cn－Ln）/（Hn－Ln）×100
        /// </summary>
        public double RSV { get; private set; }

        /// <summary>
        /// 当日K值=2/3×前一日K值+1/3×当日RSV
        /// </summary>
        public double KValue { get; private set; }

        /// <summary>
        /// 当日D值 = 2 / 3×前一日D值+1/3×当日K值
        /// </summary>
        public double DValue { get; private set; }

        /// <summary>
        /// J值=3* 当日K值-2* 当日D值
        /// </summary>
        public double JValue { get; private set; }

        public DateTime Time { get; private set; }
        
        public KDJIndicator(DateTime time, double closingPrice, double lowPrice, double highPrice, IKDJ preKDJ = null)
        {
            RSV = (closingPrice - lowPrice) / (highPrice - lowPrice) * 100;
            var a = (preKDJ == null ? (1 * RSV + (M1 - 1) * 0) / 1 : (1 * RSV + (M1 - 1) * preKDJ.KValue) / M1);
            var b = (preKDJ == null ? (1 * a + (M2 - 1) * 0) / 1 : (1 * a + (M2 - 1) * preKDJ.DValue) / M2);
            var e = 3 * a - 2 * b;

            KValue = a;
            DValue = b;
            JValue = e;

            if (a < 0)
                KValue = 0;
            if (a > 100)
                KValue = 100;
            if (b < 0)
                DValue = 0;
            if (b > 100)
                DValue = 100;
            if (e < 0)
                JValue = 0;
            if (e > 100)
                JValue = 100;

            Time = time;
        }        
    }
}
