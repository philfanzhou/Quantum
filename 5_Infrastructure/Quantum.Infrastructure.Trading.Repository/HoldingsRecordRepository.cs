using Framework.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class HoldingsRecordRepository : Repository<HoldingsRecordData>
    {
        public HoldingsRecordData GetHoldingsRecordByAccountAndCode(string accountId, string code)
        {

        }
    }
}
