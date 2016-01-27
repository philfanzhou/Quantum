namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 市场报价定义接口
    /// </summary>
    public interface IMarketQuotes
    {
        /// <summary>
        /// 获取指定股票价格
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        double GetPrice(string stockCode);
    }
}
