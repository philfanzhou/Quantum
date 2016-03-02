using System;

namespace Quantum.Domain.MarketData
{
    internal class KDJIndicator : IKDJ
    {
        /// <summary>
        /// 计算周期（n日、n周等）
        /// </summary>
        public int Cycle { get; private set; }

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

        public KDJIndicator(int cycle, double closingPrice, double lowPrice, double highPrice, double preK = 50, double preD = 50)
        {
            // 计算周期（n日、n周等）
            Cycle = cycle;

            // 周期（n日、n周等）的RSV值，即未成熟随机指标值
            // 计算公式: n日RSV =（Cn－Ln）/（Hn－Ln）×100
            RSV = (closingPrice - lowPrice) / (highPrice - lowPrice) * 100;

            // 当日K值=2/3×前一日K值+1/3×当日RSV
            KValue = 2 / 3 * preK + 1 / 3 * RSV;

            // 当日D值 = 2 / 3×前一日D值+1/3×当日K值
            DValue = 2 / 3 * preD + 1 / 3 * KValue;

            // J值=3* 当日K值-2* 当日D值
            JValue = 3 * KValue - 2 * DValue;

            Time = DateTime.Now;
        }
    }
}
