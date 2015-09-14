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
        private string CreateFileAnyway(string fileName, int maxDataCount)
        {
            string path = Environment.CurrentDirectory + @"\" + fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileHeader header = new FileHeader();
            header.MaxDataCount = maxDataCount;
            header.Comment = "招商银行";
            using (DataFile.Create(path, header))
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

        #region Constructor

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestCannotBeAccessAfterDisposed()
        {
            string path = CreateFileAnyway("TestCannotBeAccessAfterDisposed.dat", 1000);

            var file = DataFile.Open(path);
            file.Dispose();

            file.DeleteAll();
        }

        [TestMethod]
        public void TestToString()
        {
            string path = CreateFileAnyway("TestToString.dat", 1000);
            using(var file = DataFile.Open(path))
            {
                Assert.AreEqual(path, file.ToString());
            }
        }

        [TestMethod]
        public void TestFileHeader1()
        {
            string path = Environment.CurrentDirectory + @"\" + "TestFileHeader1.dat";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            int maxDataCount = 10;
            string comment = "招商银行";
            FileHeader header = new FileHeader();
            header.MaxDataCount = maxDataCount;
            header.Comment = comment;
            using (DataFile.Create(path, header))
            { }

            FileHeader readedHeader; 
            using (var file = DataFile.Open(path))
            {
                readedHeader = (FileHeader)file.Header;
            }

            //Assert.AreEqual(header, readedHeader);
            Assert.AreEqual(maxDataCount, readedHeader.MaxDataCount);
            Assert.AreEqual(comment, readedHeader.Comment);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFileHeader2()
        {
            string path = Environment.CurrentDirectory + @"\" + "TestFileHeader2.dat";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            int maxDataCount = 0;
            string comment = "招商银行";
            FileHeader header = new FileHeader();
            header.MaxDataCount = maxDataCount;
            header.Comment = comment;
            using (DataFile.Create(path, header))
            { }
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void TestCanNotOverWriteFile()
        {
            string path = CreateFileAnyway("TestCanNotOverWriteFile.dat", 1);

            // Can not create again use same path or overwrite file.
            using (DataFile.Create(path, new FileHeader()))
            { }
        }

        #endregion

        #region Read

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadArgumeng1()
        {
            int maxDataCount = 5;
            string path = CreateFileAnyway("TestReadArgumeng1.dat", maxDataCount);

            AddDataToFile(maxDataCount, path);
            using (var file = DataFile.Open(path))
            {
                file.Read(6, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadArgumeng2()
        {
            int maxDataCount = 5;
            string path = CreateFileAnyway("TestReadArgumeng2.dat", maxDataCount);

            AddDataToFile(maxDataCount, path);
            using (var file = DataFile.Open(path))
            {
                file.Read(-1, 2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadArgumeng3()
        {
            int maxDataCount = 5;
            string path = CreateFileAnyway("TestReadArgumeng3.dat", maxDataCount);

            AddDataToFile(maxDataCount, path);
            using (var file = DataFile.Open(path))
            {
                file.Read(1, -1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestReadArgumeng4()
        {
            int maxDataCount = 5;
            string path = CreateFileAnyway("TestReadArgumeng4.dat", maxDataCount);

            AddDataToFile(maxDataCount, path);
            using (var file = DataFile.Open(path))
            {
                file.Read(1, 5);
            }
        }

        #endregion

        #region Add

        [TestMethod]
        public void TestAddOneAndReadOne()
        {
            // Create file
            string path = CreateFileAnyway("TestAddOneAndReadOne.dat", 1000);

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
            string path = CreateFileAnyway("TestAddArray.dat", 100);

            int dataCount = 20;
            var expectedList = AddDataToFile(dataCount, path);
            expectedList.AddRange(AddDataToFile(dataCount, path));

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        [TestMethod]
        public void TestDelete1()
        {
            // Create file
            string path = CreateFileAnyway("TestDelete1.dat", 100);

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
        public void TestDelete2()
        {
            // Create file
            string path = CreateFileAnyway("TestDelete2.dat", 100);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);
            
            // Open and delete
            using (var file = DataFile.Open(path))
            {
                file.Delete(dataCount +5);
            }

            var actualList = ReadAllDataFromFile(path);
            CompareListItem(expectedList, actualList);
        }

        #endregion

        #region Delete

        [TestMethod]
        public void TestDeleteArray()
        {
            int maxDataCount = 5;
            string path = CreateFileAnyway("TestDeleteArray.dat", maxDataCount);

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
            string path = CreateFileAnyway("TestDeleteAll.dat", maxDataCount);

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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteArgument1()
        {
            int maxDataCount = 20;
            string path = CreateFileAnyway("TestDeleteArgument1.dat", maxDataCount);

            using (var file = DataFile.Open(path))
            {
                file.Delete(-1, 10);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteArgument2()
        {
            int maxDataCount = 20;
            string path = CreateFileAnyway("TestDeleteArgument2.dat", maxDataCount);

            using (var file = DataFile.Open(path))
            {
                file.Delete(0, 21);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteArgument3()
        {
            int maxDataCount = 20;
            string path = CreateFileAnyway("TestDeleteArgument3.dat", maxDataCount);

            using (var file = DataFile.Open(path))
            {
                file.Delete(5, 16);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteArgument4()
        {
            int maxDataCount = 20;
            string path = CreateFileAnyway("TestDeleteArgument4.dat", maxDataCount);

            using (var file = DataFile.Open(path))
            {
                file.Delete(21, 1);
            }
        }

        #endregion

        #region Update

        [TestMethod]
        public void TestUpdate()
        {
            // Create file
            string path = CreateFileAnyway("TestUpdate.dat", 100);

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
            string path = CreateFileAnyway("TestUpdateArray.dat", maxDataCount);

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

        #endregion

        #region Insert

        [TestMethod]
        public void TestInsert()
        {
            // Create file
            string path = CreateFileAnyway("TestInsert.dat", 50);

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
        public void TestInsertArray1()
        {
            string path = CreateFileAnyway("TestInsertArray1.dat", 20);

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

        [TestMethod]
        public void TestInsertArray2()
        {
            string path = CreateFileAnyway("TestInsertArray2.dat", 20);

            int dataCount = 10;
            AddDataToFile(dataCount, path);

            List<DataItem> dataList = CreateRandomRealTimeItem(5, 5);
            // Open and insert
            using (var file = DataFile.Open(path))
            {
                file.Insert(dataList, 12);
                Assert.AreEqual(12 + 5 - 1, file.Header.DataCount);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInsertArgument1()
        {
            string path = CreateFileAnyway("TestInsertArgument1.dat", 20);

            using (var file = DataFile.Open(path))
            {
                file.Insert(null, 0);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInsertArgument2()
        {
            string path = CreateFileAnyway("TestInsertArgument2.dat", 20);

            List<DataItem> expectedList = CreateRandomRealTimeItem(10, 5);
            using (var file = DataFile.Open(path))
            {
                file.Insert(expectedList, 21);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInsertArgument3()
        {
            string path = CreateFileAnyway("TestInsertArgument3.dat", 20);

            List<DataItem> expectedList = CreateRandomRealTimeItem(10, 5);
            using (var file = DataFile.Open(path))
            {
                file.Insert(expectedList, 18);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInsertArgument4()
        {
            string path = CreateFileAnyway("TestInsertArgument4.dat", 20);

            int dataCount = 10;
            var expectedList = AddDataToFile(dataCount, path);

            List<DataItem> dataList = CreateRandomRealTimeItem(12, 5);

            using (var file = DataFile.Open(path))
            {
                file.Insert(dataList, 8);
            }
        }

        #endregion
    }
}
