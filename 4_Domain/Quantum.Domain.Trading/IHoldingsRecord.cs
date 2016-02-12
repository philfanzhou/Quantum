using System;
using System.Collections.Generic;

namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 持仓记录
    /// </summary>
    public interface IHoldingsRecord
    {
        #region Property
        /// <summary>
        /// 股票代码
        /// </summary>
        string StockCode { get; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        int Quantity { get; }

        /// <summary>
        /// 获取所有交易记录
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITradingRecord> TradingRecords { get; }
        #endregion

        #region Method
        /// <summary>
        /// 成本
        /// </summary>
        double GetCost();

        /// <summary>
        /// 浮动盈亏 - 与实时行情相关
        /// </summary>
        decimal GetFloatingProfitAndLoss();

        /// <summary>
        /// 盈亏比例 - 与实时行情相关
        /// </summary>
        float GetProportion();

        /// <summary>
        /// 市值 - 与实时行情相关
        /// </summary>
        decimal GetMarketValue();

        /// <summary>
        /// 计算冻结数量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        int GetFrozenQuantity(DateTime time);

        /// <summary>
        /// 计算可卖数量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        int GetAvailableQuantity(DateTime time);
        #endregion
    }
}
