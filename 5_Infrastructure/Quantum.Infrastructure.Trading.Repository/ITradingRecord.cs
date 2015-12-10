using System;

namespace Quantum.Infrastructure.Trading.Repository
{
    /// <summary>
    /// 交易记录数据定义
    /// </summary>
    public interface ITradingRecord
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 交易帐号
        /// </summary>
        string AccountId { get; }

        /// <summary>
        /// 交易时间
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// 交易类型
        /// </summary>
        TradeType Type { get; }

        /// <summary>
        /// 股票代码
        /// </summary>
        string StockCode { get; }

        /// <summary>
        /// 数量
        /// </summary>
        int Quantity { get; }

        /// <summary>
        /// 成交价
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// 佣金
        /// </summary>
        decimal Commissions { get; }

        /// <summary>
        /// 印花税
        /// </summary>
        decimal StampDuty { get; }

        /// <summary>
        /// 过户费
        /// </summary>
        decimal TransferFees { get; }

        /// <summary>
        /// 结算费
        /// </summary>
        decimal FeesSettlement { get; }

        ///// <summary>
        ///// 发生金额
        ///// </summary>
        //decimal Amount { get; }
    }

    /// <summary>
    /// 交易类型
    /// </summary>
    public enum TradeType
    {
        /// <summary>
        /// 买入
        /// </summary>
        Buy,
        /// <summary>
        /// 卖出
        /// </summary>
        Sell
    }
}
