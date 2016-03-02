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

        public MAIndicator(int cycle, IEnumerable<double> daysClosingPrice)
        {
            Cycle = cycle;

            if (daysClosingPrice != null && daysClosingPrice.Count() > 0)
            {
                Value = MACalculator(daysClosingPrice.ToList());
            }

            Time = DateTime.Now;
        }

        private double MACalculator(List<double> daysClosingPrice)
        {
            if(Cycle <= 0)
                throw new InvalidOperationException("Can not calculate MA due to Cycle <= 0");

            if (daysClosingPrice.Count < Cycle)
                throw new InvalidOperationException("Can not calculate MA due to the closing price data number < Cycle");

            double valMA = 0;
            if (daysClosingPrice != null)
            {
                double sum = 0;
                for (int i = 0; i < daysClosingPrice.Count; i++)
                {
                    sum += daysClosingPrice[i];
                    if(i == Cycle)
                    {
                        valMA = sum / Cycle;
                        break;
                    }
                }
            }
            return valMA;
        }
    }
}
