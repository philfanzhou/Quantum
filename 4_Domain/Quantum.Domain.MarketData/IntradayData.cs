using Quantum.Domain.Indicator;
using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 分时数据
    /// </summary>
    public class IntradayData : IBuyAgainstSell
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 均价
        /// </summary>
        public double AveragePrice { get; set; }

        /// <summary>
        /// 涨跌
        /// </summary>
        public double Change { get; set; }

        /// <summary>
        /// 涨跌幅
        /// </summary>
        public double ChangeRate { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public double TotalVolume { get; set; }

        /// <summary>
        /// 分时量
        /// </summary>
        public double IntradayVolume { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public double TotalAmount { get; set; }

        /// <summary>
        /// 分时额
        /// </summary>
        public double IntradayAmount { get; set; }

        /// <summary>
        /// 委卖
        /// </summary>
        public double SellVolume { get; set; }

        /// <summary>
        /// 委买
        /// </summary>
        public double BuyVolume { get; set; }
    }
}
