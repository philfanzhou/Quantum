
namespace Quantum.Infrastructure.MarketData.MMF
{
    public struct MarketDataHeader : IMarketDataHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }
    }

}
