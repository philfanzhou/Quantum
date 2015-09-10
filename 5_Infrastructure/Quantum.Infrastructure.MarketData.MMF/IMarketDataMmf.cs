using System;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public interface IMarketDataMmf<TDataItem, TDataHeader> : IDisposable
        where TDataItem : struct
        where TDataHeader : struct, IMarketDataHeader
    {
        IMarketDataHeader Header { get; }
    }
}
