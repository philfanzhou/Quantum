using System;

namespace Quantum.Domain.Trading
{
    internal class TradingRecord : ITradingRecord
    {
        #region Constructor
        public TradingRecord(DateTime time, TradeType type, string stockCode, 
            double price, int quantity)
        {
            Time = time;
            Type = type;
            StockCode = stockCode;
            Quantity = quantity;
            Price = price;
        }
        #endregion

        public DateTime Time { get; private set; }

        public TradeType Type { get; private set; }

        public string StockCode { get; private set; }

        public int Quantity { get; private set; }

        public double Price { get; private set; }

        public decimal Commissions
        {
            get { return TradeCost.GetCommission(Price, Quantity); }
        }

        public decimal StampDuty
        {
            get { return TradeCost.GetStampDuty(Type, StockCode, Price, Quantity); }
        }

        public decimal TransferFees
        {
            get { return TradeCost.GetTransferFees(StockCode, Price, Quantity); }
        }

        public decimal FeesSettlement
        {
            get { return 0m; }
        }
        
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
