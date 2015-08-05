namespace PF.Domain.StockData
{
    using System;

    /// <summary>
    /// K线周期
    /// </summary>
    [Serializable]
    public enum KLineCycle
    {
        /// <summary>
        /// 日线
        /// </summary>
        Daily,
        /// <summary>
        /// 周线
        /// </summary>
        Weekly,
        /// <summary>
        /// 月线
        /// </summary>
        Monthly,
        /// <summary>
        /// 年线
        /// </summary>
        Yearly
    }
}