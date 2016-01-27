using System;

namespace Quantum.Domain.Indicator
{
    public static class BuyAgainstSellExt
    {
        /// <summary>
        /// 委差
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double WeiCha(this IBuyAgainstSell self)
        {
            return self.BuyVolume - self.SellVolume;
        }

        /// <summary>
        /// 委比
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double WeiBi(this IBuyAgainstSell self)
        {
            return Math.Round(self.WeiCha() / (self.BuyVolume + self.SellVolume) * 100, 2);
        }

        /// <summary>
        /// 多空比
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double DuoKongBi(this IBuyAgainstSell self)
        {
            return Math.Round(self.BuyVolume/self.SellVolume, 2);
        }
    }
}
