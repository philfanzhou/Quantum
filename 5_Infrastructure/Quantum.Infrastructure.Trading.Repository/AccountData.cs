using Framework.Infrastructure.Repository;
using System;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class AccountData
    {
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 本金
        /// </summary>
        public Decimal Principal { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public Decimal Balance { get; set; }
    }
}
