using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Application.Simulation
{
    internal static class DataProvider
    {
        public static IEnumerable<IStockRealTime> GetRealTimeData(string stockCode, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
