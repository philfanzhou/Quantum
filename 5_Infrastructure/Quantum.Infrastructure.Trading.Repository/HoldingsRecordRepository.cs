using Framework.Infrastructure.Repository;
using System.Collections.Generic;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class HoldingsRecordRepository : Repository<HoldingsRecordData>
    {
        public HoldingsRecordRepository(IRepositoryContext context)
            : base(context)
        {
        }

        public IEnumerable<HoldingsRecordData> GetHoldingsRecordByAccount(string accountId)
        {
            var accountSpec = Specification<HoldingsRecordData>
                .Eval(data => data.AccountId == accountId);

            return this.GetAll(accountSpec);
        }

        public bool ExistHoldingsRecord(string accountId, string code)
        {
            var accountSpec = Specification<HoldingsRecordData>
                .Eval(data => data.AccountId == accountId);
            var stockSpec = Specification<HoldingsRecordData>
                .Eval(dta => dta.StockCode == code);
            var condition = new AndSpecification<HoldingsRecordData>(accountSpec, stockSpec);

            return this.Exists(condition);
        }

        public HoldingsRecordData GetHoldingsRecordByAccountAndCode(string accountId, string code)
        {
            var accountSpec = Specification<HoldingsRecordData>
                .Eval(data => data.AccountId == accountId);
            var stockSpec = Specification<HoldingsRecordData>
                .Eval(dta => dta.StockCode == code);
            var condition = new AndSpecification<HoldingsRecordData>(accountSpec, stockSpec);

            return this.Single(condition);
        }
    }
}
