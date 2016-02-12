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
        protected override void GetTimeZone(DateTime currentTime, out DateTime startTime, out DateTime endTime)
        {
            startTime = currentTime.Date
                .AddHours(currentTime.Hour)
                .AddMinutes(currentTime.Minute);

            endTime = startTime.AddMinutes(1);
        }
    }
}
