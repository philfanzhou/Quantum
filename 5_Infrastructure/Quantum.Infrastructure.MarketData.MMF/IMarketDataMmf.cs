using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public interface IMarketDataMmf<TDataItem, TDataHeader> : IDisposable
        where TDataItem : struct
        where TDataHeader : struct, IMarketDataMmfHeader
    {
        IMarketDataMmfHeader Header { get; }
    }
}
