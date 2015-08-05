using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DataImport.Domain
{
    using Core.Domain;

    public class DividendDataItemRepository : Repository<DividendDataItem>
    {
        public DividendDataItemRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public DateTime GetLatestDividendDataImportDateTime(string symbol)
        {
            var spec = Specification<DividendDataItem>.Eval(dividendDataItem =>
                dividendDataItem.Stock.Symbol.Equals(symbol));
            var item = this.GetAll(spec).OrderByDescending(i => i.Date).FirstOrDefault();
            return item == null ? DateTime.MinValue : item.Date;
        }
    }
}
