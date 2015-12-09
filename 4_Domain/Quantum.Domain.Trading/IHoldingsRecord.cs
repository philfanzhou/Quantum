using Quantum.Domain.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Trading
{
    public interface IHoldingsRecord
    {
        string StockCode { get; }

        string StockName { get; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        int Quantity { get; }

        /// <summary>
        /// 可卖数量
        /// </summary>
        int QuantityForSell { get; }

        /// <summary>
        /// 成本
        /// </summary>
        double Cost { get; }

        /// <summary>
        /// 浮动盈亏
        /// </summary>
        double FloatingProfitAndLoss { get; }

        /// <summary>
        /// 盈亏比例
        /// </summary>
        float Proportion { get; }

        /// <summary>
        /// 市值
        /// </summary>
        decimal MarketValue { get; }

        /// <summary>
        /// 当前价
        /// </summary>
        double Price { get; }
    }
}
