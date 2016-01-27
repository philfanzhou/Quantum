namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 市场信息定义
    /// </summary>
    public class Market
    {
        /// <summary>
        /// 一手股票包含100股
        /// </summary>
        public const int OneHandStock = 100;

        /// <summary>
        /// 获取或设置用于查询股票实时报价接口的实例
        /// </summary>
        public static IMarketQuotes Quotes { get; set; }
    }
}
