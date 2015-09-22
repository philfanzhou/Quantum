using Pitman.Metadata;
using Quantum.Infrastructure.MarketData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Application.MarketData
{
    public class RealTimeItemConverter
    {
        public static RealTimeItem Convert(RealTimeData data)
        {
            RealTimeItem item = new RealTimeItem
            {
                Amount = data.Amount,
                High = data.High,
                Low = data.Low,
                Price = data.Price,
                Time = data.Time,
                TodayOpen = data.TodayOpen,
                Volume = data.Volume,
                YesterdayClose = data.YesterdayClose,

                BuyFivePrice = data.BuyFivePrice,
                BuyFiveVolume = data.BuyFiveVolume,
                BuyFourPrice = data.BuyFourPrice,
                BuyFourVolume = data.BuyFourVolume,
                BuyThreePrice = data.BuyThreePrice,
                BuyThreeVolume = data.BuyThreeVolume,
                BuyTwoPrice = data.BuyTwoPrice,
                BuyTwoVolume = data.BuyTwoVolume,
                BuyOnePrice = data.BuyOnePrice,
                BuyOneVolume = data.BuyOneVolume,

                SellFivePrice = data.SellFivePrice,
                SellFiveVolume = data.SellFiveVolume,
                SellFourPrice = data.SellFourPrice,
                SellFourVolume = data.SellFourVolume,
                SellThreePrice = data.SellThreePrice,
                SellThreeVolume = data.SellThreeVolume,
                SellTwoPrice = data.SellTwoPrice,
                SellTwoVolume = data.SellTwoVolume,
                SellOnePrice = data.SellOnePrice,
                SellOneVolume = data.SellOneVolume,
            };

            return item;
        }
    }
}
