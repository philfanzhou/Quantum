using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 一分钟K线信息类
    /// </summary>
    public class KLine1MinuteInfo
    {
        private static readonly TimeSpan span = new TimeSpan(0, 1, 0);

        private readonly List<StockMinutesKLine> _items = new List<StockMinutesKLine>();
        private readonly DateTime date;

        public KLine1MinuteInfo(DateTime date)
        {
            this.date = date.Date;
        }

        public override string ToString()
        {
            return this.date.ToString("yyyy-MM-dd");
        }

        public IEnumerable<IStockKLine> Items
        {
            get
            {
                return _items;
            }
        }
        public void Add(IEnumerable<IStockRealTime> realTimeItems)
        {
            foreach (var realTimeItem in realTimeItems)
            {
                Add(realTimeItem);
            }
        }

        public void Add(IStockRealTime realTimeItem)
        {
            if (realTimeItem.Time.Date != this.date.Date)
                throw new ArgumentOutOfRangeException("item");

            // todo: 过滤掉非交易时间的数据

            AddNewItemIfNeeded(realTimeItem);

            UpdateLastItemInfo(realTimeItem);
        }

        private void AddNewItemIfNeeded(IStockRealTime realTimeItem)
        {
            if (_items.Count < 1 || 
                realTimeItem.Time - _items.Last().Time > span)
            {
                var newItem = new StockMinutesKLine
                {
                    Time = new DateTime(
                        realTimeItem.Time.Year,
                        realTimeItem.Time.Month,
                        realTimeItem.Time.Day,
                        realTimeItem.Time.Hour,
                        realTimeItem.Time.Minute,
                        0),

                    // 当前分析周期的开盘价 = 第一条数据的成交价
                    Open = realTimeItem.Current,

                    // 构造第一条数据的时候，就设定好最高、最低的值
                    High = realTimeItem.Current,
                    Low = realTimeItem.Current,
                };

                _items.Add(newItem);
            }
        }

        private void UpdateLastItemInfo(IStockRealTime realTimeItem)
        {
            StockMinutesKLine currentItem = _items.Last();

            currentItem.Close = realTimeItem.Current;

            // 取得最高价和最低价
            if(realTimeItem.Current > currentItem.High)
            {
                currentItem.High = realTimeItem.Current;
            }
            else if(realTimeItem.Current < currentItem.Low)
            {
                currentItem.Low = realTimeItem.Current;
            }

            //// 根据前面的数据，求出分时成交量和成交额, 以及前收盘价
            //if (_items.Count > 1)
            //{
            //    StockMinutesKLine previousDate = _items[_items.Count - 2];
            //    currentItem.Volume = realTimeItem.Volume - previousDate.CurrentTotalVolume;
            //    currentItem.Amount = realTimeItem.Amount - previousDate.CurrentTotalAmount;

            //    currentItem.PreClose = previousDate.Current;
            //}
            //else
            //{
            //    currentItem.Volume = realTimeItem.Volume;
            //    currentItem.Amount = realTimeItem.Amount;

            //    // 如果是某一天的第一条数据，前收盘价就取昨日收盘价
            //    currentItem.PreClose = realTimeItem.YesterdayClose;
            //}
        }
    }
}
