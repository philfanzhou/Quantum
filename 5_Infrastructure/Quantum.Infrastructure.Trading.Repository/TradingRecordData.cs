using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.Trading.Metadata
{
    public class TradingRecordData
    {
        public string AccountId { get; set; }

        DateTime Date { get; set; }

        TradeType Type { get; set; }

        string StockCode { get; set; }

        int Quantity { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        double Price { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        double Commissions { get; set; }

        /// <summary>
        /// 印花税
        /// </summary>
        double StampDuty { get; set; }

        /// <summary>
        /// 过户费
        /// </summary>
        double TransferFees { get; set; }

        /// <summary>
        /// 结算费
        /// </summary>
        double FeesSettlement { get; set; }
    }

    public enum TradeType
    {
        Buy,
        Sell
    }
}
