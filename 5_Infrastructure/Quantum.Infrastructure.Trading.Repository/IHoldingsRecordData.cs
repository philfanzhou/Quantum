using System;

namespace Quantum.Infrastructure.Trading.Repository
{
    public interface IHoldingsRecordData
    {
        /// <summary>
        /// 账户
        /// </summary>
        string AccountId { get; }

        /// <summary>
        /// 股票代码
        /// </summary>
        string StockCode { get; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        int Quantity { get; }

        /// <summary>
        /// 当前价
        /// </summary>
        double Price { get; }
    }
}
