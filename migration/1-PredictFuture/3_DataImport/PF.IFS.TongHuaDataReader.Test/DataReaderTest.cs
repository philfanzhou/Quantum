using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.IFS.TongHua.DataReader;
using System.Linq;

namespace PF.IFS.TongHuaDataReader.Test
{
    [TestClass]
    public class DataReaderTest
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            new DataReader();
        }

        [TestMethod]
        public void SymbolListTest()
        {
            DataReader dataReader = new DataReader();
            dataReader.AnalyseDayLineFiles(new[] { TestData.ShanghaiDay, TestData.ShenzhenDay });
            dataReader.AnalyseDividendFile(TestData.DividendFile);
            bool result = dataReader.DayLineSymbols.Contains("600036");
            Assert.IsTrue(result);
        }
    }
}
