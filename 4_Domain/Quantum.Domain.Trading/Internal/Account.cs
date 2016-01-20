using System;
using System.Collections.Generic;

namespace Quantum.Domain.Trading
{
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
        private readonly Dictionary<string, HoldingsRecord> _holdingRecords =
            new Dictionary<string, HoldingsRecord>();
        #endregion

        #region Constructor
        private Account(string name)
        {
            _id = new Guid().ToString();
            _name = name;
        }

        internal Account(string id, string name, decimal principal, decimal balance, IEnumerable<HoldingsRecord> holdingRecords)
        {
            _id = id;
            _name = name;
            _principal = principal;
            _balance = balance;
            
            foreach(var record in holdingRecords)
            {
                _holdingRecords.Add(record.StockCode, record);
            }
        }
        #endregion

        public string Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public decimal Principal
        {
            get { return _principal; }
        }

        public decimal Balance
        {
            get { return _balance; }
        }

        public decimal TotalAssets
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
            throw new NotImplementedException();
        }

        public IEnumerable<ITradingRecord> GetAllTradingRecord()
        {
            throw new NotImplementedException();
        }

        public bool Buy(DateTime time, string stockCode, double price, int quantity)
        {
            var tradingRecord = new TradingRecord(time, TradeType.Buy, stockCode, price, quantity);
            if (this._balance - tradingRecord.Amount < 0)
            {
                return false;
            }

            // 减去账户余额
            this._balance -= tradingRecord.Amount;

            // 查找或生成持仓记录
            HoldingsRecord holdingRecord;
            if (!this._holdingRecords.TryGetValue(tradingRecord.StockCode, out holdingRecord))
            {
                holdingRecord = HoldingsRecord.Create(tradingRecord.StockCode);
                this._holdingRecords.Add(holdingRecord.StockCode, holdingRecord);
            }

            // 保存交易记录
            holdingRecord.Add(tradingRecord);

            return true;
        }

        public bool Sell(DateTime time, string stockCode, double price, int quantity)
        {
            HoldingsRecord holdingsRecord;
            if (!this._holdingRecords.TryGetValue(stockCode, out holdingsRecord))
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
            this._balance += tradingRecord.Amount;

            // 持仓卖完之后要删除持仓记录
            if(holdingsRecord.Quantity <= 0)
            {
                this._holdingRecords.Remove(holdingsRecord.StockCode);
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
    }
}

