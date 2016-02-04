using System;

namespace Quantum.Domain.MarketData.Entity
{
    internal class BuyAgainstSell : IBuyAgainstSell
    {
        public BuyAgainstSell(IStockIntraday stockIntraday)
        {
            this.Time = stockIntraday.Time;
            this.BuyVolume = stockIntraday.BuyVolume;
            this.SellVolume = stockIntraday.SellVolume;
        }

        public DateTime Time { get; private set; }

        public double BuyVolume { get; private set; }

        public double SellVolume { get; private set; }

        public double CommissionDiff
        {
            get
            {
                return BuyVolume - SellVolume;
            }
        }

        public double CommissionRatio
        {
            get
            {
                return Math.Round(CommissionDiff / (BuyVolume + SellVolume) * 100, 2, MidpointRounding.AwayFromZero);
            }
        }

        public double MultiEmptyRatio
        {
            get
            {
                return Math.Round(BuyVolume / SellVolume, 2, MidpointRounding.AwayFromZero);
            }
        }
    }
}
