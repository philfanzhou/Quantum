using System;
using AutoMapper;
using PF.Application.Dto.FilterTask;
using PF.Domain.FilterTasks.Entities;

namespace PF.Infrastructure.Impl.DtoAdapter
{
    internal class ScheduledFilterTaskDomainToDto : Profile
    {
        protected override void Configure()
        {
            CreateMap<ScheduledFilterTask, ScheduledFilterTaskDto>()
                .ForMember(dest => dest.ExecStartTime, opt => opt.MapFrom(src => src.Result.ExecStartTime))
                .ForMember(dest => dest.ExecEndTime, opt => opt.MapFrom(src => src.Result.ExecEndTime))
                .ForMember(dest => dest.ScheduleExecTime, opt => opt.MapFrom(src => src.Schedule.ExecTime))
                .ForMember(dest => dest.ScheduleRepeat, opt => opt.MapFrom(src => src.Schedule.Repeat));
            base.Configure();
        }
    }
    internal class ScheduledFilterTaskDtoToDomain : Profile
    {
        protected override void Configure()
        {
            CreateMap<ScheduledFilterTaskDto, ScheduledFilterTask>()
                .ConstructUsing(t => new ScheduledFilterTask(string.IsNullOrEmpty(t.Id) ? Guid.Empty.ToString() : t.Id, Convert(t)))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Result, opt => opt.Ignore());
            base.Configure();
        }

        private FilterSchedule Convert(ScheduledFilterTaskDto scheduledFilterTask)
        {
            var repeat = (FilterTaskRepeat)Enum.Parse(typeof(FilterTaskRepeat), scheduledFilterTask.ScheduleRepeat);
            return new FilterSchedule { ExecTime = scheduledFilterTask.ScheduleExecTime, Repeat = repeat };
        }
    }
}
