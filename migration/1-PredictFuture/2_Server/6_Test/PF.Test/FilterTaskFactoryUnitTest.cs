using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.Domain.FilterEngine;

namespace PF.Test
{
    [TestClass]
    public class FilterTaskFactoryUnitTest
    {
        [TestMethod]
        public void CreateNewTest1()
        {
            var factory = new FilterTaskFactory(2);
            var task1 = factory.CreateNew(() => Thread.Sleep(500));
            var task2 = factory.CreateNew(() => Thread.Sleep(1000));
            var starttime = DateTime.Now;
            task1.Start();
            task2.Start();
            var task3 = factory.CreateNew(() => Thread.Sleep(50));
            var timespan = DateTime.Now - starttime;
            task3.Start();
            Assert.IsTrue(timespan.TotalMilliseconds > 500);
        }

        [TestMethod]
        public void CreateNewTest2()
        {
            var factory = new FilterTaskFactory(2);
            var task1 = factory.CreateNew(() => { Thread.Sleep(500); return 500; });
            var task2 = factory.CreateNew(() => Thread.Sleep(1000));
            var starttime = DateTime.Now;
            task1.Start();
            task2.Start();
            var task3 = factory.CreateNew(() => { Thread.Sleep(50); return 500; });
            var timespan = DateTime.Now - starttime;
            task3.Start();
            Assert.IsTrue(timespan.TotalMilliseconds > 500);
        }
    }
}
