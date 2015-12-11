using Quantum.Infrastructure.Trading.Repository;
using System;
using System.Collections.Generic;

namespace Quantum.Domain.Trading
{
    internal class Account : IAccountData, IAccount
    {
        #region Field
        private readonly string _accountId = Guid.NewGuid().ToString();
        private readonly string _accountName;
        private readonly Dictionary<string, HoldingsRecord> _holdingRecords = new Dictionary<string, HoldingsRecord>();
        private readonly List<ITradingRecord> _tradingRecords = new List<ITradingRecord>();
        #endregion

        #region Constructor
        public Account(string name)
        {
            _accountName = name;
        }
        #endregion

        #region IAccountData Members
        /// <summary>
        /// 帐号
        /// </summary>
        public string Id
        {
            get { return _accountId; }
        }

        /// <summary>
        /// 户名
        /// </summary>
        public string Name
        {
            get { return _accountName; }
        }

        /// <summary>
        /// 本金
        /// </summary>
        public decimal Principal { get; private set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; private set; }
        #endregion

        #region IAccount Members
        public void TransferIn(decimal amount)
        {
            Principal += amount;
            Balance += amount;
        }

        public bool TransferOut(decimal amount)
        {
            if (amount > Balance)
            {
                return false;
            }

            Principal -= amount;
            Balance -= amount;
            return true;
        }

        public bool Buy(string stockCode, double price, int quantity)
        {
            var tradingRecord = new TradingRecord(this.Id, TradeType.Buy, stockCode, price, quantity);
            if (this.Balance - tradingRecord.Amount < 0)
            {
                return false;
            }

            // 减去账户余额
            this.Balance -= tradingRecord.Amount;

            // 查找或生成持仓记录
            HoldingsRecord holdingRecord;
            if (!this._holdingRecords.TryGetValue(tradingRecord.StockCode, out holdingRecord))
            {
                holdingRecord = new HoldingsRecord(this._accountId, tradingRecord.StockCode);
                this._holdingRecords.Add(holdingRecord.StockCode, holdingRecord);
            }

            // 保存交易记录
            holdingRecord.Add(tradingRecord);
            this._tradingRecords.Add(tradingRecord);

            return true;
        }

        public int AvailableQuantityToBuy(string stockCode, double price)
        {
            decimal number = Balance / (decimal)price;
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

        public bool Sell(string stockCode, double price, int quantity)
        {
            HoldingsRecord holdingsRecord;
            if (!this._holdingRecords.TryGetValue(stockCode, out holdingsRecord))
            {
                return false;
            }
            
            if (quantity > holdingsRecord.Balance)
            {
                return false;
            }

            // 更新持仓和交易记录
            var tradingRecord = new TradingRecord(this._accountId, TradeType.Sell, stockCode, price, quantity);
            holdingsRecord.Add(tradingRecord);
            this._tradingRecords.Add(tradingRecord);

            // 更新账户余额
            this.Balance += tradingRecord.Amount;

            // 持仓卖完之后要删除持仓记录
            if(holdingsRecord.Quantity <= 0)
            {
                this._holdingRecords.Remove(holdingsRecord.StockCode);
            }

            return true;
        }
        #endregion

        private int AvailableQuantityToBuy(string stockCode, double price, int quantity)
        {
            decimal amount = (decimal)price * quantity;
            amount += TradeCost.GetCommission(price, quantity);
            amount += TradeCost.GetTransferFees(stockCode, price, quantity);

            if (amount > Balance)
            {
                return AvailableQuantityToBuy(stockCode, price, quantity - Market.OneHandStock);
            }
            else
            {
                return quantity;
            }
        }
    }
}

