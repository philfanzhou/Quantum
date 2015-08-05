namespace PF.Domain.Indicator
{
    using PF.Domain.StockData;

    public interface ITechnicalIndicator : IValueIndicator
    {
        /// <summary>
        /// K线周期
        /// </summary>
        KLineCycle Cycle { get; set; }
        /// <summary>
        /// 引用偏移量
        /// </summary>
        /// <example>X天之前的股价</example>
        int ReferOffset { get; set; }
    }
}
