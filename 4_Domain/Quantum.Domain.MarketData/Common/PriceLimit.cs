using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 涨停板和跌停板计算
    /// </summary>
    public static class PriceLimit
    {
        /// <summary>
        /// 计算股票涨停板价格
        /// </summary>
        /// <param name="preClose">前收</param>
        /// <returns></returns>
        public static double StockUpLimit(double preClose)
        {
            return GetLimitPrice(preClose, true, 2);
        }

        /// <summary>
        /// 计算基金涨停板价格
        /// </summary>
        /// <param name="preClose">前收</param>
        /// <returns></returns>
        public static double FundUpLimit(double preClose)
        {
            return GetLimitPrice(preClose, true, 3);
        }

        /// <summary>
        /// 计算股票跌停板价格
        /// </summary>
        /// <param name="preClose">前收</param>
        /// <returns></returns>
        public static double StockDownLimit(double preClose)
        {
            return GetLimitPrice(preClose, false, 2);
        }

        /// <summary>
        /// 计算基金跌停板价格
        /// </summary>
        /// <param name="preClose">前收</param>
        /// <returns></returns>
        public static double FundDownLimit(double preClose)
        {
            return GetLimitPrice(preClose, false, 3);
        }

        private static double GetLimitPrice(double preClose, bool isUp, int digits)
        {
            if(digits != 2 && digits != 3)
            {
                throw new ArgumentOutOfRangeException("digits");
            }

            decimal preClosePrice = (decimal)preClose;

            // 考虑10% 以及上下浮动一分钱的情况
            decimal price1;
            decimal price2;
            decimal price3;
            if (isUp)
            {
                // 计算涨停板
                price1 = preClosePrice + (decimal)Math.Round(preClose * 0.1, digits, MidpointRounding.AwayFromZero);
            }
            else
            {
                // 计算跌停板
                price1 = preClosePrice - (decimal)Math.Round(preClose * 0.1, digits, MidpointRounding.AwayFromZero);
            }
            if (digits == 2)
            {
                price2 = price1 + 0.01m;
                price3 = price1 - 0.01m;
            }
            else
            {
                // 基金的精度是0.001
                price2 = price1 + 0.001m;
                price3 = price1 - 0.001m;
            }

            // 计算每种情况的涨幅
            decimal percent1 = (price1 - preClosePrice) / preClosePrice * 100;
            decimal percent2 = (price2 - preClosePrice) / preClosePrice * 100;
            decimal percent3 = (price3 - preClosePrice) / preClosePrice * 100;

            // 计算每种情况离10%的距离
            decimal distance1 = Math.Abs(Math.Abs(percent1) - 10);
            decimal distance2 = Math.Abs(Math.Abs(percent2) - 10);
            decimal distance3 = Math.Abs(Math.Abs(percent3) - 10);

            // 返回最接近10%的价格
            if (distance1 < distance2 && distance1 < distance3)
            {
                return Convert.ToDouble(price1);
            }
            if (distance2 < distance1 && distance2 < distance3)
            {
                return Convert.ToDouble(price2);
            }
            else
            {
                return Convert.ToDouble(price3);
            }
        }
    }
}
