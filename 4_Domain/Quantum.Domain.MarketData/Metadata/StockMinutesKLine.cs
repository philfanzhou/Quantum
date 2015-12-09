using Ore.Infrastructure.MarketData;
using System;

namespace Quantum.Domain.MarketData
{
    internal class StockMinutesKLine : IStockKLine
    {
        #region IStockKLine Members
        public string Code { get; set; }

        public Market Market { get; set; }

        public string ShortName { get; set; }

        public DateTime Time { get; set; }

        public double Open { get; set; }

        public double PreClose { get; set; }

        public double Current { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Volume { get; set; }

        public double Amount { get; set; }
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
            return Time.ToString("yyyy-MM-dd hh:mm:ss") + string.Format("  Price:{0}", Current);
        }
    }
}
