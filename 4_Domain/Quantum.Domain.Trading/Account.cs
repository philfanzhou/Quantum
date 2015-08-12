using Framework.Infrastructure.Repository;
using Quantum.Infrastructure.Trading.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Trading
{
    internal class Account : IAccount
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
            decimal amount = price * quantity;
            decimal commission = TradeCost.GetCommission(price, quantity);
            decimal transferFees = TradeCost.GetTransferFees(code, price, quantity);
            amount = amount + commission + transferFees;

            decimal balance;
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var accountRepository = context.GetRepository<Repository<AccountData>>();
                AccountData accountData = accountRepository.Get(this.accountId);
                balance = accountData.Balance;

                if (amount > balance)
                {
                    return false;
                }

                accountData.Balance = balance - amount;
                context.UnitOfWork.RegisterModified(accountData);

                TradingRecordData tradingRecord = new TradingRecordData()
                    {
                        AccountId = accountData.Id,
                        Date = DateTime.Now,
                        Type = TradeType.Buy,
                        StockCode = code,
                        Quantity = quantity,
                        Price = price,
                        Commissions = commission,
                        StampDuty = 0m,
                        TransferFees = transferFees,
                        FeesSettlement = 0m
                    };
                context.UnitOfWork.RegisterNew(tradingRecord);

                var holdingsRecordRepository
                    = context.GetRepository<HoldingsRecordRepository>();
                var holdingsRecord
                    = holdingsRecordRepository
                    .GetHoldingsRecordByAccountAndCode(accountData.Id, code);
                if(holdingsRecord == null)
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
                    holdingsRecord.Quantity += quantity;
                    context.UnitOfWork.RegisterModified(holdingsRecord);
                }

                context.UnitOfWork.Commit();
            }
            
            return false;
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
            throw new NotImplementedException();
        }

        public int AvailableQuantityToSell(string code)
        {
            throw new NotImplementedException();
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
    }
}
