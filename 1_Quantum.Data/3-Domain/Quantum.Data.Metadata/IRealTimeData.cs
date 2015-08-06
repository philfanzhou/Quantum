using System;

namespace Quantum.Data.Metadata
{
    /// <summary>
    /// 实时行情数据结构接口
    /// </summary>
    public interface IRealTimeData
    {
        /// <summary>
        /// 挂牌代码
        /// </summary>
        string Code { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 今开
        /// </summary>
        double TodayOpen { get; }

        /// <summary>
        /// 昨收
        /// </summary>
        double YesterdayClose { get; }

        /// <summary>
        /// 成交
        /// </summary>
        double Price { get; }

        /// <summary>
        /// 最高
        /// </summary>
        double High { get; }

        /// <summary>
        /// 最低
        /// </summary>
        double Low { get; }

        /// <summary>
        /// 成交量
        /// </summary>
        double Volume { get; }
        
        /// <summary>
        /// 成交额
        /// </summary>
        double Amount { get; }

        /// <summary>
        /// 卖五价
        /// </summary>
        double SellFivePrice { get; }

        /// <summary>
        /// 卖五量
        /// </summary>
        double SellFiveVolume { get; }

        /// <summary>
        /// 卖四价
        /// </summary>
        double SellFourPrice { get; }

        /// <summary>
        /// 卖四量
        /// </summary>
        double SellFourVolume { get; }

        /// <summary>
        /// 卖三价
        /// </summary>
        double SellThreePrice { get; }

        /// <summary>
        /// 卖三量
        /// </summary>
        double SellThreeVolume { get; }

        /// <summary>
        /// 卖二价
        /// </summary>
        double SellTwoPrice { get; }

        /// <summary>
        /// 卖二量
        /// </summary>
        double SellTwoVolume { get; }

        /// <summary>
        /// 卖一价
        /// </summary>
        double SellOnePrice { get; }

        /// <summary>
        /// 卖一量
        /// </summary>
        double SellOneVolume { get; }

        /// <summary>
        /// 买一价
        /// </summary>
        double BuyOnePrice { get; }

        /// <summary>
        /// 买一量
        /// </summary>
        double BuyOneVolume { get; }

        /// <summary>
        /// 买二价
        /// </summary>
        double BuyTwoPrice { get; }

        /// <summary>
        /// 买二量
        /// </summary>
        double BuyTwoVolume { get; }

        /// <summary>
        /// 买三价
        /// </summary>
        double BuyThreePrice { get; }

        /// <summary>
        /// 买三量
        /// </summary>
        double BuyThreeVolume { get; }

        /// <summary>
        /// 买四价
        /// </summary>
        double BuyFourPrice { get; }

        /// <summary>
        /// 买四量
        /// </summary>
        double BuyFourVolume { get; }

        /// <summary>
        /// 买五价
        /// </summary>
        double BuyFivePrice { get; }

        /// <summary>
        /// 买五量
        /// </summary>
        double BuyFiveVolume { get; }

        /// <summary>
        /// 日期与时间
        /// </summary>
        DateTime Time { get; }
    }
}
