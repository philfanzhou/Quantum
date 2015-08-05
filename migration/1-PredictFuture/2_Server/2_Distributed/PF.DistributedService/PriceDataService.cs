using System.Linq;
using Core.Infrastructure.Crosscutting.Service;
using PF.Application.Dto.StockData;
using PF.Application.StockData.ServiceImpl;
using PF.DistributedService.Contracts.StockData;
using System;
using System.Collections.Generic;

namespace PF.DistributedService
{
    public class PriceDataService : WCFServiceBase, IPriceDataService
    {
        private readonly PriceDataAppService _appService
            = new PriceDataAppService();
        public List<DailyPriceDataItemDto> GetDaylineData(string symbol, DateTime startDay, DateTime endDay)
        {
            return _appService.GetDaylineData(symbol, startDay, endDay).ToList();
        }
    }
}
