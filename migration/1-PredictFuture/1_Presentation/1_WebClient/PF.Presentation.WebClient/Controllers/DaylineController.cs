using Core.DistributedServices.WCF;
using PF.Application.Dto.StockData;
using PF.DistributedService.Contracts.StockData;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PF.Presentation.WebClient.Controllers
{
    public class DaylineController : Controller
    {
        public ActionResult Index()
        {
            return View(new List<DailyPriceDataItemDto>());
        }

        [HttpPost]
        public ActionResult Index(string stockSymbol, string startDate, string endDate)
        {
            var client = new ServiceClient<IPriceDataService>();
            var list = client.Invoke(x => x.GetDaylineData(stockSymbol, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate)));

            return View(list);
        }

    }
}
