using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;

namespace Quantum.Domain.MarketData
{
    public static class RealTimeConverter
    {
        public static IEnumerable<IStockKLine> ConvertTo1Minute(IEnumerable<IStockRealTime> realTimeItems)
        {
            List<IStockKLine> result = new List<IStockKLine>();
            Dictionary<DateTime, KLine1MinuteInfo> dicKLine1MinuteInfo = new Dictionary<DateTime, KLine1MinuteInfo>();

            foreach (var realTimeItem in realTimeItems)
            {
                DateTime date = realTimeItem.Time.Date;
                if (!dicKLine1MinuteInfo.ContainsKey(date))
                {
                    dicKLine1MinuteInfo.Add(date, new KLine1MinuteInfo(date));
                }

                dicKLine1MinuteInfo[date].Add(realTimeItem);
            }

            foreach (var infoItem in dicKLine1MinuteInfo.Values)
            {
                result.AddRange(infoItem.Items);
            }

            return result;
        }
    }
}
