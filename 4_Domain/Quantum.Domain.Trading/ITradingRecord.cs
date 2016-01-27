using System;

namespace Quantum.Domain.Trading
{
    public interface ITradingRecord
    {
        #region Property
        /// <summary>
        /// 交易时间
        /// </summary>
        DateTime Time { get; }

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
        #endregion

        #region Method
        /// <summary>
        /// 获取交易的总金额
        /// </summary>
        decimal GetAmount();
        #endregion
    }
}
