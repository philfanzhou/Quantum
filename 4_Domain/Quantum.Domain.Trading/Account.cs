using Framework.Infrastructure.Repository;
using Quantum.Infrastructure.Trading.Repository;
using System;
using System.Linq;

namespace Quantum.Domain.Trading
{
    public class Account : IAccount
    {
        /// <summary>
        /// 一手股票包含100股
        /// </summary>
        private const int NumberOfRoundBlocks = 100;

        private string accountId;

        public Account(string Id)
        {
            this.accountId = Id;
        }

        public bool Buy(string code, decimal price, int quantity)
        {
            var tradingRecord = this.CreateTradingRecord(TradeType.Buy, code, price, quantity);

            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var accountRepository = context.GetRepository<Repository<AccountData>>();
                AccountData account = accountRepository.Get(this.accountId);
                decimal balance = account.Balance + tradingRecord.Amount;
                if (balance < 0)
                {
                    return false;
                }

                account.Balance = balance;
                context.UnitOfWork.RegisterModified(account);

                context.UnitOfWork.RegisterNew(tradingRecord);

                var holdingsRepository = context.GetRepository<HoldingsRecordRepository>();
                var holdingsRecord = holdingsRepository.GetByAccountAndCode(this.accountId, code);
                if (holdingsRecord == null)
                {
                    holdingsRecord = new HoldingsRecordData()
                    {
                        AccountId = this.accountId,
                        StockCode = code,
                        Quantity = quantity
                    };
                    context.UnitOfWork.RegisterNew(holdingsRecord);
                }
                else
                {
                    holdingsRecord = holdingsRepository
                    .GetByAccountAndCode(this.accountId, code);
                    holdingsRecord.Quantity += quantity;
                    context.UnitOfWork.RegisterModified(holdingsRecord);
                }

                context.UnitOfWork.Commit();
            }
            
            return true;
        }

        public int AvailableQuantityToBuy(string code, decimal price)
        {
            decimal balance;
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<Repository<AccountData>>();
                AccountData accountData = repository.Get(this.accountId);
                balance = accountData.Balance;
            }

            decimal number = balance / price;
            if (number < NumberOfRoundBlocks)
            {
                return 0;
            }
            else
            {
                int roundBlocks = Convert.ToInt32(Math.Floor(number / NumberOfRoundBlocks));
                int quantity =  roundBlocks * NumberOfRoundBlocks;

                return AvailableQuantityToBuy(code, price, quantity, balance);
            }
        }

        public bool Sell(string code, decimal price, int quantity)
        {
            int availableQuantity = AvailableQuantityToSell(code);
            if(quantity > availableQuantity)
            {
                return false;
            }

            var tradingRecord = this.CreateTradingRecord(TradeType.Sell, code, price, quantity);

            using (IRepositoryContext context = RepositoryContext.Create())
            {
                context.UnitOfWork.RegisterNew(tradingRecord);

                var holdingsRepository = context.GetRepository<HoldingsRecordRepository>();
                var holdingsRecord = holdingsRepository.GetByAccountAndCode(this.accountId, code);
                if(holdingsRecord.Quantity - quantity == 0)
                {
                    context.UnitOfWork.RegisterDeleted(holdingsRecord);
                }
                else
                {
                    holdingsRecord.Quantity = holdingsRecord.Quantity - quantity;
                    context.UnitOfWork.RegisterModified(holdingsRecord);
                }

                var accountRepository = context.GetRepository<Repository<AccountData>>();
                AccountData account = accountRepository.Get(this.accountId);
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
                var holdingsRecord = holdingsRepository.GetByAccountAndCode(this.accountId, code);
                if (holdingsRecord == null)
                {
                    return 0;
                }

                var tradingRepository = context.GetRepository<TradingRecordDataRepository>();
                var todayBuyRecord = tradingRepository.GetBuyRecord(this.accountId, code, Market.Time);
                int todayBuyQuantity = todayBuyRecord.Sum(p => p.Quantity);

                return holdingsRecord.Quantity - todayBuyQuantity;
            }
        }

        public void TransferIn(decimal amount)
        {
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<Repository<AccountData>>();
                AccountData accountData = repository.Get(this.accountId);

                accountData.Principal += amount;
                accountData.Balance += amount;

                context.UnitOfWork.RegisterModified(accountData);
                context.UnitOfWork.Commit();
            }
        }

        public void TransferOut(decimal amount)
        {
            throw new NotImplementedException();
        }

        private int AvailableQuantityToBuy(string code, decimal price, int quantity, decimal balance)
        {
            decimal amount = price * quantity;
            amount += TradeCost.GetCommission(price, quantity);
            amount += TradeCost.GetTransferFees(code, price, quantity);

            if (amount > balance)
            {
                return AvailableQuantityToBuy(code, price, quantity - NumberOfRoundBlocks, balance);
            }
            else
            {
                return quantity;
            }
        }

        private TradingRecordData CreateTradingRecord(TradeType type, string code, decimal price, int quantity)
        {
            TradingRecordData record = new TradingRecordData()
            {
                AccountId = this.accountId,
                Date = Market.Time,
                Type = type,
                StockCode = code,
                Quantity = quantity,
                Price = price,
                FeesSettlement = 0m
            };

            record.Commissions = TradeCost.GetCommission(price, quantity);
            record.TransferFees = TradeCost.GetTransferFees(code, price, quantity);
            if (type == TradeType.Sell)
            {
                record.StampDuty = TradeCost.GetStampDuty(code, price, quantity);
            }

            decimal amount = (price * quantity) +
                record.Commissions +
                record.StampDuty +
                record.TransferFees +
                record.FeesSettlement;
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
