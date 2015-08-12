using Framework.Infrastructure.Repository;
using System;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class TradingRecordData : Entity
    {
        public string AccountId { get; set; }

        public DateTime Date { get; set; }

        public TradeType Type { get; set; }

        public string StockCode { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public decimal Price { get; set; }

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

        public TradingRecordData()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }

    public enum TradeType
    {
        Buy,
        Sell
    }
}
