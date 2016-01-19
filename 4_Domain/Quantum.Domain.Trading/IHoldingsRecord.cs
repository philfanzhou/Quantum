using System;

namespace Quantum.Domain.Trading
{
    public interface IHoldingsRecord
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        string StockCode { get; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        int Quantity { get; }

        /// <summary>
        /// 成本
        /// </summary>
        double Cost { get; }

        /// <summary>
        /// 计算可卖数量
        /// </summary>
        /// <param name="time"></param>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        int AvailableQuantityToSell(DateTime time);

        /// <summary>
        /// 浮动盈亏 - 与实时行情相关
        /// </summary>
        double FloatingProfitAndLoss { get; }

        /// <summary>
        /// 盈亏比例 - 与实时行情相关
        /// </summary>
        float Proportion { get; }

        /// <summary>
        /// 市值 - 与实时行情相关
        /// </summary>
        decimal MarketValue { get; }
    }
}
