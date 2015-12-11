namespace Quantum.Infrastructure.Trading.Repository
{
    public interface IAccountData
    {
        /// <summary>
        /// 账户ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 户名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 本金
        /// </summary>
        decimal Principal { get; }

        /// <summary>
        /// 余额
        /// </summary>
        decimal Balance { get; }
    }
}
