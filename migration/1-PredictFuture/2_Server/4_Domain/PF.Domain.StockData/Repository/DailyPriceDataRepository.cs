namespace PF.Domain.StockData
{
    using Core.Domain;
    using PF.Domain.StockData.Entities;
    using System;
    using System.Collections.Generic;

    public class DailyPriceDataRepository : Repository<DailyPriceDataItem>
    {
        public DailyPriceDataRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public IEnumerable<DailyPriceDataItem> GetDaylineData(
            string symbol,
            DateTime startDay,
            DateTime endDay)
        {
            var spec = Specification<DailyPriceDataItem>.Eval(t =>
                t.Stock.Symbol.Equals(symbol) &&
                t.Date >= startDay &&
                t.Date <= endDay);

            return this.GetAll(spec);
        }
    }
}
