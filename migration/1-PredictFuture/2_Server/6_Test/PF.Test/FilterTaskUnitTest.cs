using System;
using Core.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.Application.Dto.FilterTask;
using PF.Domain.FilterConditions.Entities;
using PF.Domain.FilterTasks.Entities;
using PF.Domain.Indicator;

namespace PF.Test
{
    [TestClass]
    public class FilterTaskUnitTest
    {
        [TestMethod]
        public void FilterTaskToDtoTest()
        {
            var expression = new FilterExpression(Guid.NewGuid().ToString(), "成交量>100", new[] { new AmountIndicator() });
            var filtertask = new DraftFilterTask(Guid.NewGuid().ToString())
                {
                    Name = "Task1",
                    Condition = new FilterCondition(Guid.NewGuid().ToString(), 1, expression)
                    {
                        Name = "Conditiion1",
                        Description = "Conditiion1",
                    }
                };
            var filtertaskdto = filtertask.ProjectedAs<DraftFilterTaskDto>();
            var task = filtertaskdto.ProjectedAs<DraftFilterTask>();
            Assert.AreEqual(task.Id, filtertask.Id);
        }
    }
}
