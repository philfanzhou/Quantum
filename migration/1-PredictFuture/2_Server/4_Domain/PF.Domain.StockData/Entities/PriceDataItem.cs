namespace PF.Domain.StockData.Entities
{
    public abstract class PriceDataItem : StockDataItemBase
    {
        protected PriceDataItem(string id) : base(id) {}

        protected PriceDataItem()
        {
        }

        /// <summary>
        /// 开盘价
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// 收盘价
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// 成交金额
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public double Volume { get; set; }
    }
}