using Ore.Infrastructure.MarketData;
using System;

namespace Quantum.Domain.MarketData
{
    internal class StockIntraday : IStockIntraday
    {
        #region IStockIntraday Members
        public string Code { get; set; }

        public Market Market { get; set; }

        public string ShortName { get; set; }

        public DateTime Time { get; set; }

        public double YesterdayClose { get; set; }

        public double Current { get; set; }

        public double AveragePrice { get; set; }

        public double Volume { get; set; }

        public double Amount { get; set; }

        public double BuyVolume { get; set; }

        public double SellVolume { get; set; }
        #endregion

        /// <summary>
        /// 当前时刻的总成交量
        /// </summary>
        public double CurrentTotalVolume { get; set; }

        /// <summary>
        /// 当前时刻的总成交额
        /// </summary>
        public double CurrentTotalAmount { get; set; }

        public override string ToString()
        {
            return Time.ToString("yyyy-MM-dd hh:mm:ss") + string.Format("   Current:{0}", Current);
        }
    }
}
