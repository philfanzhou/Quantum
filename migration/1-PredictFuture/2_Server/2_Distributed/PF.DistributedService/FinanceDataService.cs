using System.Linq;
using Core.Infrastructure.Crosscutting.Service;
using PF.Application.Dto.StockData;
using PF.Application.StockData.ServiceImpl;
using PF.DistributedService.Contracts.StockData;
using System;
using System.Collections.Generic;

namespace PF.DistributedService
{
    public class FinanceDataService : WCFServiceBase, IFinanceDataService
    {
        private readonly FinanceDataAppService _appService
            = new FinanceDataAppService();
        public List<DividendDataItemDto> GetDivdendData(string symbol, DateTime startDay, DateTime endDay)
        {
            return _appService.GetDivdendData(symbol, startDay, endDay).ToList();
        }
    }
}
