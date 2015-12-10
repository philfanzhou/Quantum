using Quantum.Infrastructure.Trading.Repository;
using System;

namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 券商
    /// </summary>
    public static class Broker
    {
        public static IAccount CreateAccount(string name)
        {
            return new Account(name);
        }

        public static IAccount GetAccount(string id)
        {
            throw new NotImplementedException()；
        }
    }
}
