using Ore.Infrastructure.MarketData;
using System;

namespace Quantum.Domain.MarketData
{
    public class Min1Packages<T> : PackageCollections<T>
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
