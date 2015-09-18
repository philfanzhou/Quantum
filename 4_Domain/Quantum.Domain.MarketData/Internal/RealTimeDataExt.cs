using Quantum.Infrastructure.MarketData.Repository;

namespace Quantum.Domain.MarketData
{
    internal static class RealTimeItemExt
    {
        /// <summary>
        /// 委买
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double BuyVolume(this RealTimeItem self)
        {
            return self.BuyOneVolume +
                self.BuyTwoVolume +
                self.BuyThreeVolume +
                self.BuyFourVolume +
                self.BuyFiveVolume;
        }

        /// <summary>
        /// 委卖
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double SellVolume(this RealTimeItem self)
        {
            return self.SellOneVolume +
                self.SellTwoVolume +
                self.SellThreeVolume +
                self.SellFourVolume +
                self.SellFiveVolume;
        }
    }
}
