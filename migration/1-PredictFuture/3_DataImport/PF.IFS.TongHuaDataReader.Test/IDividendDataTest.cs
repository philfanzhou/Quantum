using Microsoft.VisualStudio.TestTools.UnitTesting;
using PF.IFS.TongHua.DataReader;
using System;
using System.Linq;

namespace PF.IFS.TongHuaDataReader.Test
{
    [TestClass]
    public class IDividendDataTest
    {
        [TestMethod]
        public void GetDividendDataTest()
        {
            DataReader dataReader = new DataReader();
            dataReader.AnalyseDividendFile(TestData.DividendFile);
            IDividendData info = dataReader.GetDividendData("432534", DateTime.MinValue);
            Assert.IsNull(info);
        }

        [TestMethod]
        public void TestMethod1()
        {
            DataReader dataReader = new DataReader();
            dataReader.AnalyseDividendFile(TestData.DividendFile);
            IDividendData info = dataReader.GetDividendData("600036", DateTime.MinValue);
            Assert.IsNotNull(info);

            var items = info.Items.ToList();
            var item = items[0];
            Assert.AreEqual(0.0, item.Bonus);
            Assert.AreEqual(0.12, item.Cash);
            Assert.AreEqual(new DateTime(2003, 7, 16), item.Date);
            Assert.AreEqual("2003-07-16(每十股 红利1.20元 )", item.Description);
            Assert.AreEqual(0.0, item.Dispatch);
            Assert.AreEqual(new DateTime(2003, 7, 16), item.ExdividendDate);
            Assert.AreEqual(new DateTime(1, 1, 1), item.ListingDate);
            Assert.AreEqual(0.0, item.Price);
            Assert.AreEqual(new DateTime(2003, 7, 15), item.RegisterDate);
            Assert.AreEqual(0.0, item.Split);

            item = items[5];
            Assert.AreEqual(0.0, item.Bonus);
            Assert.AreEqual(0.18, item.Cash);
            Assert.AreEqual(new DateTime(2006, 9, 21), item.Date);
            Assert.AreEqual("2006-09-21(每十股 红利1.80元 )", item.Description);
            Assert.AreEqual(0.0, item.Dispatch);
            Assert.AreEqual(new DateTime(2006, 9, 21), item.ExdividendDate);
            Assert.AreEqual(new DateTime(1, 1, 1), item.ListingDate);
            Assert.AreEqual(0.0, item.Price);
            Assert.AreEqual(new DateTime(2006, 9, 20), item.RegisterDate);
            Assert.AreEqual(0.0, item.Split);

            item = items[12];
            Assert.AreEqual(0.0, item.Bonus);
            Assert.AreEqual(0.42, item.Cash);
            Assert.AreEqual(new DateTime(2012, 6, 7), item.Date);
            Assert.AreEqual("2012-06-07(每十股 红利4.20元 )", item.Description);
            Assert.AreEqual(0.0, item.Dispatch);
            Assert.AreEqual(new DateTime(2012, 6, 7), item.ExdividendDate);
            Assert.AreEqual(new DateTime(1, 1, 1), item.ListingDate);
            Assert.AreEqual(0.0, item.Price);
            Assert.AreEqual(new DateTime(2012, 6, 6), item.RegisterDate);
            Assert.AreEqual(0.0, item.Split);
        }
    }
}
