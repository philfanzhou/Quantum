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
        #region Private Method
        private string CreateFileAnyway(string fileName, int maxDataCount)
        {
            string path = Environment.CurrentDirectory + @"\" + fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (RealTimeFile.Create(path, maxDataCount))
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

        private static void CompareListItem(List<RealTimeItem> expectedList, List<RealTimeItem> actualList)
        {
            Assert.IsNotNull(actualList);
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

        private static List<RealTimeItem> ReadAllDataFromFile(string path)
        {
            List<RealTimeItem> actualList = new List<RealTimeItem>();
            using (var file = RealTimeFile.Open(path))
            {
                actualList.AddRange(file.ReadAll());
            }
            return actualList;
        }

        private List<RealTimeItem> AddDataToFile(int maxDataCount, string path)
        {
            int intervalSecond = 5; // 每条数据的时间间隔为5S
            List<RealTimeItem> expectedList = CreateRandomRealTimeItem(maxDataCount, intervalSecond);

            // Open and add date
            using (var file = RealTimeFile.Open(path))
            {
                file.Add(expectedList);
            }
            return expectedList;
        }
        #endregion

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void TestCanNotOverWriteFile()
        {
            string path = CreateFileAnyway("testOverWrite.dat", 1);

            // Can not create again use same path or overwrite file.
            using (RealTimeFile.Create(path, 1))
            { }
        }

        [TestMethod]
        public void TestAdd()
        {
            // Create file
            string path = CreateFileAnyway("testAdd.dat", 1000);

            int dataCount = 100;
            var expectedList = AddDataToFile(dataCount, path);

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestAddArray()
        {
            string path = CreateFileAnyway("testAddArray.dat", 100);

            int dataCount = 20;
            var expectedList = AddDataToFile(dataCount, path);
            expectedList.AddRange(AddDataToFile(dataCount, path));

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestDelete()
        {
            // Create file
            string path = CreateFileAnyway("testDelete.dat", 100);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            expectedList.RemoveAt(dataIndex);

            // Open and delete
            using (var file = RealTimeFile.Open(path))
            {
                file.Delete(dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestDeleteArray()
        {
            int maxDataCount = 5;
            string path = CreateFileAnyway("testDeleteArray.dat", maxDataCount);

            int dataCount = maxDataCount;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            int removeDataCount = maxDataCount - dataIndex;
            expectedList.RemoveRange(dataIndex, removeDataCount);

            // Open and delete
            using (var file = RealTimeFile.Open(path))
            {
                file.Delete(dataIndex, removeDataCount);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // Create file
            string path = CreateFileAnyway("testUpdate.dat", 100);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var expected = expectedList[dataIndex];
            expected.Amount = 300;
            expectedList[dataIndex] = expected;

            // Open and update
            using (var file = RealTimeFile.Open(path))
            {
                file.Update(expected, dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestInsert()
        {
            // Create file
            string path = CreateFileAnyway("testInsert.dat", 50);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var addDataItems = CreateRandomRealTimeItem(10, 5);
            expectedList.InsertRange(dataIndex, addDataItems);

            // Open and insert
            using (var file = RealTimeFile.Open(path))
            {
                foreach (var dataItem in addDataItems)
                {
                    file.Insert(dataItem, dataIndex++);
                }
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestInsertArray()
        {
            string path = CreateFileAnyway("testInsertArray.dat", 20);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var addDataItems = CreateRandomRealTimeItem(10, 5);
            expectedList.InsertRange(dataIndex, addDataItems);

            // Open and insert
            using (var file = RealTimeFile.Open(path))
            {
                file.Insert(addDataItems, dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }
    }
}
