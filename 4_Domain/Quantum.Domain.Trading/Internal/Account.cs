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
        private readonly string _accountId = Guid.NewGuid().ToString();
        /// <summary>
        /// 户名
        /// </summary>
        private readonly string _accountName;
        /// <summary>
        /// 本金
        /// </summary>
        private decimal _principal;
        /// <summary>
        /// 余额
        /// </summary>
        private decimal _balance;
        #endregion

        #region Constructor
        public Account(string name)
        {
            _accountName = name;
        }
        #endregion

        #region IAccount Members
        string IAccount.Id
        {
            get { return _accountId; }
        }
        
        string IAccount.Name
        {
            get { return _accountName; }
        }
        
        decimal IAccount.Principal
        {
            get { return _principal; }
        }

        decimal IAccount.Balance
        {
            get { return _balance; }
        }

        decimal IAccount.TotalAssets
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        decimal IAccount.MarketValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void IAccount.TransferIn(decimal amount)
        {
            Principal += amount;
            Balance += amount;
        }

        bool IAccount.TransferOut(decimal amount)
        {
            if (amount > Balance)
            {
                return false;
            }

            Principal -= amount;
            Balance -= amount;
            return true;
        }

        bool IAccount.Buy(DateTime time, string stockCode, double price, int quantity)
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

        bool IAccount.Sell(DateTime time, string stockCode, double price, int quantity)
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

        int IAccount.AvailableQuantityToBuy(string stockCode, double price)
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

