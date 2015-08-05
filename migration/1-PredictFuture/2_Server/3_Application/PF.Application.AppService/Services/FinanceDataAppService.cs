namespace PF.Application.StockData.ServiceImpl
{
    using Core.Application;
    using Core.Domain;
    using Core.Infrastructure.Crosscutting;
    using PF.Application.Dto.StockData;
    using PF.Domain.StockData;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FinanceDataAppService
    {
        public IEnumerable<DividendDataItemDto> GetDivdendData(
            string symbol, 
            DateTime startDay, 
            DateTime endDay)
        {
            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DividendDataRepository>();
                var data = repository.GetDividendData(symbol, startDay, endDay).ToList();
                var result = data.Select(d => d.ProjectedAs<DividendDataItemDto>()).ToList();
                return result;
            }
        }
    }
}
