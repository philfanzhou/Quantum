//using Ore.Infrastructure.MarketData;
//using System;
//using System.Collections.Generic;

//namespace Quantum.Domain.MarketData
//{
//    public static class IntradayConverter
//    {
//        public static IEnumerable<IStockIntraday> ConvertToIntraday(IEnumerable<IStockRealTime> realTimeItems)
//        {
//            List<IStockIntraday> result = new List<IStockIntraday>();
//            Dictionary<DateTime, IntradayInfo> dicIntradayInfo = new Dictionary<DateTime, IntradayInfo>();

//            foreach(var realTimeItem in realTimeItems)
//            {
//                DateTime date = realTimeItem.Time.Date;
//                if (!dicIntradayInfo.ContainsKey(date))
//                {
//                    dicIntradayInfo.Add(date, new IntradayInfo(date));
//                }

//                dicIntradayInfo[date].Add(realTimeItem);
//            }

//            foreach(var infoItem in dicIntradayInfo.Values)
//            {
//                result.AddRange(infoItem.Items);
//            }

//            return result;
//        }
//    }
//}
