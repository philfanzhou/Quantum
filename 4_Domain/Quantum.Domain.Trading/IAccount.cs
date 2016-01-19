﻿using System;

namespace Quantum.Domain.Trading
{
    public interface IAccount
    {
        /// <summary>
        /// 账户ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 户名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 本金
        /// </summary>
        decimal Principal { get; }

        /// <summary>
        /// 余额
        /// </summary>
        decimal Balance { get; }

        /// <summary>
        /// 总资产
        /// </summary>
        decimal TotalAssets { get; }

        /// <summary>
        /// 持仓市值
        /// </summary>
        decimal MarketValue { get; }

        ///// <summary>
        ///// 冻结资金
        ///// </summary>
        //decimal FrozenFund { get; }

        ///// <summary>
        ///// 可用资金
        ///// </summary>
        //decimal AvailableFund { get; }

        /// <summary>
        /// 银行转券商
        /// </summary>
        /// <param name="amount"></param>
        void TransferIn(decimal amount);

        /// <summary>
        /// 券商转银行
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        bool TransferOut(decimal amount);

        /// <summary>
        /// 买入
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        bool Buy(DateTime time, string stockCode, double price, int quantity);

        /// <summary>
        /// 卖出
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        bool Sell(DateTime time, string stockCode, double price, int quantity);

        /// <summary>
        /// 计算可买数量
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        int AvailableQuantityToBuy(string stockCode, double price);
    }
}
