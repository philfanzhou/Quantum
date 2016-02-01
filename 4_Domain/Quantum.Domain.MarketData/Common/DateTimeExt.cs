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
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    _ret = true;
                    break;
                case DayOfWeek.Saturday:
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
            TimeSpan time = self.TimeOfDay;

            if (time < new TimeSpan(9, 30, 0))
            {
                return false;
            }
            else if (time > new TimeSpan(11, 30, 0)
                && time < new TimeSpan(13, 0, 0))
            {
                return false;
            }
            else if(time > new TimeSpan(15, 0, 0))
            {
                return false;
            }

            return true;
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
        /// 根据交易类型，将时间向后推进到下一个交易时间单位
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateTime Increase(this DateTime self, KLineType type)
        {

        }
    }
}
