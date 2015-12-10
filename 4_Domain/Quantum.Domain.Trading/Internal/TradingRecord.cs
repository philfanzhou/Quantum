using Quantum.Infrastructure.Trading.Repository;
using System;

namespace Quantum.Domain.Trading
{
    internal class TradingRecord : ITradingRecord
    {
        public string Id { get; private set; }

        public string AccountId { get; private set; }

        public DateTime Date { get; private set; }

        public TradeType Type { get; private set; }

        public string StockCode { get; private set; }

        public int Quantity { get; private set; }
        
        public decimal Price { get; private set; }
        
        public decimal Commissions { get; private set; }
        
        public decimal StampDuty { get; private set; }
        
        public decimal TransferFees { get; private set; }
        
        public decimal FeesSettlement { get; private set; }

        public TradingRecord(string accountId, TradeType type, string code, decimal price, int quantity, DateTime time)
        {
            Id = Guid.NewGuid().ToString();
            AccountId = accountId;
            Date = time;
            Type = type;
            StockCode = code;
            Quantity = quantity;
            Price = price;
            FeesSettlement = 0m;
            Commissions = TradeCost.GetCommission(price, quantity);
            TransferFees = TradeCost.GetTransferFees(code, price, quantity);
            StampDuty = TradeCost.GetStampDuty(type, code, price, quantity);
        }
    }
}
