using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.Application.Dto.FilterCondition;
using PF.Application.Dto.FilterTask;
using PF.Application.Dto.Indicator;
using PF.Application.StockFilter;
using PF.DistributedService.Hosting;
using PF.Domain.FilterTasks;
using PF.Infrastructure.Impl.DbConfig;

namespace PF.Test
{
    [TestClass]
    public class FilterTaskAppServiceUnitTest
    {
        [TestInitialize]
        public void Initialize()
        {
            ServiceInitialize.Init();
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (var ctx = new PfDbContext())
            {
                if (ctx.Database.Exists())
                {
                    ctx.Database.Delete();
                }
                ctx.Database.Create();
            }
        }

        [TestMethod]
        public void AddDraftFilterTaskTest()
        {
            var taskdto = new DraftFilterTaskDto
                {
                    Name = "DraftTask",
                    Condition = new FilterConditionDto
                    {
                        Name = "DraftTaskConditon",
                        ExpressionString = "成交量>100",
                        Indicators = new List<IndicatorDto> { new IndicatorDto { Name = "成交量" } }
                    }
                };
            var service = new FilterTaskAppService();
            service.AddDraftFilterTask(taskdto, 0);

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                var task = repository.GetAll().FirstOrDefault();
                Assert.IsNotNull(task);
            }
        }

        [TestMethod]
        public void AddScheduledFilterTaskTest()
        {
            var taskdto = new ScheduledFilterTaskDto
                {
                    Name = "ScheduledFilterTask",
                    ScheduleExecTime = DateTime.Now,
                    ScheduleRepeat = "OpenDay",
                    Condition = new FilterConditionDto
                    {
                        Name = "ScheduledFilterTaskConditon",
                        ExpressionString = "成交量>100",
                        Indicators = new List<IndicatorDto> { new IndicatorDto { Name = "成交量" } }
                    }
                };
            var service = new FilterTaskAppService();
            service.AddScheduledFilterTask(taskdto, 0);

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<ScheduledFilterTaskRepository>();
                var task = repository.GetAll().FirstOrDefault();
                Assert.IsNotNull(task);
            }
        }

        [TestMethod]
        public void GetAllDraftFilterTasksTest()
        {
            var taskdto = new DraftFilterTaskDto
                {
                    Name = "DraftTask",
                    Condition = new FilterConditionDto
                    {
                        Name = "DraftTaskConditon",
                        ExpressionString = "成交量>100",
                        Indicators = new List<IndicatorDto> { new IndicatorDto { Name = "成交量" } }
                    }
                };
            var service = new FilterTaskAppService();
            service.AddDraftFilterTask(taskdto, 0);

            var tasks = service.GetAllDraftFilterTasks(0).ToList();
            Assert.IsNotNull(tasks);
            Assert.AreEqual(1, tasks.Count);
        }

        [TestMethod]
        public void GetAllScheduledFilterTasksTest()
        {
            var taskdto = new ScheduledFilterTaskDto
                {
                    Name = "ScheduledFilterTask",
                    ScheduleExecTime = DateTime.Now,
                    ScheduleRepeat = "OpenDay",
                    Condition = new FilterConditionDto
                    {
                        Name = "ScheduledFilterTaskConditon",
                        ExpressionString = "成交量>100",
                        Indicators = new List<IndicatorDto> { new IndicatorDto { Name = "成交量" } }
                    }
                };
            var service = new FilterTaskAppService();
            service.AddScheduledFilterTask(taskdto, 0);

            var tasks = service.GetAllScheduledFilterTasks(0).ToList();
            Assert.IsNotNull(tasks);
            Assert.AreEqual(1, tasks.Count);
        }

        [TestMethod]
        public void UpdateDraftFilterTaskTest()
        {
            var taskdto = new DraftFilterTaskDto
                {
                    Name = "DraftTask",
                    Condition = new FilterConditionDto
                    {
                        Name = "DraftTaskConditon",
                        ExpressionString = "成交量>100",
                        Indicators = new List<IndicatorDto> { new IndicatorDto { Name = "成交量" } }
                    }
                };
            var service = new FilterTaskAppService();
            service.AddDraftFilterTask(taskdto, 0);

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                var task = repository.GetAll().First();
                var newtaskdto = new DraftFilterTaskDto
                    {
                        Id = task.Id,
                        Name = "DraftTask1",
                        Condition = new FilterConditionDto
                        {
                            Id = task.ConditionId,
                            Name = "DraftTaskConditon1",
                            ExpressionString = "成交量>1001",
                            Indicators = new List<IndicatorDto> { new IndicatorDto { Name = "成交量" } }
                        }
                    };
                service.UpdateDraftFilterTask(newtaskdto);
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<DraftFilterTaskRepository>();
                var task = repository.GetAll().FirstOrDefault();
                Assert.IsNotNull(task);
                Assert.AreEqual("DraftTask1", task.Name);
                Assert.AreEqual("DraftTaskConditon1", task.Condition.Name);
            }
        }

        [TestMethod]
        public void UpdateScheduledFilterTaskTest()
        {
            var taskdto = new ScheduledFilterTaskDto
                {
                    Name = "ScheduledFilterTask",
                    ScheduleExecTime = DateTime.Now,
                    ScheduleRepeat = "OpenDay",
                    Condition = new FilterConditionDto
                    {
                        Name = "ScheduledFilterTaskConditon",
                        ExpressionString = "成交量>100",
                        Indicators = new List<IndicatorDto> { new IndicatorDto { Name = "成交量" } }
                    }
                };
            var service = new FilterTaskAppService();
            service.AddScheduledFilterTask(taskdto, 0);

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<ScheduledFilterTaskRepository>();
                var task = repository.GetAll().First();
                var newtaskdto = new ScheduledFilterTaskDto
                    {
                        Id = task.Id,
                        Name = "ScheduledFilterTask1",
                        ScheduleExecTime = DateTime.Now,
                        ScheduleRepeat = "OpenDay",
                        Condition = new FilterConditionDto
                        {
                            Id = task.ConditionId,
                            Name = "ScheduledFilterTaskConditon1",
                            ExpressionString = "成交量>100",
                            Indicators = new List<IndicatorDto> { new IndicatorDto { Name = "成交量" } }
                        }
                    };
                service.UpdateScheduledFilterTask(newtaskdto);
            }

            using (var context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<ScheduledFilterTaskRepository>();
                var task = repository.GetAll().FirstOrDefault();
                Assert.IsNotNull(task);
                Assert.AreEqual("ScheduledFilterTask1", task.Name);
                Assert.AreEqual("ScheduledFilterTaskConditon1", task.Condition.Name);
            }
        }
    }
}
