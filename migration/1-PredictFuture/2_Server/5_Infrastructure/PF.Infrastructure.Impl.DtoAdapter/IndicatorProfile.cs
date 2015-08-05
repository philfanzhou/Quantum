using AutoMapper;
using PF.Application.Dto.Indicator;
using PF.Domain.Indicator;

namespace PF.Infrastructure.Impl.DtoAdapter
{
    internal class IndicatorDomainToDto : Profile
    {
        protected override void Configure()
        {
            CreateMap<IIndicator, IndicatorDto>();
            base.Configure();
        }
    }
}
