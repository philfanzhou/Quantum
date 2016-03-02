using Ore.Infrastructure.MarketData;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 移动平均线
    /// 计算方法：N日移动平均线 = N日收市价之和 / N
    /// </summary>
    public interface IMA : ITimeSeries
    {
        /// <summary>
        /// 均线周期 如：5，10，20，60
        /// </summary>
        int Cycle { get; }

        /// <summary>
        /// 值
        /// </summary>
        double Value { get; }
    }
}
