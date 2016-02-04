namespace Ore.Infrastructure.MarketData
{
    /// <summary>
    /// 分时数据定义
    /// </summary>
    public interface IStockIntraday : ITimeSeries
    {
        /// <summary>
        /// 当前价
        /// </summary>
        double Current { get; }

        /// <summary>
        /// 均价 = 当前时刻总成交额 / 当前时刻总成交量
        /// </summary>
        double AveragePrice { get; }

        /// <summary>
        /// 前一交易日收盘价
        /// </summary>
        double YesterdayClose { get; }

        /// <summary>
        /// 分时成交量
        /// </summary>
        double Volume { get; }

        /// <summary>
        /// 分时成交额
        /// </summary>
        double Amount { get; }

        /// <summary>
        /// 委买
        /// </summary>
        double BuyVolume { get; }

        /// <summary>
        /// 委卖
        /// </summary>
        double SellVolume { get; }
    }
}
