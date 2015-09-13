using System;
using System.Runtime.InteropServices;

namespace Framework.Infrastructure.MemoryMappedFile.Test
{
    public struct FileHeader : IMemoryMappedFileHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        private string comment;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        //private byte[] comment;

        public string Comment
        {
            //get
            //{
            //    return System.Text.Encoding.GetEncoding("GBK").GetString(comment);
            //}
            //set
            //{
            //    byte[] data = System.Text.Encoding.GetEncoding("GBK").GetBytes(value);
            //    if (data.Length > 20)
            //    {
            //        throw new ArgumentOutOfRangeException();
            //    }
            //    this.comment = new byte[20];
            //    data.CopyTo(comment, 0);
            //}
            get { return this.comment; }
            set { this.comment = value; }
        }
    }

    public struct DataItem
    {
        public int IntData { get; set; }

        public float FloatData { get; set; }

        public double DoubleData { get; set; }

        public decimal DecimalData { get; set; }

        public long LongData { get; set; }

        public DataItem2 OtherStruct { get; set; }

        public double Amount { get; set; }

        public DateTime Time { get; set; }

        public override string ToString()
        {
            return this.DoubleData.ToString();
        }
    }

    public struct DataItem2
    {
        public int IntData { get; set; }

        public double DoubleData { get; set; }
    }

    public class DataFile
        : MemoryMappedFileBase<FileHeader, DataItem>
    {
        private DataFile() { }
    }
}
