using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Application.Trading
{
    public interface IAccount
    {
        string Id { get; }

        string Name { get; }

        /// <summary>
        /// 总资产
        /// </summary>
        Decimal TotalAssets { get; }

        /// <summary>
        /// 市值
        /// </summary>
        Decimal MarketValue { get; }

        /// <summary>
        /// 冻结资金
        /// </summary>
        Decimal FrozenFund { get; }

        /// <summary>
        /// 余额
        /// </summary>
        Decimal Balance { get; }

        /// <summary>
        /// 可用资金
        /// </summary>
        Decimal AvailableFund { get; }
    }
}
