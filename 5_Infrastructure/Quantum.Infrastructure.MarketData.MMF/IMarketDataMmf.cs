using System;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public interface IMarketDataMmf : IDisposable
    {
        IMarketDataHeader Header { get; }
    }
}
