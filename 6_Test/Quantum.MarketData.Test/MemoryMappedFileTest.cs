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
                maxDataCount = file.Header.MaxDataCount;
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

        [TestMethod]
        public void TestUpdate()
        {
            // Create file
            string path = CreateFileAnyway("testUpdate.dat");

            int maxDataCount = 10;
            int intervalSecond = 5;// 每条数据的时间间隔为5S
            List<RealTimeItem> dataSourceList = CreateRandomRealTimeItem(maxDataCount, intervalSecond); ;

            // Open and add date
            using (var file = new RealTimeFile(path))
            {
                foreach (var item in dataSourceList)
                {
                    file.Add(item);
                }
            }

            int dataIndex = 3;
            var expected = dataSourceList[dataIndex];
            expected.Amount = 300;

            // Open and update
            using (var file = new RealTimeFile(path))
            {
                file.Update(expected, dataIndex);
            }

            // Open and read
            RealTimeItem actual;
            using (var file = new RealTimeFile(path))
            {
                actual = file.Read(dataIndex);
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestDelete()
        {
            // Create file
            string path = CreateFileAnyway("testDelete.dat");

            int maxDataCount = 10;
            int intervalSecond = 5;// 每条数据的时间间隔为5S
            List<RealTimeItem> expectedList = CreateRandomRealTimeItem(maxDataCount, intervalSecond);

            // Open and add date
            using (var file = new RealTimeFile(path))
            {
                foreach (var item in expectedList)
                {
                    file.Add(item);
                }
            }

            int dataIndex = 3;
            expectedList.RemoveAt(dataIndex);

            // Open and delete
            using (var file = new RealTimeFile(path))
            {
                file.Delete(dataIndex);
            }

            // Open and read data
            List<RealTimeItem> actualList = new List<RealTimeItem>();
            using (var file = new RealTimeFile(path))
            {
                for (int i = 0; i < file.Header.DataCount; i++)
                {
                    actualList.Add(file.Read(i));
                }
            }

            // Compare expected and actual
            Assert.AreNotEqual(0, expectedList.Count);
            Assert.AreNotEqual(0, actualList.Count);
            Assert.AreEqual(expectedList.Count, actualList.Count);

            Random random = new Random();
            for (int i = 0; i < expectedList.Count; i++)
            {
                var expected = expectedList[i];
                var actual = actualList[i];

                Assert.AreEqual(expected, actual);

                actual.BuyOneVolume = random.Next();
                Assert.AreNotEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TestInsert()
        {
            // Create file
            string path = CreateFileAnyway("testDelete.dat");

            int maxDataCount = 10;
            int intervalSecond = 5;// 每条数据的时间间隔为5S
            List<RealTimeItem> expectedList = CreateRandomRealTimeItem(maxDataCount - 1, intervalSecond);

            // Open and add date
            using (var file = new RealTimeFile(path))
            {
                foreach (var item in expectedList)
                {
                    file.Add(item);
                }
            }

            int dataIndex = 3;
            var addDataItem = CreateRandomRealTimeItem(1, intervalSecond)[0];
            expectedList.Insert(dataIndex, addDataItem);

            // Open and insert
            using (var file = new RealTimeFile(path))
            {
                file.Insert(addDataItem, dataIndex);
            }

            // Open and read data
            List<RealTimeItem> actualList = new List<RealTimeItem>();
            using (var file = new RealTimeFile(path))
            {
                for (int i = 0; i < file.Header.DataCount; i++)
                {
                    actualList.Add(file.Read(i));
                }
            }

            // Compare expected and actual
            Assert.AreNotEqual(0, expectedList.Count);
            Assert.AreNotEqual(0, actualList.Count);
            Assert.AreEqual(expectedList.Count, actualList.Count);

            Random random = new Random();
            for (int i = 0; i < expectedList.Count; i++)
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
