
namespace Quantum.Domain.MarketData
{
    using System;

    /// <summary>
    /// 分时数据
    /// </summary>
    public class IntradayData : IMarketData
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
        public double Volume { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public double Amount { get; set; }
    }
}
