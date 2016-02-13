using Ore.Infrastructure.MarketData;
using System;

namespace Quantum.Domain.TimeSeries
{
    /// <summary>
    /// 以1年作为时间片的数据包裹集合
    /// </summary>
    public class Year1Collections<T> : PackageCollections<T>
        where T : ITimeSeries
    {
        protected override ITimeZone GetTimeZone(DateTime currentTime)
        {
            DateTime startTime = new DateTime(currentTime.Year, 1, 1);
            DateTime endTime = startTime.AddYears(1);

            return new TimeZone(startTime, endTime);
        }
    }
}
