using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public interface IMarketDataMmf
    {
        IMarketDataMmfHeader Header { get; }
    }
}
