using Framework.Infrastructure.Repository;
using System;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class AccountData : Entity
    {
        public string Name { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public Decimal Balance { get; set; }
    }
}
