using Ore.Infrastructure.MarketData;

namespace Quantum.Domain.MarketData
{
    internal static class RealTimeItemExt
    {
        /// <summary>
        /// 委买
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double BuyVolume(this IStockRealTime self)
        {
            return self.Buy1Volume +
                self.Buy2Volume +
                self.Buy3Volume +
                self.Buy4Volume +
                self.Buy5Volume;
        }

        /// <summary>
        /// 委卖
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double SellVolume(this IStockRealTime self)
        {
            return self.Sell1Volume +
                self.Sell2Volume +
                self.Sell3Volume +
                self.Sell4Volume +
                self.Sell5Volume;
        }
    }
}
