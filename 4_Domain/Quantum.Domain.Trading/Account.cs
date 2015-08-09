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
        private string accountId;

        public Account(string Id)
        {
            this.accountId = Id;
        }

        public bool Buy(string code, double price, int quantity)
        {
            throw new NotImplementedException();
        }

        public int AvailableQuantityToBuy(string code, double price)
        {
            throw new NotImplementedException();
        }

        public bool Sell(string code, double price, int quantity)
        {
            throw new NotImplementedException();
        }

        public int AvailableQuantityToSell(string code)
        {
            throw new NotImplementedException();
        }

        public bool TransferIn(double amount)
        {
            throw new NotImplementedException();
        }

        public bool TransferOut(double amount)
        {
            throw new NotImplementedException();
        }
    }
}
