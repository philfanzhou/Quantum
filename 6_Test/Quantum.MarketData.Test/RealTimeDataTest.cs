using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Infrastructure.MarketData.Repository;
using Pitman.DataReader;
using Pitman.Metadata;
using System.Diagnostics;

namespace Quantum.MarketData.Test
{
    /// <summary>
    /// Summary description for RealTimeFileTest
    /// </summary>
    [TestClass]
    public class RealTimeDataTest
    {
        [TestMethod]
        public void TestCreateFile()
        {
            var reader = DataReaderCreator.Create();
            var repository = new RealTimeDataRepository();
            while (DateTime.Now.Hour < 16)
            {
                System.Threading.Thread.Sleep(4000);
                var realTimedata = reader.GetData("sh600036");

                var realTimeItem = ConvertDataToItem(realTimedata);
                repository.Add(MarketType.Shanghai, "600036", realTimeItem);

                Debug.WriteLine(realTimeItem.ToString());
            }

        }

        [TestMethod]
        public void TestReadData()
        {
            var repository = new RealTimeDataRepository();
            var dataList = repository.GetOneDayData(MarketType.Shanghai, "600036", DateTime.Now);
        }

        private RealTimeItem ConvertDataToItem(RealTimeData data)
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
                BuyOnePrice  = data.BuyOnePrice,
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
