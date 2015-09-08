using Quantum.Infrastructure.MarketData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.MarketData
{
    public static class RealTimeDataExt
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

        /// <summary>
        /// 委差
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double WeiCha(this RealTimeData self)
        {
            return self.BuyVolume() - self.SellVolume();
        }

        /// <summary>
        /// 委比
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double WeiBi(this RealTimeData self)
        {
            return Math.Round(self.WeiCha() / (self.BuyVolume() + self.SellVolume()) * 100, 2);
        }
    }
}
