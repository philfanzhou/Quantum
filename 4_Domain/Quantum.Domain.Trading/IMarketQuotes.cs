namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 市场报价定义接口
    /// </summary>
    public interface IMarketQuotes
    {
        double GetPrice(string stockCode);
    }
}
