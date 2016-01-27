using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.Trading
{
    [Serializable]
    internal class Account : IAccount
    {
        #region Field
        /// <summary>
        /// 账户ID
        /// </summary>
        private readonly string _id;
        /// <summary>
        /// 户名
        /// </summary>
        private readonly string _name;
        /// <summary>
        /// 本金
        /// </summary>
        private decimal _principal;
        /// <summary>
        /// 余额
        /// </summary>
        private decimal _balance;
        /// <summary>
        /// 持仓记录
        /// </summary>
        private readonly Dictionary<string, HoldingsRecord> _holdingsRecords =
            new Dictionary<string, HoldingsRecord>();
        #endregion

        #region Constructor
        private Account(string name)
        {
            _id = Guid.NewGuid().ToString();
            _name = name;
        }

        internal Account(IAccount account)
        {
            _id = account.Id;
            _name = account.Name;
            _principal = account.Principal;
            _balance = account.Balance;

            var holdingsRecords = account.GetAllHoldingsRecord();
            foreach(var item in holdingsRecords)
            {
                var record = new HoldingsRecord(item);
                _holdingsRecords.Add(record.StockCode, record);
            }
        }
        #endregion

        #region Property
        public string Id { get { return _id; } }

        public string Name { get { return _name; } }

        public decimal Principal { get { return _principal; } }

        public decimal Balance { get { return _balance; } }

        public decimal TotalAssets { get { return Balance + MarketValue; } }

        public decimal MarketValue { get { return _holdingsRecords.Values.Sum(p => p.GetMarketValue()); } }
        #endregion

        #region Public Method
        public void TransferIn(decimal amount)
        {
            this._principal += amount;
            this._balance += amount;
        }

        public bool TransferOut(decimal amount)
        {
            if (amount > this._balance)
            {
                return false;
            }

            this._principal -= amount;
            this._balance -= amount;
            return true;
        }

        public IEnumerable<IHoldingsRecord> GetAllHoldingsRecord()
        {
            return _holdingsRecords.Values;
        }

        public IEnumerable<ITradingRecord> GetAllTradingRecord()
        {
            List<ITradingRecord> tradingRecords = new List<ITradingRecord>();
            foreach(var holdingsRecord in _holdingsRecords.Values)
            {
                tradingRecords.AddRange(holdingsRecord.TradingRecords);
            }
            return tradingRecords;
        }

        public bool Buy(DateTime time, string stockCode, double price, int quantity)
        {
            var tradingRecord = new TradingRecord(time, TradeType.Buy, stockCode, price, quantity);
            if (this._balance - tradingRecord.GetAmount() < 0)
            {
                return false;
            }

            // 减去账户余额
            this._balance -= tradingRecord.GetAmount();

            // 查找或生成持仓记录
            HoldingsRecord holdingRecord;
            if (!this._holdingsRecords.TryGetValue(tradingRecord.StockCode, out holdingRecord))
            {
                holdingRecord = HoldingsRecord.Create(tradingRecord.StockCode);
                this._holdingsRecords.Add(holdingRecord.StockCode, holdingRecord);
            }

            // 保存交易记录
            holdingRecord.Add(tradingRecord);

            return true;
        }

        public bool Sell(DateTime time, string stockCode, double price, int quantity)
        {
            HoldingsRecord holdingsRecord;
            if (!this._holdingsRecords.TryGetValue(stockCode, out holdingsRecord))
            {
                return false;
            }
            
            if (quantity > holdingsRecord.GetAvailableQuantity(time))
            {
                return false;
            }

            // 更新持仓和交易记录
            var tradingRecord = new TradingRecord(time, TradeType.Sell, stockCode, price, quantity);
            holdingsRecord.Add(tradingRecord);
            //this._tradingRecords.Add(tradingRecord);

            // 更新账户余额
            this._balance += tradingRecord.GetAmount();

            // 持仓卖完之后要删除持仓记录
            if(holdingsRecord.Quantity <= 0)
            {
                this._holdingsRecords.Remove(holdingsRecord.StockCode);
            }

            return true;
        }

        public int AvailableQuantityToBuy(string stockCode, double price)
        {
            decimal number = this._balance / (decimal)price;
            if (number < Market.OneHandStock)
            {
                return 0;
            }
            else
            {
                int roundBlocks = Convert.ToInt32(Math.Floor(number / Market.OneHandStock));
                int quantity = roundBlocks * Market.OneHandStock;

                return AvailableQuantityToBuy(stockCode, price, quantity);
            }
        }
        #endregion

        #region Internal Method
        internal static Account Create(string name)
        {
            return new Account(name);
        }
        #endregion

        #region Private Method
        private int AvailableQuantityToBuy(string stockCode, double price, int quantity)
        {
            decimal amount = (decimal)price * quantity;
            amount += TradeCost.GetCommission(price, quantity);
            amount += TradeCost.GetTransferFees(stockCode, price, quantity);

            if (amount > this._balance)
            {
                return AvailableQuantityToBuy(stockCode, price, quantity - Market.OneHandStock);
            }
            else
            {
                return quantity;
            }
        }
        #endregion

        public override string ToString()
        {
            return _name;
        }
    }
}

