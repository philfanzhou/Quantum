using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.Trading
{
    internal class HoldingsRecord : IHoldingsRecord
    {
        #region Field
        private readonly string _stockCode;
        private int _quantity;
        private readonly List<ITradingRecord> _tradingRecords = new List<ITradingRecord>();
        #endregion

        #region Constructor
        private HoldingsRecord(string stockCode)
        {
            _stockCode = stockCode;
        }

        internal HoldingsRecord(string stockCode, int quantity, IEnumerable<ITradingRecord> tradingRecords)
        {
            _stockCode = stockCode;
            _quantity = quantity;
            _tradingRecords = tradingRecords.ToList();
        }
        #endregion

        public string StockCode { get { return _stockCode; } }

        public int Quantity { get { return _quantity; } }

        public IEnumerable<ITradingRecord> TradingRecords { get { return _tradingRecords; } }

        public double Cost
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double FloatingProfitAndLoss
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public float Proportion
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public decimal MarketValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int GetFrozenQuantity(DateTime time)
        {
            // 交易日期小于最新交易记录日期
            // 比如最后一次交易为2016.1.20，查询2016.1.19的冻结数量就不合法. 因为这样的时间不符合常理
            var latestRecord = this._tradingRecords.OrderBy(p => p.Time).Last();
            if (time.Date < latestRecord.Time.Date)
            {
                return _quantity;
            }

            var todayBuyRecords = this._tradingRecords.Where(p =>
                p.Time.Date == time.Date &&
                p.Type == TradeType.Buy);
            int todayBuyQuantity = todayBuyRecords.Sum(p => p.Quantity);

            return todayBuyQuantity;
        }

        public int GetAvailableQuantity(DateTime time)
        {
            // 可卖数量 = 持仓 - 冻结
            int frozenQuantity = GetFrozenQuantity(time);
            if (this._quantity > frozenQuantity)
            {
                return _quantity - frozenQuantity;
            }
            else
            {
                return 0;
            }
        }

        #region Internal Method
        internal static HoldingsRecord Create(string stockCode)
        {
            return new HoldingsRecord(stockCode);
        }

        internal void Add(ITradingRecord tradingRecord)
        {
            if (tradingRecord.Type == TradeType.Buy)
            {
                this._quantity += tradingRecord.Quantity;
            }
            else
            {
                this._quantity -= tradingRecord.Quantity;
            }
            
            this._tradingRecords.Add(tradingRecord);
        }
        #endregion
    }
}
