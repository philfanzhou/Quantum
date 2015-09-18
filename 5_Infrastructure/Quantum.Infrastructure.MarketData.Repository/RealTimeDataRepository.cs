using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.MarketData.Repository
{
    public class RealTimeDataRepository
    {
        public void Add(MarketType marketType, string stockCode, RealTimeItem data)
        {
            using (var file = RealTimeFile.CreateOrOpen(marketType, stockCode, data.Time))
            {
                file.Add(data);
            }
        }

        public IEnumerable<RealTimeItem> GetOneDayData(MarketType marketType, string stockCode, DateTime day)
        {
            using (var file = RealTimeFile.Open(marketType, stockCode, day))
            {
                return file.ReadAll();
            }
        }
    }
}
