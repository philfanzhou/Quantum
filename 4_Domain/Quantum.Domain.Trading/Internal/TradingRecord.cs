using System;

namespace Quantum.Domain.Trading
{
    [Serializable]
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
            Commissions = TradeCost.GetCommission(Price, Quantity);
            StampDuty = TradeCost.GetStampDuty(Type, StockCode, Price, Quantity);
            TransferFees = TradeCost.GetTransferFees(StockCode, Price, Quantity);
            FeesSettlement = 0m;
        }
        #endregion

        #region Property
        public DateTime Time { get; private set; }

        public TradeType Type { get; private set; }

        public string StockCode { get; private set; }

        public int Quantity { get; private set; }

        public double Price { get; private set; }

        public decimal Commissions { get; private set; }

        public decimal StampDuty { get; private set; }

        public decimal TransferFees { get; private set; }

        public decimal FeesSettlement { get; private set; }
        #endregion

        #region Public Method
        public decimal GetAmount()
        {
            decimal amount
                = ((decimal)Price * Quantity)
                + Commissions
                + StampDuty
                + TransferFees
                + FeesSettlement;

            return amount;
        }
        #endregion

        public override string ToString()
        {
            return StockCode + " " + Time.ToString("yyyy/MM/dd hh-mm-ss") + " " + Price;
        }
    }
}
