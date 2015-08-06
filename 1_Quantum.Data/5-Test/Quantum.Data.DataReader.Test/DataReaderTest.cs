using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Data.Metadata;
using System.Collections.Generic;

namespace Quantum.Data.DataReader.Test
{
    [TestClass]
    public class DataReaderTest
    {
        [TestMethod]
        public void SinaRealTimeDataConstructorTest()
        {
            SinaDataReader reader = new SinaDataReader();
            IRealTimeData data = reader.GetData("sh600036");
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void GetMultipleDataTest()
        {
            string[] codes = new string[] { "sh600036","sz150209","sh600518","sz300118","sh600298","sh601009","sh601933","sh600660","sh600196" };
            SinaDataReader reader = new SinaDataReader();
            IEnumerable<IRealTimeData> datas = reader.GetData(codes);

            Assert.IsNotNull(datas);
            foreach(IRealTimeData data in datas)
            {
                Assert.IsNotNull(data);
            }
        }
    }
}
