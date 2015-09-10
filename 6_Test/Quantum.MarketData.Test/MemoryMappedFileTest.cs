using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Infrastructure.MarketData.MMF;
using Quantum.Infrastructure.MarketData.Metadata;
using System.IO;
using System.Collections.Generic;

namespace Quantum.MarketData.Test
{
    [TestClass]
    public class MemoryMappedFileTest
    {
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void TestCanNotOverWriteFile()
        {
            string path = CreateFileAnyway("testOverWrite.dat");

            // Can not create again use same path or overwrite file.
            using (RealTimeFile.Create(path))
            { }
        }

        [TestMethod]
        public void TestWriteAndRead()
        {
            // Create file
            string path = CreateFileAnyway("testReadAndWrite.dat");

            int maxDataCount;
            int intervalSecond = 5;// 每条数据的时间间隔为5S
            List<RealTimeItem> expectedList;

            // Open and add date
            using (var file = new RealTimeFile(path))
            {
                // Create source data
                maxDataCount = file.MaxDataCount;
                expectedList = CreateRandomRealTimeItem(maxDataCount, intervalSecond);

                foreach (var item in expectedList)
                {
                    file.Add(item);
                }
            }

            // Open and read data
            List<RealTimeItem> actualList = new List<RealTimeItem>();
            using (var file = new RealTimeFile(path))
            {
                for (int i = 0; i < maxDataCount; i++)
                {
                    actualList.Add(file.Read(i));
                }
            }

            // Compare expected and actual
            Assert.AreNotEqual(0, expectedList.Count);
            Assert.AreNotEqual(0, actualList.Count);

            Random random = new Random();
            for (int i = 0; i < maxDataCount; i++)
            {
                var expected = expectedList[i];
                var actual = actualList[i];

                Assert.AreEqual(expected, actual);

                actual.BuyOneVolume = random.Next();
                Assert.AreNotEqual(expected, actual);
            }
        }

        private string CreateFileAnyway(string fileName)
        {
            string path = Environment.CurrentDirectory + @"\" + fileName;

            if(File.Exists(path))
            {
                File.Delete(path);
            }

            using (RealTimeFile.Create(path))
            { }

            return path;
        }

        private List<RealTimeItem> CreateRandomRealTimeItem(int count, int intervalSecond)
        {
            List<RealTimeItem> result = new List<RealTimeItem>();
            Random random = new Random();
            DateTime time = DateTime.Now;

            for (int i = 0; i < count; i++)
            {
                #region Create data item
                var tempItem = new RealTimeItem
                {
                    TodayOpen = Math.Round(random.NextDouble(), 2),
                    YesterdayClose = Math.Round(random.NextDouble(), 2),
                    Price = Math.Round(random.NextDouble(), 2),
                    High = Math.Round(random.NextDouble(), 2),
                    Low = Math.Round(random.NextDouble(), 2),
                    Volume = random.Next(),
                    Amount = random.Next(),
                    Time = time,
                    SellFivePrice = Math.Round(random.NextDouble(), 2),
                    SellFiveVolume = random.Next(),
                    SellFourPrice = Math.Round(random.NextDouble(), 2),
                    SellFourVolume = random.Next(),
                    SellThreePrice = Math.Round(random.NextDouble(), 2),
                    SellThreeVolume = random.Next(),
                    SellTwoPrice = Math.Round(random.NextDouble(), 2),
                    SellTwoVolume = random.Next(),
                    SellOnePrice = Math.Round(random.NextDouble(), 2),
                    SellOneVolume = random.Next(),
                    BuyFivePrice = Math.Round(random.NextDouble(), 2),
                    BuyFiveVolume = random.Next(),
                    BuyFourPrice = Math.Round(random.NextDouble(), 2),
                    BuyFourVolume = random.Next(),
                    BuyThreePrice = Math.Round(random.NextDouble(), 2),
                    BuyThreeVolume = random.Next(),
                    BuyTwoPrice = Math.Round(random.NextDouble(), 2),
                    BuyTwoVolume = random.Next(),
                    BuyOnePrice = Math.Round(random.NextDouble(), 2),
                    BuyOneVolume = random.Next(),
                };
                #endregion
                result.Add(tempItem);
                time = time.AddSeconds(intervalSecond);
            }

            return result;
        }
    }
}
