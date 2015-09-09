using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Infrastructure.MarketData.MMF;
using Quantum.Infrastructure.MarketData.Metadata;
using System.IO;

namespace Quantum.MarketData.Test
{
    [TestClass]
    public class MemoryMappedFileTest
    {
        [TestMethod]
        public void TestCanNotOverWriteFile()
        {
            string path = Environment.CurrentDirectory + @"\testOverWrite.dat";
            using (RealTimeFile.Create(path))
            { }

            Type exceptionType = null;
            try
            {
                using (RealTimeFile.Create(path))
                { }
            }
            catch(Exception e)
            {
                exceptionType = e.GetType();
            }
            finally
            {
                Assert.AreEqual(typeof(IOException), exceptionType);
                DeleteFile(path);
            }
        }

        [TestMethod]
        public void TestWriteAndRead()
        {
            string path = Environment.CurrentDirectory + @"\testReadAndWrite.dat";

            var source = CreateOneRealTimeItem();
            using(var file = RealTimeFile.Create(path))
            {
                file.Add(source);
            }

            RealTimeItem target;
            using(var file = new RealTimeFile(path))
            {
                target = file.Read();
            }

            Assert.AreEqual(source, target);
            target.BuyOneVolume = 1111;
            Assert.AreNotEqual(source, target);

            DeleteFile(path);
        }

        private RealTimeItem CreateOneRealTimeItem()
        {
            return new RealTimeItem
            {
                TodayOpen = 13.52,
                YesterdayClose = 13.11,
                Price = 18.88,
                High = 19.36,
                Low = 13.52,
                Volume = 3896453,
                Amount = 2345867,
                Time = new DateTime(2015, 9, 9, 14, 13, 42),
                SellFivePrice = 17.45,
                SellFiveVolume = 45323,
                SellFourPrice = 17.44,
                SellFourVolume = 1245,
                SellThreePrice = 17.43,
                SellThreeVolume = 23423,
                SellTwoPrice = 17.42,
                SellTwoVolume = 2343,
                SellOnePrice = 17.41,
                SellOneVolume = 7856,
                BuyFivePrice = 18.99,
                BuyFiveVolume = 33456422,
                BuyFourPrice = 19.00,
                BuyFourVolume = 22412532,
                BuyThreePrice = 20.00,
                BuyThreeVolume = 324353252,
                BuyTwoPrice = 21.00,
                BuyTwoVolume = 98273242,
                BuyOnePrice = 29.99,
                BuyOneVolume = 127412431514
            };
        }

        private void DeleteFile(string path)
        {
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
