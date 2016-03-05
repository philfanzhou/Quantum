using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Decision
{
    public interface ITradingAction
    {
        ActionType Type { get; }

        int Quantity { get; }
    }

    /// <summary>
    /// 交易类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 买入
        /// </summary>
        Buy,
        /// <summary>
        /// 卖出
        /// </summary>
        Sell
    }
}
