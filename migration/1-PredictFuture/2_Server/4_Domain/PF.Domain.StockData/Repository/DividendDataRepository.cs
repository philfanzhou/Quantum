namespace PF.Domain.StockData
{
    using Core.Domain;
    using PF.Domain.StockData.Entities;
    using System;
    using System.Collections.Generic;

    public class DividendDataRepository : Repository<DividendDataItem>
    {
        public DividendDataRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public IEnumerable<DividendDataItem> GetDividendData(
            string symbol, 
            DateTime startDay, 
            DateTime endDay)
        {
            var spec = Specification<DividendDataItem>.Eval(t =>
                t.Stock.Symbol.Equals(symbol) &&
                t.Date >= startDay &&
                t.Date <= endDay);

            return this.GetAll(spec);
        }
    }
}
