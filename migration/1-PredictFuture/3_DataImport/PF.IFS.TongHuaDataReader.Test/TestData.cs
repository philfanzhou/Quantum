using System;
using System.IO;

namespace PF.IFS.TongHuaDataReader.Test
{
    internal class TestData
    {
        public static string DividendFile
        {
            get { return Path.Combine(Environment.CurrentDirectory, @"TestData\权息资料.财经"); }
        }

        public static string ShanghaiDay
        {
            get { return Path.Combine(Environment.CurrentDirectory, @"TestData\ShangHaiDay\"); }
        }

        public static string ShenzhenDay
        {
            get { return Path.Combine(Environment.CurrentDirectory, @"TestData\ShenzhenDay\"); }
        }
    }
}
