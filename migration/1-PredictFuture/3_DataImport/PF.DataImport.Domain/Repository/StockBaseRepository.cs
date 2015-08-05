using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DataImport.Domain
{
    using Core.Domain;

    public class StockBaseRepository : Repository<StockBase>
    {
        public StockBaseRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public StockBase GetStockBaseBySymbol(string smybol)
        {
            var spec = Specification<StockBase>.Eval(stockBase =>
                stockBase.Symbol.Equals(smybol));
            return this.Single(spec);
        }
    }
}
