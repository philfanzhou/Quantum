//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using PF.Domain.Indicator;
//using PF.Domain.StockData.Service;

//namespace PF.Test
//{
//    [TestClass]
//    public class IndicatorTest
//    {
//        [TestMethod]
//        public void GetSuppoertedIndicators()
//        {
//            PriceDataService pds = new PriceDataService();
//            IndicatorService service = new IndicatorService(pds);
//            var items =service.GetSupportedIndicators();
//            Assert.IsNotNull(items);
//        }

//        [TestMethod]
//        public void GetIndicator()
//        {
//            PriceDataService pds = new PriceDataService();
//            IndicatorService service = new IndicatorService(pds);
//            var item = service.QueryIndicator("OpenPrice", IndicatorCycle.DailyLine);
//            Assert.IsNotNull(item);
//        }
//    }
//}
