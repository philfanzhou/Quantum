//using Ore.Infrastructure.MarketData;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Quantum.Domain.MarketData
//{
//    /// <summary>
//    /// 分时数据信息类
//    /// </summary>
//    internal class IntradayInfo
//    {
//        private static readonly TimeSpan span = new TimeSpan(0, 1, 0);

//        private readonly List<StockIntraday> _intradayItems = new List<StockIntraday>();
//        private readonly DateTime date;

//        public IntradayInfo(DateTime date)
//        {
//            this.date = date.Date;
//        }

//        public override string ToString()
//        {
//            return this.date.ToString("yyyy-MM-dd");
//        }

//        public IEnumerable<IStockIntraday> Items
//        {
//            get
//            {
//                return _intradayItems;
//            }
//        }

//        public void Add(IEnumerable<IStockRealTime> realTimeItems)
//        {
//            foreach (var realTimeItem in realTimeItems)
//            {
//                Add(realTimeItem);
//            }
//        }

//        public void Add(IStockRealTime realTimeItem)
//        {
//            if (realTimeItem.Time.Date != this.date.Date)
//                throw new ArgumentOutOfRangeException("item");

//            AddNewIntradayItemIfNeeded(realTimeItem);
//            UpdateLastIntradayItemInfo(realTimeItem);
//        }

//        private void AddNewIntradayItemIfNeeded(IStockRealTime realTimeItem)
//        {
//            if (_intradayItems.Count < 1 || 
//                realTimeItem.Time - _intradayItems.Last().Time > span)
//            {
//                var newItem = new StockIntraday
//                {
//                    Time = new DateTime(
//                        realTimeItem.Time.Year,
//                        realTimeItem.Time.Month,
//                        realTimeItem.Time.Day,
//                        realTimeItem.Time.Hour,
//                        realTimeItem.Time.Minute,
//                        0),
//                };

//                this._intradayItems.Add(newItem);
//            }
//        }

//        private void UpdateLastIntradayItemInfo(IStockRealTime realTimeItem)
//        {
//            StockIntraday intradayItem = _intradayItems.Last();

//            intradayItem.YesterdayClose = realTimeItem.YesterdayClose;
//            intradayItem.BuyVolume = realTimeItem.BuyVolume();
//            intradayItem.SellVolume = realTimeItem.SellVolume();

//            //intradayItem.CurrentTotalVolume = realTimeItem.Volume;
//            //intradayItem.CurrentTotalAmount = realTimeItem.Amount;

//            // 集合竞价期间需要对当前价格进行特殊处理,因为集合竞价期间不存在成交价，只存在委卖委买价
//            intradayItem.Current = Math.Abs(realTimeItem.Current) < 0.00001
//                ? realTimeItem.Sell1Price
//                : realTimeItem.Current;

//            // 集合竞价期间无成交量，不进行均价计算
//            intradayItem.AveragePrice = Math.Abs(realTimeItem.Volume) < 0.00001 ? 0 :
//                Math.Round(realTimeItem.Amount / realTimeItem.Volume, 2);

//            // 根据前面的数据，求出分时成交量和成交额
//            if (_intradayItems.Count > 1)
//            {
//                StockIntraday previousDate = _intradayItems[_intradayItems.Count - 2];
//                //intradayItem.Volume = realTimeItem.Volume - previousDate.CurrentTotalVolume;
//                //intradayItem.Amount = realTimeItem.Amount - previousDate.CurrentTotalAmount;
//            }
//            else
//            {
//                intradayItem.Volume = realTimeItem.Volume;
//                intradayItem.Amount = realTimeItem.Amount;
//            }
//        }
//    }
//}
