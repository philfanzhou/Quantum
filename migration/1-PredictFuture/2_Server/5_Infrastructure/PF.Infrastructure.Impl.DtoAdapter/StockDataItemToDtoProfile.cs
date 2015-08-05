namespace PF.Infrastructure.Impl.DtoAdapter
{
    using AutoMapper;
    using PF.Application.Dto.StockData;
    using PF.Domain.StockData.Entities;

    public class StockDataItemToDtoProfile<TStockDataItem, TPriceDataItemDto> : Profile
        where TStockDataItem : StockDataItemBase
        where TPriceDataItemDto : StockDataItemDtoBase
    {
        protected override void Configure()
        {
            CreateMap<TStockDataItem, TPriceDataItemDto>().ForMember(dest => dest.StockSymbol, opt => opt.MapFrom(src => src.Stock.Symbol));
            base.Configure();
        }
    }
}
