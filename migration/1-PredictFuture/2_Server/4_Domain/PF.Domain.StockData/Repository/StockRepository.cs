using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PF.Domain.StockData.Entities;
using Core.Domain;

namespace PF.Domain.StockData.Repository
{
    public class StockRepository : Repository<Stock>
    {
        public StockRepository(IRepositoryContext context) : base(context)
        {
        }
    }
}
