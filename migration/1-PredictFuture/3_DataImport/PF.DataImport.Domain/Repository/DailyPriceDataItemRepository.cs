using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DataImport.Domain
{
    using Core.Domain;

    public class DailyPriceDataItemRepository : Repository<DailyPriceDataItem>
    {
        public DailyPriceDataItemRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public DateTime GetLatestDailyPriceDataImportDateTime(string symbol)
        {
            var spec = Specification<DailyPriceDataItem>.Eval(dailyPriceDataItem =>
                dailyPriceDataItem.Stock.Symbol.Equals(symbol));
            var item = this.GetAll(spec).OrderByDescending(i => i.Date).FirstOrDefault();
            return item == null ? DateTime.MinValue : item.Date;
        }
    }
}
