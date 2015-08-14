using Framework.Infrastructure.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class HoldingsRecordRepository : Repository<HoldingsRecordData>
    {
        public HoldingsRecordRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public IEnumerable<HoldingsRecordData> GetByAccount(string accountId)
        {
            return this.GetAll().Where(p => p.AccountId == accountId);
        }

        public HoldingsRecordData GetByAccountAndCode(string accountId, string code)
        {
            return this.GetByAccount(accountId).SingleOrDefault(p => p.StockCode == code);
        }
    }
}
