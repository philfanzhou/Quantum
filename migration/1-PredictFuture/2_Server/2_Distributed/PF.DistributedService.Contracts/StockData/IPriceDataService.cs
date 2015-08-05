using System.ServiceModel;

namespace PF.DistributedService.Contracts.StockData
{
    using PF.Application.Dto.StockData;
    using System;
    using System.Collections.Generic;

    [ServiceContract]
    public interface IPriceDataService
    {
        [OperationContract]
        List<DailyPriceDataItemDto> GetDaylineData(string symbol, DateTime startDay, DateTime endDay);
    }
}
