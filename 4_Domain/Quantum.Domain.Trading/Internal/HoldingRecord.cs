using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Trading
{
    internal class HoldingsRecord : IHoldingsRecord
    {
        #region Field
        private readonly string _accountId;
        private readonly string _stockCode;
        #endregion

        public HoldingsRecord(string accountId, string stockCode)
        {
            _accountId = accountId;
            _stockCode = stockCode;
        }

        public int Quantity { get; private set; }

        public string StockCode { get { return _stockCode; } }

        public double Price { get; set; }

        /// <summary>
        /// 获取持仓可用余额
        /// </summary>
        /// <returns></returns>
        public int Balance
        {
            get
            {
                // 无交易记录直接返回0
                if (this.Quantity <= 0 || this._tradingRecords.Count <= 0)
                {
                    return 0;
                }

                // 交易时间小于最新交易记录时间--返回0
                var latestRecord = this._tradingRecords.OrderBy(p => p.Date).Last();
                if (SystemTime.Now.Date < latestRecord.Date.Date)
                {
                    return 0;
                }

                int todayBuyQuantity = GetTodayBuyQuantity();
                if (this.Quantity > todayBuyQuantity)
                {
                    return Quantity - todayBuyQuantity;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取持仓冻结数量
        /// </summary>
        /// <returns></returns>
        public int FrozenQuantity
        {
            get
            {
                // 无交易记录直接返回0
                if (this.Quantity <= 0 || this._tradingRecords.Count <= 0)
                {
                    return 0;
                }

                // 交易时间小于最新交易记录时间--返回0
                var latestRecord = this._tradingRecords.OrderBy(p => p.Date).Last();
                if (SystemTime.Now.Date < latestRecord.Date.Date)
                {
                    return 0;
                }

                return GetTodayBuyQuantity();
            }
        }

        public void Add(ITradingRecord tradingRecord)
        {
            if(tradingRecord.StockCode != this.StockCode)
            {
                throw new ArgumentOutOfRangeException("tradingRecord");
            }

            if(tradingRecord.Type == TradeType.Buy)
            {
                this.Quantity += tradingRecord.Quantity;
            }
            else
            {
                this.Quantity -= tradingRecord.Quantity;
            }

            // 用交易记录的价格来更新持仓记录的价格
            this.Price = tradingRecord.Price;

            this._tradingRecords.Add(tradingRecord);
        }

        /// <summary>
        /// 取得今天的购买数量
        /// </summary>
        /// <returns></returns>
        private int GetTodayBuyQuantity()
        {
            var todayBuyRecords = this._tradingRecords.Where(p =>
                p.Date.Date == SystemTime.Now.Date &&
                p.Type == TradeType.Buy);
            int todayBuyQuantity = todayBuyRecords.Sum(p => p.Quantity);

            return todayBuyQuantity;
        }
    }
}
