using Framework.Infrastructure.MemoryMappedFile;
using System;
using System.IO;

namespace Quantum.Infrastructure.MarketData.Repository
{
    internal class RealTimeFile
        : MemoryMappedFileBase<RealTimeFileHeader, RealTimeItem>
    {
        private RealTimeFile() { }

        internal static int GetMaxDataCount()
        {
            // 按每月最大数据量处理
            throw new System.NotImplementedException();
        }

        internal static bool Exist(string stockCode, MarketType marketType, DateTime month)
        {
            string path = GetFilePath(stockCode, marketType, month);
            return File.Exists(path);
        }

        internal static IMemoryMappedFile<RealTimeFileHeader, RealTimeItem> Open(string stockCode, MarketType marketType, DateTime month)
        {
            string path = GetFilePath(stockCode, marketType, month);
            return Open(path);
        }

        internal static IMemoryMappedFile<RealTimeFileHeader, RealTimeItem> Create(string stockCode, MarketType marketType, DateTime month)
        {
            RealTimeFileHeader heaer = new RealTimeFileHeader
            {
                DataCount = 0,
                MaxDataCount = GetMaxDataCount(),
                MarketType = marketType,
                DataType = DataType.RealTime,
                StartDay = new DateTime(month.Year, month.Month, 1),
                EndDay = new DateTime(month.Year, month.Month, 1).AddMonths(1).AddDays(-1)
            };

            string path = GetFilePath(stockCode, marketType, month);
            return Create(path, heaer);
        }

        private static string GetFilePath(string stockCode, MarketType marketType, DateTime month)
        {
            throw new System.NotImplementedException();
        }
    }

    internal struct RealTimeFileHeader : IMemoryMappedFileHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }

        public MarketType MarketType { get; set; }

        public DataType DataType { get; set; }

        public DateTime StartDay { get; set; }

        public DateTime EndDay { get; set; }
    }
}
