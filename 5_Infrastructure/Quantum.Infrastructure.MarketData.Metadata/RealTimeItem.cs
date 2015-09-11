using System;

namespace Quantum.Infrastructure.MarketData.Metadata
{
    /// <summary>
    /// 实时数据结构体定义
    /// </summary>
    public struct RealTimeItem
    {
        /// <summary>
        /// 今开
        /// </summary>
        public double TodayOpen { get; set; }

        /// <summary>
        /// 昨收
        /// </summary>
        public double YesterdayClose { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 最高
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// 最低
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 日期与时间
        /// </summary>
        public DateTime Time { get; set; }

        #region 卖盘

        /// <summary>
        /// 卖五价
        /// </summary>
        public double SellFivePrice { get; set; }

        /// <summary>
        /// 卖五量
        /// </summary>
        public double SellFiveVolume { get; set; }

        /// <summary>
        /// 卖四价
        /// </summary>
        public double SellFourPrice { get; set; }

        /// <summary>
        /// 卖四量
        /// </summary>
        public double SellFourVolume { get; set; }

        /// <summary>
        /// 卖三价
        /// </summary>
        public double SellThreePrice { get; set; }

        /// <summary>
        /// 卖三量
        /// </summary>
        public double SellThreeVolume { get; set; }

        /// <summary>
        /// 卖二价
        /// </summary>
        public double SellTwoPrice { get; set; }

        /// <summary>
        /// 卖二量
        /// </summary>
        public double SellTwoVolume { get; set; }

        /// <summary>
        /// 卖一价
        /// </summary>
        public double SellOnePrice { get; set; }

        /// <summary>
        /// 卖一量
        /// </summary>
        public double SellOneVolume { get; set; }

        #endregion

        #region 买盘

        /// <summary>
        /// 买一价
        /// </summary>
        public double BuyOnePrice { get; set; }

        /// <summary>
        /// 买一量
        /// </summary>
        public double BuyOneVolume { get; set; }

        /// <summary>
        /// 买二价
        /// </summary>
        public double BuyTwoPrice { get; set; }

        /// <summary>
        /// 买二量
        /// </summary>
        public double BuyTwoVolume { get; set; }

        /// <summary>
        /// 买三价
        /// </summary>
        public double BuyThreePrice { get; set; }

        /// <summary>
        /// 买三量
        /// </summary>
        public double BuyThreeVolume { get; set; }

        /// <summary>
        /// 买四价
        /// </summary>
        public double BuyFourPrice { get; set; }

        /// <summary>
        /// 买四量
        /// </summary>
        public double BuyFourVolume { get; set; }

        /// <summary>
        /// 买五价
        /// </summary>
        public double BuyFivePrice { get; set; }

        /// <summary>
        /// 买五量
        /// </summary>
        public double BuyFiveVolume { get; set; }

        #endregion

        public override string ToString()
        {
            return this.Price.ToString();
        }
    }
}
