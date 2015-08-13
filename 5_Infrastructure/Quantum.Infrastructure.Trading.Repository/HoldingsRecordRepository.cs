using Framework.Infrastructure.Repository;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class HoldingsRecordRepository : Repository<HoldingsRecordData>
    {
        public HoldingsRecordRepository(IRepositoryContext context)
            : base(context)
        {
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
