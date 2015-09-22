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
        private static readonly TimeSpan span = new TimeSpan(0, 1, 0);

        private readonly List<IntradayData> _items = new List<IntradayData>();
        private readonly DateTime date;

        public IntradayInfo(DateTime date)
        {
            this.date = date.Date;
        }

        public override string ToString()
        {
            return this.date.ToString("yyyy-MM-dd");
        }

        public IEnumerable<IntradayData> Items 
        {
            get
            {
                return _items;
            }
        }

        public void Add(IEnumerable<RealTimeItem> items)
        {
            foreach(var item in items)
            {
                Add(item);
            }
        }

        public void Add(RealTimeItem item)
        {
            if (item.Time.Date != this.date.Date)
                throw new ArgumentOutOfRangeException("item");

            if (_items.Count < 1 ||
                item.Time - _items.Last().Time > span)
            {
                var newItem = new IntradayData
                {
                    Time = new DateTime(
                        item.Time.Year,
                        item.Time.Month,
                        item.Time.Day,
                        item.Time.Hour,
                        item.Time.Minute,
                        0)
                };

                this._items.Add(newItem);
            }

            IntradayData currentData = _items.Last();

            currentData.Price = item.Price;
            currentData.Change = Math.Round(item.Price - item.TodayOpen, 2);
            currentData.ChangeRate =
                Math.Round(currentData.Change / item.TodayOpen * 100, 2);

            currentData.TotalVolume = item.Volume;
            currentData.TotalAmount = item.Amount;
            currentData.AveragePrice = Math.Round(currentData.TotalAmount / currentData.TotalVolume, 2);
            currentData.BuyVolume = item.BuyVolume();
            currentData.SellVolume = item.SellVolume();


            if (_items.Count <= 1)
            {
                currentData.IntradayVolume = item.Volume;
                currentData.IntradayAmount = item.Amount;
            }
            else
            {
                IntradayData previousDate = _items[_items.Count - 2];
                currentData.IntradayVolume = item.Volume - previousDate.TotalVolume;
                currentData.IntradayAmount = item.Amount - previousDate.TotalAmount;
            }
        }
    }
}
