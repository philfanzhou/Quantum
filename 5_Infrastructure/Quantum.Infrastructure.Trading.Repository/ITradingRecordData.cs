using System;

namespace Quantum.Infrastructure.Trading.Repository
{
    /// <summary>
    /// 交易记录数据定义
    /// </summary>
    public interface ITradingRecordData
    {
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
        double Price { get; }

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
    }
}
