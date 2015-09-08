using Quantum.Infrastructure.MarketData.Repository;

namespace Quantum.Domain.MarketData
{
    internal static class RealTimeDataExt
    {
        /// <summary>
        /// 委买
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double BuyVolume(this RealTimeData self)
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
        public static double SellVolume(this RealTimeData self)
        {
            return self.SellOneVolume +
                self.SellTwoVolume +
                self.SellThreeVolume +
                self.SellFourVolume +
                self.SellFiveVolume;
        }
    }
}
