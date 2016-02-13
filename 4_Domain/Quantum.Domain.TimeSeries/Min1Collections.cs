using Ore.Infrastructure.MarketData;
using System;

namespace Quantum.Domain.TimeSeries
{
    /// <summary>
    /// 以1分钟作为时间片的数据包裹集合
    /// </summary>
    public class Min1Collections<T> : PackageCollections<T>
        where T : ITimeSeries
    {
        protected override ITimeZone GetTimeZone(DateTime currentTime)
        {
            DateTime startTime = currentTime.Date
                .AddHours(currentTime.Hour)
                .AddMinutes(currentTime.Minute);

            DateTime endTime = startTime.AddMinutes(1);

            return new TimeZone(startTime, endTime);
        }
    }
}
