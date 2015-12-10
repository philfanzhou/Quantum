using Framework.Infrastructure.Repository;
using Quantum.Infrastructure.Trading.Repository;
using System;
using System.Linq;

namespace Quantum.Domain.Trading
{
    public class Account : IAccount
    {
        #region Field
        private readonly string _accountId;
        private readonly string _accountName;
        #endregion

        #region IAccount Members
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

        #region Constructor
        public Account(string name)
        {
            _accountId = Guid.NewGuid().ToString();
            _accountName = name;
            Principal = 0;
            Balance = 0;
        }
        #endregion

        ///// <summary>
        ///// 总资产
        ///// </summary>
        //public decimal TotalAssets
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        ///// <summary>
        ///// 持仓市值
        ///// </summary>
        //public decimal MarketValue
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

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

        public bool Buy(string code, decimal price, int quantity, DateTime time)
        {
            var tradingRecord = CreateTradingRecord(TradeType.Buy, code, price, quantity, time);
            
            if (Balance + tradingRecord.Amount < 0)
            {
                return false;
            }

            account.Balance = balance;

            var holdingsRepository = context.GetRepository<HoldingsRecordRepository>();
            var holdingsRecord = holdingsRepository.GetByAccountAndCode(this._accountId, code);
            if (holdingsRecord == null)
            {
                holdingsRecord = new HoldingsRecordData()
                {
                    AccountId = this._accountId,
                    StockCode = code,
                    Quantity = quantity
                };
                context.UnitOfWork.RegisterNew(holdingsRecord);
            }
            else
            {
                holdingsRecord = holdingsRepository
                .GetByAccountAndCode(this._accountId, code);
                holdingsRecord.Quantity += quantity;
                context.UnitOfWork.RegisterModified(holdingsRecord);
            }

            context.UnitOfWork.Commit();


            return true;
        }

        public int AvailableQuantityToBuy(string code, decimal price)
        {
            decimal number = Balance / price;
            if (number < Market.OneHandStock)
            {
                return 0;
            }
            else
            {
                int roundBlocks = Convert.ToInt32(Math.Floor(number / Market.OneHandStock));
                int quantity = roundBlocks * Market.OneHandStock;

                return AvailableQuantityToBuy(code, price, quantity);
            }
        }

        private int AvailableQuantityToBuy(string code, decimal price, int quantity)
        {
            decimal amount = price * quantity;
            amount += TradeCost.GetCommission(price, quantity);
            amount += TradeCost.GetTransferFees(code, price, quantity);

            if (amount > Balance)
            {
                return AvailableQuantityToBuy(code, price, quantity - Market.OneHandStock);
            }
            else
            {
                return quantity;
            }
        }

        public bool Sell(string code, decimal price, int quantity)
        {
            int availableQuantity = AvailableQuantityToSell(code);
            if (quantity > availableQuantity)
            {
                return false;
            }

            var tradingRecord = this.CreateTradingRecord(TradeType.Sell, code, price, quantity);

            using (IRepositoryContext context = RepositoryContext.Create())
            {
                context.UnitOfWork.RegisterNew(tradingRecord);

                var holdingsRepository = context.GetRepository<HoldingsRecordRepository>();
                var holdingsRecord = holdingsRepository.GetByAccountAndCode(this._accountId, code);
                if (holdingsRecord.Quantity - quantity == 0)
                {
                    context.UnitOfWork.RegisterDeleted(holdingsRecord);
                }
                else
                {
                    holdingsRecord.Quantity = holdingsRecord.Quantity - quantity;
                    context.UnitOfWork.RegisterModified(holdingsRecord);
                }

                var accountRepository = context.GetRepository<Repository<AccountData>>();
                AccountData account = accountRepository.Get(this._accountId);
                account.Balance = account.Balance + tradingRecord.Amount;
                context.UnitOfWork.RegisterModified(account);

                context.UnitOfWork.Commit();
            }

            return true;
        }

        public int AvailableQuantityToSell(string code)
        {
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var holdingsRepository = context.GetRepository<HoldingsRecordRepository>();
                var holdingsRecord = holdingsRepository.GetByAccountAndCode(this._accountId, code);
                if (holdingsRecord == null)
                {
                    return 0;
                }

                var tradingRepository = context.GetRepository<TradingRecordDataRepository>();
                var todayBuyRecord = tradingRepository.GetBuyRecord(this._accountId, code, Market.Time);
                int todayBuyQuantity = todayBuyRecord.Sum(p => p.Quantity);

                return holdingsRecord.Quantity - todayBuyQuantity;
            }
        }

        private TradingRecord CreateTradingRecord(TradeType type, string code, decimal price, int quantity, DateTime time)
        {
            TradingRecord record = new TradingRecord
            {
                AccountId = this._accountId,
                Date = time,
                Type = type,
                StockCode = code,
                Quantity = quantity,
                Price = price,
                FeesSettlement = 0m,
                Commissions = TradeCost.GetCommission(price, quantity),
                TransferFees = TradeCost.GetTransferFees(code, price, quantity),
                StampDuty = TradeCost.GetStampDuty(type, code, price, quantity)
            };

            decimal amount = (price * quantity) + record.Commissions + record.StampDuty +
                record.TransferFees + record.FeesSettlement;
            if (type == TradeType.Buy)
            {
                record.Amount = -amount;
            }
            else
            {
                record.Amount = amount;
            }

            return record;
        }
    }
}

