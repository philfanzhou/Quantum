
namespace Quantum.Domain.Indicator
{
    /// <summary>
    /// 多空指标
    /// </summary>
    public interface IBuyAgainstSell
    {
        double SellVolume { get; }

        double BuyVolume { get; }
    }
}
