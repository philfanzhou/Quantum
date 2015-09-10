
namespace Quantum.Infrastructure.MarketData.MMF
{
    public struct MyDataHeader : IMarketDataMmfHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }
    }

    public interface IMarketDataMmfHeader
    {
        /// <summary>
        /// 当前已有数据量
        /// </summary>
        int DataCount { get; set; }

        /// <summary>
        /// 最大可容纳的数据量
        /// </summary>
        int MaxDataCount { get; set; }
    }
}
