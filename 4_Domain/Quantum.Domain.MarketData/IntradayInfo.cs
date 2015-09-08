using Quantum.Infrastructure.MarketData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 分时数据信息类
    /// </summary>
    public class IntradayInfo
    {
        private static TimeSpan _timeSpen = new TimeSpan(0, 1, 0);
        private List<IntradayData> items = new List<IntradayData>();

        public IEnumerable<IntradayData> Items 
        {
            get
            {
                return items;
            }
        }

        public void AddRealTimeData(RealTimeData realTimeData)
        {

            if (items.Count < 1 ||
                realTimeData.Time - items.Last().Time > _timeSpen)
            {
                var newItem = new IntradayData();
                newItem.Time = new DateTime(
                    realTimeData.Time.Year,
                    realTimeData.Time.Month,
                    realTimeData.Time.Day,
                    realTimeData.Time.Hour,
                    realTimeData.Time.Minute,
                    0);

                this.items.Add(newItem);
            }

            IntradayData currentData = items.Last();

            currentData.Price = realTimeData.Price;
            currentData.Change = Math.Round(realTimeData.Price - realTimeData.TodayOpen, 2);
            currentData.ChangeRate =
                Math.Round(currentData.Change / realTimeData.TodayOpen * 100, 2);

            currentData.TotalVolume = realTimeData.Volume;
            currentData.TotalAmount = realTimeData.Amount;
            currentData.AveragePrice = Math.Round(currentData.TotalAmount / currentData.TotalVolume, 2);
            currentData.BuyVolume = realTimeData.BuyVolume();
            currentData.SellVolume = realTimeData.SellVolume();


            if (items.Count <= 1)
            {
                currentData.IntradayVolume = realTimeData.Volume;
                currentData.IntradayAmount = realTimeData.Amount;
            }
            else
            {
                IntradayData previousDate = items[items.Count - 2];
                currentData.IntradayVolume = realTimeData.Volume - previousDate.TotalVolume;
                currentData.IntradayAmount = realTimeData.Amount - previousDate.TotalAmount;
            }
        }
    }
}
