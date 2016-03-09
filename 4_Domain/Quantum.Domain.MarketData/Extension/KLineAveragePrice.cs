using Ore.Infrastructure.MarketData;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.MarketData
{
    public static class KLineAveragePrice
    {
        public static IAveragePrice AveragePrice(this IStockKLine self)
        {
            return new AveragePrice
            {
                Amount = self.Amount,
                Price = self.Close,
                Time = self.Time,
                Volume = self.Volume
            };
        }

        public static IEnumerable<IAveragePrice> AveragePrice(this IEnumerable<IStockKLine> self)
        {
            return self.Select(p => p.AveragePrice());
        }

        public static IEnumerable<IAveragePrice> AveragePrice(this IEnumerable<IStockKLine> self, IEnumerable<IAveragePrice> current)
        {
            List<IAveragePrice> currentList = current.OrderBy(p => p.Time).ToList();
            var latestAveragePrice = currentList.Last();
            var needToHandle = self.Where(p => p.Time > latestAveragePrice.Time);

            currentList.AddRange(needToHandle.Select(p => p.AveragePrice()));
            return currentList;
        }
    }
}
