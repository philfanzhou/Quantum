namespace PF.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PF.Application.StockData.ServiceImpl;
    using PF.Domain.StockData.Entities;
    using PF.Infrastructure.Impl.DbConfig;
    using System;
    using System.Linq;

    [TestClass]
    public class StockDataUnitofWorkTest
    {
        [TestMethod]
        public void KlineDataAppServiceTest()
        {
            using (var ctx = new PfDbContext())
            {
                var stock = new Stock("93079434-3E1C-4DC8-9157-BC0BAB130AB3") { Symbol = "600576" };
                ctx.Set<Stock>().Add(stock);
                ctx.Set<DailyPriceDataItem>()
                    .Add(new DailyPriceDataItem() { Date = new DateTime(2014, 2, 19), Stock = stock });
                ctx.SaveChanges();
            }

            var result = new PriceDataAppService().GetDaylineData("600576", new DateTime(2014, 2, 19), new DateTime(2014, 2, 20));
            Assert.IsNotNull(result.FirstOrDefault(d => d.StockSymbol == "600576"));
        }

        [TestMethod]
        public void DividendDataAppServiceTest()
        {
            using (var ctx = new PfDbContext())
            {
                var stock = new Stock("6877E16E-BCB6-4CE4-95FF-F36ABA46946B") { Symbol = "600577" };
                ctx.Set<Stock>().Add(stock);
                ctx.Set<DividendDataItem>()
                    .Add(new DividendDataItem { Date = new DateTime(2014, 2, 19), Stock = stock });
                ctx.SaveChanges();
            }

            var result = new FinanceDataAppService().GetDivdendData("600577", new DateTime(2014, 2, 19), new DateTime(2014, 2, 20));
            Assert.IsNotNull(result.FirstOrDefault(d => d.StockSymbol == "600577"));
        }
    }
}
