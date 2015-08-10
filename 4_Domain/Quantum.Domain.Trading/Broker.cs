using Framework.Infrastructure.Repository;
using Quantum.Infrastructure.Trading.Repository;
using System;

namespace Quantum.Domain.Trading
{
    public static class Broker
    {
        public static AccountData CreateAccount(string name)
        {
            AccountData account = new AccountData();
            account.Id = GetNewAccountId();
            account.Name = name;
            account.Principal = 0;
            account.Balance = 0;

            using (IRepositoryContext context = RepositoryContext.Create())
            {
                context.UnitOfWork.RegisterNew(account);
                context.UnitOfWork.Commit();
            }

            return account;
        }

        public static AccountData GetAccount(string id)
        {
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<Repository<AccountData>>();
                return repository.Get(id);
            }
        }

        private static string GetNewAccountId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
