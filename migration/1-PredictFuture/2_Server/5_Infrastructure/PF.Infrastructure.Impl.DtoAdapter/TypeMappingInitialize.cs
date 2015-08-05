using AutoMapper;
using PF.Application.Dto.StockData;
using PF.Domain.StockData.Entities;

namespace PF.Infrastructure.Impl.DtoAdapter
{
    public class TypeMappingInitialize
    {
        public static void Init()
        {
            // DTO Mapper
            Mapper.Initialize(conf =>
            {
                conf.AddProfile<StockDataItemToDtoProfile<DailyPriceDataItem, DailyPriceDataItemDto>>();
                conf.AddProfile<StockDataItemToDtoProfile<DividendDataItem, DividendDataItemDto>>();
                conf.AddProfile<DraftFilterTaskDomainToDto>();
                conf.AddProfile<DraftFilterTaskDtoToDomain>();
                conf.AddProfile<ScheduledFilterTaskDomainToDto>();
                conf.AddProfile<ScheduledFilterTaskDtoToDomain>();
                conf.AddProfile<FilterConditionDomainToDto>();
                conf.AddProfile<FilterConditionDtoToDomain>();
                conf.AddProfile<IndicatorDomainToDto>();
            });
        }
    }
}
