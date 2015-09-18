using Quantum.Infrastructure.MarketData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 分时数据信息类
    /// </summary>
    public class IntradayInfo
    {
        private static readonly TimeSpan TimeSpan = new TimeSpan(0, 1, 0);
        private readonly List<IntradayData> _items = new List<IntradayData>();

        public IEnumerable<IntradayData> Items 
        {
            get
            {
                return _items;
            }
        }

        public void AddRealTimeData(RealTimeItem realTimeData)
        {

            if (_items.Count < 1 ||
                realTimeData.Time - _items.Last().Time > TimeSpan)
            {
                var newItem = new IntradayData
                {
                    Time = new DateTime(
                        realTimeData.Time.Year,
                        realTimeData.Time.Month,
                        realTimeData.Time.Day,
                        realTimeData.Time.Hour,
                        realTimeData.Time.Minute,
                        0)
                };

                this._items.Add(newItem);
            }

            IntradayData currentData = _items.Last();

            currentData.Price = realTimeData.Price;
            currentData.Change = Math.Round(realTimeData.Price - realTimeData.TodayOpen, 2);
            currentData.ChangeRate =
                Math.Round(currentData.Change / realTimeData.TodayOpen * 100, 2);

            currentData.TotalVolume = realTimeData.Volume;
            currentData.TotalAmount = realTimeData.Amount;
            currentData.AveragePrice = Math.Round(currentData.TotalAmount / currentData.TotalVolume, 2);
            currentData.BuyVolume = realTimeData.BuyVolume();
            currentData.SellVolume = realTimeData.SellVolume();


            if (_items.Count <= 1)
            {
                currentData.IntradayVolume = realTimeData.Volume;
                currentData.IntradayAmount = realTimeData.Amount;
            }
            else
            {
                IntradayData previousDate = _items[_items.Count - 2];
                currentData.IntradayVolume = realTimeData.Volume - previousDate.TotalVolume;
                currentData.IntradayAmount = realTimeData.Amount - previousDate.TotalAmount;
            }
        }
    }
}
