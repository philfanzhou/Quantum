using System;

namespace Quantum.Domain.MarketData
{
    internal class AveragePrice : IAveragePrice
    {
        public DateTime Time { get; internal set; }

        public double Volume { get; internal set; }

        public double Amount { get; internal set; }

        public double Price { get; internal set; }

        public double Average
        {
            get
            {
                return Math.Round(Price, 2, MidpointRounding.AwayFromZero);
            }
        }

        public double Eccentricity
        {
            get
            {
                return Math.Round((Average - Price) / Price, 2, MidpointRounding.AwayFromZero);
            }
        }
    }
}
