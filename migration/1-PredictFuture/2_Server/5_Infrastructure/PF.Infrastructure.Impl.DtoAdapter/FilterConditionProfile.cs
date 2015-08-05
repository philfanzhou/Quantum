using System;
using System.Linq;
using AutoMapper;
using PF.Application.Dto.FilterCondition;
using PF.Domain.FilterConditions.Entities;
using PF.Domain.Indicator;

namespace PF.Infrastructure.Impl.DtoAdapter
{
    internal class FilterConditionDomainToDto : Profile
    {
        protected override void Configure()
        {
            CreateMap<FilterCondition, FilterConditionDto>()
                .ForMember(dest => dest.ExpressionString, opt => opt.MapFrom(src => src.Expression.ExpressionString))
                .ForMember(dest => dest.Indicators, opt => opt.MapFrom(src => src.Expression.Indicators));
            base.Configure();
        }
    }

    internal class FilterConditionDtoToDomain : Profile
    {
        protected override void Configure()
        {
            CreateMap<FilterConditionDto, FilterCondition>()
                .ConstructUsing(dest => new FilterCondition(string.IsNullOrEmpty(dest.Id) ? Guid.Empty.ToString() : dest.Id, -1, Convert(dest)))
                .ForMember(dest => dest.CreateTime, opt => opt.Ignore())
                .ForMember(dest => dest.CreateByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.Expression, opt => opt.Ignore())
                .ForMember(dest => dest.Result, opt => opt.Ignore())
                .ForMember(dest => dest.SerializedResult, opt => opt.Ignore());
            base.Configure();
        }

        private FilterExpression Convert(FilterConditionDto filterConditionDto)
        {
            var indicators = filterConditionDto.Indicators.Select(i => IndicatorFactory.FindIndicatorByName(i.Name)).Where(i => i != null);
            var id = string.IsNullOrEmpty(filterConditionDto.Id) ? Guid.Empty.ToString() : filterConditionDto.Id;
            return new FilterExpression(id, filterConditionDto.ExpressionString, indicators);
        }
    }
}
