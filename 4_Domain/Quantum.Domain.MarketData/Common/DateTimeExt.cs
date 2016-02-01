using Ore.Infrastructure.MarketData;
using System;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 根据股票交易业务逻辑对日期类型的扩展
    /// </summary>
    public static class DateTimeExt
    {
        /// <summary>
        /// 判断当前日期是不是交易日
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsTradingDate(this DateTime self)
        {
            bool _ret = false;

            switch (self.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    _ret = true;
                    break;
                case DayOfWeek.Tuesday:
                    _ret = true;
                    break;
                case DayOfWeek.Wednesday:
                    _ret = true;
                    break;
                case DayOfWeek.Thursday:
                    _ret = true;
                    break;
                case DayOfWeek.Friday:
                    _ret = true;
                    break;
                case DayOfWeek.Saturday:
                    _ret = false;
                    break;
                case DayOfWeek.Sunday:
                    _ret = false;
                    break;
            }

            //if(_ret)
            //{
            //    // todo: 还可以加入放假日期的判断
            //}

            return _ret;
        }

        /// <summary>
        /// 判断当前时间是不是交易时间
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsTradingTime(this DateTime self)
        {

        }

        /// <summary>
        /// 获取当前日期之后的最近的一个交易日
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static DateTime GetNearestTradingDate(this DateTime self)
        {
            if(self.IsTradingDate())
            {
                return self;
            }
            else
            {
                return self.AddDays(1).GetNearestTradingDate();
            }
        }

        /// <summary>
        /// 根据交易类型，将时间向后推进
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateTime Increase(this DateTime self, KLineType type)
        {

        }
    }
}
