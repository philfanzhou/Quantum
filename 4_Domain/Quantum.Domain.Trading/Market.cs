namespace Quantum.Domain.Trading
{
    public class Market
    {
        /// <summary>
        /// 一手股票包含100股
        /// </summary>
        public const int OneHandStock = 100;

        /// <summary>
        /// 市场报价接口实例
        /// </summary>
        public static IMarketQuotes Quotes { get; set; }
    }
}
