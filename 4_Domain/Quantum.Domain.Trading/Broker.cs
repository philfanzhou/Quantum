using Framework.Infrastructure.Repository;
using Quantum.Infrastructure.Trading.Repository;
using System;

namespace Quantum.Domain.Trading
{
    public static class Broker
    {
        public static AccountData CreateAccount(string name)
        {
            var account = new AccountData
            {
                Id = GetNewAccountId(),
                Name = name,
                Principal = 0,
                Balance = 0
            };

            using (var context = RepositoryContext.Create())
            {
                context.UnitOfWork.RegisterNew(account);
                context.UnitOfWork.Commit();
            }

            return account;
        }

        public static AccountData GetAccount(string id)
        {
            using (var context = RepositoryContext.Create())
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
