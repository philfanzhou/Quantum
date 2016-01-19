namespace Quantum.Infrastructure.EF.Trading.Config
{
    public class HoldingsRecordDbo
    {
        /// <summary>
        /// 账户
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
