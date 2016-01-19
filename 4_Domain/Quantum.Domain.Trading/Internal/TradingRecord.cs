using System;

namespace Quantum.Domain.Trading
{
    internal class TradingRecord : ITradingRecord
    {
        public TradingRecord(DateTime time, TradeType type, string code, 
            double price, int quantity)
        {
            Time = time;
            Type = type;
            StockCode = code;
            Quantity = quantity;
            Price = price;

            Commissions = TradeCost.GetCommission(price, quantity);
            StampDuty = TradeCost.GetStampDuty(type, code, price, quantity);
            TransferFees = TradeCost.GetTransferFees(code, price, quantity);
            FeesSettlement = 0m;
        }
        
        public DateTime Time { get; private set; }

        public TradeType Type { get; private set; }

        public string StockCode { get; private set; }

        public int Quantity { get; private set; }

        public double Price { get; private set; }

        public decimal Commissions { get; private set; }

        public decimal StampDuty { get; private set; }

        public decimal TransferFees { get; private set; }

        public decimal FeesSettlement { get; private set; }

        /// <summary>
        /// 交易总金额
        /// </summary>
        public decimal Amount
        {
            get
            {
                decimal amount
                    = ((decimal)Price * Quantity)
                    + Commissions
                    + StampDuty
                    + TransferFees
                    + FeesSettlement;

                return amount;
            }
        }
    }
}
