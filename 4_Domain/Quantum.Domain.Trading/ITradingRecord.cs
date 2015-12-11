using Quantum.Infrastructure.Trading.Repository;

namespace Quantum.Domain.Trading
{
    public interface ITradingRecord : ITradingRecordData
    {
        /// <summary>
        /// 获取交易的总金额
        /// </summary>
        decimal Amount { get; }
    }
}
