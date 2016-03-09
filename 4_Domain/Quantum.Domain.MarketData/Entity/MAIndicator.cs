using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.MarketData
{
    internal class MAIndicator : IMA
    {
        /// <summary>
        /// 均线周期 如：5，10，20，60
        /// </summary>
        public int Cycle { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; private set; }

        public DateTime Time { get; private set; }

        public MAIndicator(DateTime time, int cycle, double value)
        {
            Cycle = cycle;
            Value = Math.Round(value, 2);
            Time = time;
        }
    }
}
