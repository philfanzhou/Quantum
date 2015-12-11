using Quantum.Infrastructure.Trading.Repository;
using System;

namespace Quantum.Domain.Trading
{
    internal class TradingRecord : ITradingRecordData, ITradingRecord
    {
        public TradingRecord(string accountId, TradeType type, string code, 
            double price, int quantity)
        {
            AccountId = accountId;
            Date = SystemTime.Now;
            Type = type;
            StockCode = code;
            Quantity = quantity;
            Price = price;
            FeesSettlement = 0m;
            Commissions = TradeCost.GetCommission(price, quantity);
            TransferFees = TradeCost.GetTransferFees(code, price, quantity);
            StampDuty = TradeCost.GetStampDuty(type, code, price, quantity);
        }

        #region ITradingRecordData Members
        public string AccountId { get; private set; }

        public DateTime Date { get; private set; }

        public TradeType Type { get; private set; }

        public string StockCode { get; private set; }

        public int Quantity { get; private set; }

        public double Price { get; private set; }

        public decimal Commissions { get; private set; }

        public decimal StampDuty { get; private set; }

        public decimal TransferFees { get; private set; }

        public decimal FeesSettlement { get; private set; }
        #endregion

        #region ITradingRecord Members
        /// <summary>
        /// 交易总金额
        /// </summary>
        public decimal Amount
        {
            get
            {
                decimal amount = ((decimal)Price * Quantity) + Commissions + StampDuty + TransferFees + FeesSettlement;
                return amount;
            }
        }
        #endregion

    }
}
