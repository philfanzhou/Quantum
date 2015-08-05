using System.ServiceModel;

namespace PF.DistributedService.Contracts.StockData
{
    using PF.Application.Dto.StockData;
    using System;
    using System.Collections.Generic;

    [ServiceContract]
    public interface IFinanceDataService
    {
        [OperationContract]
        List<DividendDataItemDto> GetDivdendData(string symbol, DateTime startDay, DateTime endDay);
    }
}
