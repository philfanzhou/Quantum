using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.MarketData.Entity
{
    internal class BuyAgainstSell : IBuyAgainstSell
    {
        public BuyAgainstSell(IStockIntraday stockIntraday)
        {

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
                return Math.Round(CommissionDiff / (BuyVolume + SellVolume) * 100, 2);
            }
        }

        public double MultiEmptyRatio
        {
            get
            {
                return Math.Round(BuyVolume / SellVolume, 2);
            }
        }
    }
}
