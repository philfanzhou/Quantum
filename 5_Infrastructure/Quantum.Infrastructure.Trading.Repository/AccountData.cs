namespace Quantum.Infrastructure.Trading.Repository
{
    public class AccountData
    {
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 本金
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
    }
}
