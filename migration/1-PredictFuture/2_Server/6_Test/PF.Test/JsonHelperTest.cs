using System.Linq;
using Core.Infrastructure.Crosscutting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.Domain.FilterConditions.Entities;
using System;
using System.Collections.Generic;

namespace PF.Test
{
    [TestClass]
    public class JsonHelperTest
    {
        private ISerializer serializer = ContainerHelper.Resolve<ISerializer>();

        [TestMethod]
        public void SerializeTest()
        {
            ConditionResult cr = new ConditionResult(new List<string> { "test1", "test2" });
            string result = serializer.JsonSerializer(cr);

            Assert.IsNotNull(result);

            result = serializer.JsonSerializer<object>(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeserializeTest()
        {
            ConditionResult cr = new ConditionResult(new List<string> { "test1", "test2" });
            string result = serializer.JsonSerializer(cr);

            ConditionResult newCr = serializer.JsonDeserialize<ConditionResult>(result);

            Assert.IsNotNull(newCr);



            var guid = serializer.JsonDeserialize<Guid>(null);
            Assert.AreEqual(guid, Guid.Empty);

            LinkedList<object> objs = new LinkedList<object>();
            objs.AddFirst(newCr);
            result = serializer.JsonSerializer(objs);
            var newObjs = serializer.JsonDeserialize<LinkedList<object>>(result);
            Assert.IsTrue(newObjs.OfType<ConditionResult>().Any());

            newCr = serializer.JsonDeserialize<ConditionResult>(null);
            Assert.IsNull(newCr);
        }
    }
}
