//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Ore.Infrastructure.MarketData;
//using Pitman.Infrastructure.FileDatabase;
//using Quantum.Domain.MarketData;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Test.Domain.MarketData
//{
//    [TestClass]
//    public class UnitTest1
//    {
//        [TestMethod]
//        public void TestGetIntradayData()
//        {
//            RealTimeDataRepository repository = new RealTimeDataRepository();
//            var realTimeData = repository.GetData("600036", new DateTime(2015, 12, 7), new DateTime(2015, 12, 7));
//            List<IStockIntraday> intradayList = IntradayConverter.ConvertToIntraday(realTimeData).ToList();
//        }

//        [TestMethod]
//        public void TestGet1MinuteKLine()
//        {
//            RealTimeDataRepository repository = new RealTimeDataRepository();
//            var realTimeData = repository.GetData("600036", new DateTime(2015, 12, 7), new DateTime(2015, 12, 7));
//            List<IStockKLine> kLineList = KLineConverter.ConvertTo1Minute(realTimeData).ToList();
//        }
//    }
//}
