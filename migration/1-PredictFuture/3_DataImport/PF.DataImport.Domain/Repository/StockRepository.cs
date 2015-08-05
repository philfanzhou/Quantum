using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DataImport.Domain
{
    using Core.Domain;

    public class StockRepository : Repository<Stock>
    {
        public StockRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public Stock GetStockBySymbol(string smybol)
        {
            var spec = Specification<Stock>.Eval(stock =>
                stock.Symbol.Equals(smybol));
            return this.Single(spec);
        }
    }
}
