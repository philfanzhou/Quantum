namespace Quantum.Infrastructure.EF.Trading.Config
{
    public class AccountDbo
    {
        /// <summary>
        /// 账户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 户名
        /// </summary>
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
