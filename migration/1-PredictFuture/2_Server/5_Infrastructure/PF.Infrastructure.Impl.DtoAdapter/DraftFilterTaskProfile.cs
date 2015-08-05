using System;
using AutoMapper;
using PF.Application.Dto.FilterTask;
using PF.Domain.FilterTasks.Entities;

namespace PF.Infrastructure.Impl.DtoAdapter
{
    internal class DraftFilterTaskDomainToDto : Profile
    {
        protected override void Configure()
        {
            CreateMap<DraftFilterTask, DraftFilterTaskDto>()
                .ForMember(dest => dest.ExecStartTime, opt => opt.MapFrom(src => src.Result.ExecStartTime))
                .ForMember(dest => dest.ExecEndTime, opt => opt.MapFrom(src => src.Result.ExecEndTime));
            base.Configure();
        }
    }

    internal class DraftFilterTaskDtoToDomain : Profile
    {
        protected override void Configure()
        {
            CreateMap<DraftFilterTaskDto, DraftFilterTask>()
                .ConstructUsing(t => new DraftFilterTask(string.IsNullOrEmpty(t.Id) ? Guid.Empty.ToString() : t.Id))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Result, opt => opt.Ignore());
            base.Configure();
        }
    }
}
