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
        protected override void GetTimeZone(DateTime currentTime, out DateTime startTime, out DateTime endTime)
        {
            startTime = new DateTime(currentTime.Year, 0, 0);
            endTime = startTime.AddYears(1);
        }
    }
}
