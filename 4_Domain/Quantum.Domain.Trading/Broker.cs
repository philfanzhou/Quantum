using Framework.Infrastructure.Repository;
using Quantum.Infrastructure.Trading.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Trading
{
    public class Broker
    {
        public AccountData CreateAccount(string name)
        {
            AccountData account = new AccountData();
            account.Id = GetNewAccountId();
            account.Name = name;

            using (IRepositoryContext context = RepositoryContext.Create())
            {
                context.UnitOfWork.RegisterNew(account);
                context.UnitOfWork.Commit();
            }

            return account;
        }

        private string GetNewAccountId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
