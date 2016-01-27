using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.Trading
{
    [Serializable]
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

        internal HoldingsRecord(IHoldingsRecord holdingsRecord)
        {
            _stockCode = holdingsRecord.StockCode;
            _quantity = holdingsRecord.Quantity;
            
            foreach(var item in holdingsRecord.TradingRecords)
            {
                _tradingRecords.Add(new TradingRecord(item));
            }
        }
        #endregion

        #region Property
        public string StockCode { get { return _stockCode; } }

        public int Quantity { get { return _quantity; } }

        public IEnumerable<ITradingRecord> TradingRecords { get { return _tradingRecords; } }
        #endregion

        #region Public Method
        public double GetCost()
        {
            if (_quantity == 0)
            {
                return 0;
            }

            decimal totalCost = CalculateTotalCost();
            decimal cost = totalCost / _quantity;
            return (double)Math.Round(cost, 3, MidpointRounding.AwayFromZero);
        }

        public decimal GetFloatingProfitAndLoss()
        {
            return GetMarketValue() - CalculateTotalCost();
        }

        public float GetProportion()
        {
            if (_quantity == 0)
            {
                return 0;
            }

            // 浮动盈亏为0则盈亏比例为0
            if (GetFloatingProfitAndLoss() - 0 <= 0.00000001m)
            {
                return 0;
            }

            // = 浮动盈亏 / （市值 - 浮动盈亏）
            decimal totalCost = CalculateTotalCost();
            decimal marketValue = GetMarketValue();
            decimal proportion = (1 - (totalCost / (marketValue - totalCost))) * 100;

            return (float)Math.Round(proportion, 2, MidpointRounding.AwayFromZero);
        }

        public decimal GetMarketValue()
        {
            if (_quantity == 0)
            {
                return 0;
            }

            return Math.Round(CalculateMarketValue(), 3, MidpointRounding.AwayFromZero);
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
        #endregion

        #region Internal Method
        /// <summary>
        /// 新建持仓记录
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        internal static HoldingsRecord Create(string stockCode)
        {
            return new HoldingsRecord(stockCode);
        }

        /// <summary>
        /// 添加交易记录
        /// </summary>
        /// <param name="tradingRecord"></param>
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

        #region Private Method
        /// <summary>
        /// 计算持仓市值
        /// </summary>
        /// <returns></returns>
        private decimal CalculateMarketValue()
        {
            if (Market.Quotes == null)
            {
                return 0;
            }

            double price = Market.Quotes.GetPrice(this._stockCode);
            decimal marketValue = (decimal)price * _quantity;
            return marketValue;
        }

        /// <summary>
        /// 计算总交易成本
        /// </summary>
        /// <returns></returns>
        private decimal CalculateTotalCost()
        {
            decimal amount = 0m;
            foreach (var tradItem in _tradingRecords)
            {
                if (tradItem.Type == TradeType.Buy)
                {
                    // 买入的所有金额都计入成本
                    amount += tradItem.GetAmount();
                }
                else if (tradItem.Type == TradeType.Sell)
                {
                    // 卖出交易的股票金额作为成本降低
                    amount -= (decimal)tradItem.Price * tradItem.Quantity;

                    // 但是卖出手续费需要计入成本
                    amount += tradItem.Commissions;
                    amount += tradItem.FeesSettlement;
                    amount += tradItem.StampDuty;
                    amount += tradItem.TransferFees;
                }
            }

            return amount;
        }
        #endregion

        public override string ToString()
        {
            return _stockCode + " " + _quantity.ToString();
        }
    }
}
