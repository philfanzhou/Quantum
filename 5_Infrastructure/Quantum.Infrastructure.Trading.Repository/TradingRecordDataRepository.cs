using Framework.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class TradingRecordDataRepository : Repository<TradingRecordData>
    {
        public TradingRecordDataRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public IEnumerable<TradingRecordData> GetByAccount(string accountId)
        {
            return this.GetAll().Where(p => p.AccountId == accountId);
        }

        public IEnumerable<TradingRecordData> GetBuyRecord(string accountId, string code, DateTime now)
        {
            return this.GetByAccountAndCode(accountId, code)
                .Where(p => 
                    p.Date.Day == now.Day &&
                    p.Type == TradeType.Buy);
        }

        public IEnumerable<TradingRecordData> GetByAccountAndCode(string accountId, string code)
        {
            return this.GetByAccount(accountId)
                .Where(p => p.StockCode == code);
        }
    }
}
