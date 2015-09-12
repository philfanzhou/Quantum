using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    [TestClass]
    public class MemoryMappedFileTest
    {
        #region Private Method
        private string CreateFileAnyway(int maxDataCount)
        {
            string path = Environment.CurrentDirectory + @"\" + "TestMemoryMappedFile.dat";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (DataFile.Create(path, maxDataCount))
            { }

            return path;
        }

        private List<DataItem> CreateRandomRealTimeItem(int count, int intervalSecond)
        {
            List<DataItem> result = new List<DataItem>();
            Random random = new Random();
            DateTime time = DateTime.Now;

            for (int i = 0; i < count; i++)
            {
                #region Create data item
                var tempItem = new DataItem
                {
                    IntData = random.Next(),
                    FloatData = float.Parse(Math.Round(random.NextDouble(), 2).ToString()),
                    DoubleData = Math.Round(random.NextDouble(), 2),
                    DecimalData = Convert.ToDecimal(Math.Round(random.NextDouble(), 2)),
                    LongData = random.Next(),
                    OtherStruct = new DataItem2
                    {
                        IntData = random.Next(),
                        DoubleData = Math.Round(random.NextDouble(), 2),
                    },
                    Amount = random.Next(),
                    Time = time,
                };
                #endregion
                result.Add(tempItem);
                time = time.AddSeconds(intervalSecond);
            }

            return result;
        }

        private static void CompareListItem(List<DataItem> expectedList, List<DataItem> actualList)
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

                actual.DoubleData = random.NextDouble();
                Assert.AreNotEqual(expected, actual);
            }
        }

        private static List<DataItem> ReadAllDataFromFile(string path)
        {
            List<DataItem> actualList = new List<DataItem>();
            using (var file = DataFile.Open(path))
            {
                actualList.AddRange(file.ReadAll());
            }
            return actualList;
        }

        private List<DataItem> AddDataToFile(int maxDataCount, string path)
        {
            int intervalSecond = 5; // 每条数据的时间间隔为5S
            List<DataItem> expectedList = CreateRandomRealTimeItem(maxDataCount, intervalSecond);

            // Open and add date
            using (var file = DataFile.Open(path))
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
            string path = CreateFileAnyway(1);

            // Can not create again use same path or overwrite file.
            using (DataFile.Create(path, 1))
            { }
        }

        [TestMethod]
        public void TestAddOneAndReadOne()
        {
            // Create file
            string path = CreateFileAnyway(1000);

            int dataCount = 100;
            int intervalSecond = 5; // 每条数据的时间间隔为5S
            List<DataItem> expectedList = CreateRandomRealTimeItem(dataCount, intervalSecond);

            // Open and add date
            using (var file = DataFile.Open(path))
            {
                foreach (var item in expectedList)
                {
                    file.Add(item);
                }
            }

            List<DataItem> actualList = new List<DataItem>();
            using (var file = DataFile.Open(path))
            {
                for (int i = 0; i < dataCount; i++)
                {
                    actualList.Add(file.Read(i));
                }
            }

            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestAddArray()
        {
            string path = CreateFileAnyway(100);

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
            string path = CreateFileAnyway(100);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            expectedList.RemoveAt(dataIndex);

            // Open and delete
            using (var file = DataFile.Open(path))
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
            string path = CreateFileAnyway(maxDataCount);

            int dataCount = maxDataCount;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            int removeDataCount = maxDataCount - dataIndex;
            expectedList.RemoveRange(dataIndex, removeDataCount);

            // Open and delete
            using (var file = DataFile.Open(path))
            {
                file.Delete(dataIndex, removeDataCount);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestDeleteAll()
        {
            int maxDataCount = 20;
            string path = CreateFileAnyway(maxDataCount);

            int dataCount = maxDataCount;
            var expectedList = AddDataToFile(dataCount, path);

            using (var file = DataFile.Open(path))
            {
                file.DeleteAll();
            }

            var actualList = ReadAllDataFromFile(path);

            Assert.AreEqual(0, actualList.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // Create file
            string path = CreateFileAnyway(100);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var expected = expectedList[dataIndex];
            expected.Amount = 300;
            expectedList[dataIndex] = expected;

            // Open and update
            using (var file = DataFile.Open(path))
            {
                file.Update(expected, dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestUpdateArray()
        {
            int maxDataCount = 100;
            string path = CreateFileAnyway(maxDataCount);

            int dataCount = maxDataCount;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 5;
            int updateDataCount = 20;
            int intervalSecond = 5; // 每条数据的时间间隔为5S
            List<DataItem> updateList = CreateRandomRealTimeItem(updateDataCount, intervalSecond);

            int j = 0;
            for(int i = dataIndex; i < dataIndex + updateDataCount; i++)
            {
                expectedList[i] = updateList[j++];
            }

            // Open and update
            using (var file = DataFile.Open(path))
            {
                file.Update(updateList, dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestInsert()
        {
            // Create file
            string path = CreateFileAnyway(50);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var addDataItems = CreateRandomRealTimeItem(10, 5);
            expectedList.InsertRange(dataIndex, addDataItems);

            // Open and insert
            using (var file = DataFile.Open(path))
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
            string path = CreateFileAnyway(20);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            int dataIndex = 3;
            var addDataItems = CreateRandomRealTimeItem(10, 5);
            expectedList.InsertRange(dataIndex, addDataItems);

            // Open and insert
            using (var file = DataFile.Open(path))
            {
                file.Insert(addDataItems, dataIndex);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }
    }
}
