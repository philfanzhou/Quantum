using Framework.Infrastructure.Repository;
using Pitman.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.Trading.MarketData
{
    public class MarketDataRepository : Repository<RealTimeData>
    {
        public MarketDataRepository(IRepositoryContext context)
            : base(context)
        {
        }
    }
}
