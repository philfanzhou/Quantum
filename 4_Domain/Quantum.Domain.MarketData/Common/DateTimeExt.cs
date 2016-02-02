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
            // 上交所90年12越19日开业
            if(self < new DateTime(1990, 12, 19))
            {
                return false;
            }

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
            if(self.IsTradingDate() == false)
            {
                return false;
            }

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
        /// 获取下一个交易日
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static DateTime ToNextTradingDate(this DateTime self)
        {
            if (self < new DateTime(1990, 12, 19))
            {
                return new DateTime(1990, 12, 19);
            }

            DateTime date = self.Date.AddDays(1);
            if(date.IsTradingDate())
            {
                return date;
            }
            else
            {
                return date.ToNextTradingDate();
            }
        }

        /// <summary>
        /// 获取下一个交易分钟
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static DateTime ToNextTradingMinute(this DateTime self)
        {
            if (self.IsTradingDate() == false)
            {
                self = self.ToNextTradingDate();
                return self.AddHours(9).AddMinutes(31);
            }

            DateTime time = self.AddMinutes(1);
            TimeSpan span = time.TimeOfDay;
            if (span < new TimeSpan(9, 31, 0))
            {
                return time.Date.AddHours(9).AddMinutes(31);
            }
            else if (span > new TimeSpan(11, 30, 0)
                && span < new TimeSpan(13, 0, 0))
            {
                return time.Date.AddHours(13).AddMinutes(1);
            }
            else if (span > new TimeSpan(15, 0, 0))
            {
                return time.Date.AddDays(1).ToNextTradingMinute();
            }

            return time;
        }
    }
}
