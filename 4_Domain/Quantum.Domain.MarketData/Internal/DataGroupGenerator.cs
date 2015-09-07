using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.MarketData
{
    internal static class DataGroupGenerator
    {
        public static IEnumerable<IEnumerable<IMarketData>> Group(TimeFrame timeFrame, IEnumerable<IMarketData> data)
        {
            throw new NotImplementedException();
        }
    }
}
