
using System.Linq;
using Core.Domain;

namespace PF.Domain.Indicator
{
    using PF.Domain.StockData;
    using PF.Domain.StockData.Entities;
    using System;

    internal class IndicatorDataService
    {
        public PriceDataItem GetPriceDate(
            string stockId,
            KLineCycle cycle,
            int referOffset,
            DateTime endDate)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DailyPriceDataRepository>();
                return repository.GetDaylineData(stockId, endDate, endDate).FirstOrDefault();
            }
        }
    }
}
