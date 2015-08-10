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

        public void Buy(string code, double price, int quantity)
        {
            throw new NotImplementedException();
        }

        public int AvailableQuantityToBuy(double price)
        {
            decimal balance;
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<Repository<AccountData>>();
                AccountData accountData = repository.Get(this.accountId);
                balance = accountData.Balance;
            }

            decimal number = balance / (decimal)price;

            if (number < NumberOfRoundBlocks)
            {
                return 0;
            }
            else
            {
                int roundBlocks = Convert.ToInt32(Math.Floor(number / NumberOfRoundBlocks));
                return roundBlocks * NumberOfRoundBlocks;
            }
        }

        public void Sell(string code, double price, int quantity)
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
    }
}
