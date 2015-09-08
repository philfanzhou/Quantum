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
            IntradayData currentData;
            bool needUpdate;

            if(items.Count == 0)
            {
                needUpdate = false;
            }
            else if (realTimeData.Time - items.Last().Time < _timeSpen)
            {
                needUpdate = true;
            }
            else
            {
                needUpdate = false;
            }

            if(!needUpdate)
            {
                currentData = new IntradayData();
                this.items.Add(currentData);

                currentData.Time = new DateTime(
                    realTimeData.Time.Year,
                    realTimeData.Time.Month,
                    realTimeData.Time.Day,
                    realTimeData.Time.Hour,
                    realTimeData.Time.Minute,
                    0);
            }
            else
            {
                currentData = items.Last();
            }

            currentData.Price = realTimeData.Price;
            currentData.Change = Math.Round(realTimeData.Price - realTimeData.TodayOpen, 2);
            currentData.ChangeRate = 
                Math.Round(currentData.Change / realTimeData.TodayOpen * 100, 2);
            currentData.TotalVolume = realTimeData.Volume;
            currentData.TotalAmount = realTimeData.Amount;
            currentData.AveragePrice = Math.Round(currentData.TotalAmount / currentData.TotalVolume, 2);

            if(items.Count <= 1)
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
