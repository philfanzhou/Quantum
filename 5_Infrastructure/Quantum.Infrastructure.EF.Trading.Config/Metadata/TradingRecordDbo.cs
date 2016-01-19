using System;

namespace Quantum.Infrastructure.EF.Trading.Config
{
    /// <summary>
    /// 交易记录数据定义
    /// </summary>
    public class TradingRecordDbo
    {
        /// <summary>
        /// 交易帐号
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeType { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commissions { get; set; }

        /// <summary>
        /// 印花税
        /// </summary>
        public decimal StampDuty { get; set; }

        /// <summary>
        /// 过户费
        /// </summary>
        public decimal TransferFees { get; set; }

        /// <summary>
        /// 结算费
        /// </summary>
        public decimal FeesSettlement { get; set; }
    }
}
