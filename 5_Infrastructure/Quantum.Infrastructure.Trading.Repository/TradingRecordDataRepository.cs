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

        public IEnumerable<TradingRecordData> GetTradingRecordByAccount(string accountId)
        {
            var accountSpec = Specification<TradingRecordData>
                .Eval(data => data.AccountId == accountId);

            return this.GetAll(accountSpec);
        }

        public IEnumerable<TradingRecordData> GetTradingRecordByAccountAndCode(string accountId, string code)
        {
            var accountSpec = Specification<TradingRecordData>
                .Eval(data => data.AccountId == accountId);
            var stockSpec = Specification<TradingRecordData>
                .Eval(dta => dta.StockCode == code);
            var condition = new AndSpecification<TradingRecordData>(accountSpec, stockSpec);

            return this.GetAll(condition);
        }
    }
}
