﻿using Ore.Infrastructure.MarketData;
using System;

namespace Quantum.Domain.TimeSeries
{
    /// <summary>
    /// 以5分钟作为时间片的数据包裹集合
    /// </summary>
    public class Min5Collections<T> : PackageCollections<T>
        where T : ITimeSeries
    {
        protected override ITimeZone GetTimeZone(DateTime currentTime)
        {
            // 取得分钟的10位数
            int tenDigit = FindNum(currentTime.Minute, 2);
            // 以分钟10位数构造一个临时时间
            DateTime tmpTime = currentTime.Date
                .AddHours(currentTime.Hour)
                .AddMinutes(tenDigit * 10);

            // 当前时间在5分之后，起始时间为5分
            DateTime startTime = tmpTime;
            if (currentTime - tmpTime > new TimeSpan(0, 5, 0))
            {
                startTime = startTime.AddMinutes(5);
            }

            // 当前时间在5分之内，起始时间为10分整数
            DateTime endTime = startTime.AddMinutes(5);

            return new TimeZone(startTime, endTime);
        }
    }
}
