using System;

namespace PF.IFS.TongHua.DataReader
{
    /// <summary>
    /// 定义K线数据接口
    /// </summary>
    public interface IKlineItem
    {
        /// <summary>
        /// 日期
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// 开盘价
        /// </summary>
        double Open { get; }

        /// <summary>
        /// 最高价
        /// </summary>
        double High { get; }

        /// <summary>
        /// 最低价
        /// </summary>
        double Low { get; }

        /// <summary>
        /// 收盘价
        /// </summary>
        double Close { get; }

        /// <summary>
        /// 成交金额
        /// </summary>
        double Amount { get; }

        /// <summary>
        /// 成交量
        /// </summary>
        double Volume { get; }

        /// <summary>
        /// 换手率
        /// </summary>
        double Turnover { get; }
    }
}