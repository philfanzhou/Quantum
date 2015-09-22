using Framework.Infrastructure.MemoryMappedFile;
using System;
using System.Collections;
using System.IO;

namespace Quantum.Infrastructure.MarketData.Repository
{
    internal class RealTimeFile
        : MemoryMappedFileBase<RealTimeFileHeader, RealTimeItem>
    {
        private static string dataFolder = Environment.CurrentDirectory   + @"\Data\RealTimeData\";

        private static readonly TimeSpan TimeSpan = new TimeSpan(0, 0, 4);

        /// <summary>
        /// (10缓冲 + 15集合竞价 + 4小时交易 + 10缓冲)每分钟12条记录
        /// </summary>
        private static int maxDataCount = (10 + 15 + 4 * 60 + 10) * 12;

        private RealTimeFile() { }

        #region Create and Open

        private static string GetFilePath(MarketType marketType, string stockCode, DateTime day)
        {
            string path = dataFolder;
            if (marketType == MarketType.Shanghai)
            {
                path += @"Shanghai\";
            }
            else if (marketType == MarketType.Shenzhen)
            {
                path += @"Shenzhen\";
            }

            path += stockCode + @"\";
            path += day.ToString("yyyyMMdd") + ".dat";

            return path;
        }

        internal static bool Exist(MarketType marketType, string stockCode, DateTime day)
        {
            string path = GetFilePath(marketType, stockCode, day);
            return File.Exists(path);
        }

        internal static IMemoryMappedFile<RealTimeFileHeader, RealTimeItem> Open(MarketType marketType, string stockCode, DateTime day)
        {
            string path = GetFilePath(marketType, stockCode, day);
            return Open(path);
        }

        internal static IMemoryMappedFile<RealTimeFileHeader, RealTimeItem> Create(MarketType marketType, string stockCode, DateTime day)
        {
            RealTimeFileHeader heaer = new RealTimeFileHeader
            {
                DataCount = 0,
                MaxDataCount = maxDataCount,
                MarketType = marketType,
                DataType = DataType.RealTime,
                StartDay = day,
                EndDay = day
            };

            string path = GetFilePath(marketType, stockCode, day);
            return Create(path, heaer);
        }

        internal static IMemoryMappedFile<RealTimeFileHeader, RealTimeItem> CreateOrOpen(MarketType marketType, string stockCode, DateTime day)
        {
            if (Exist(marketType, stockCode, day))
            {
                return Open(marketType, stockCode, day);
            }
            else
            {
                return Create(marketType, stockCode, day);
            }
        }

        #endregion

        public override void Add(RealTimeItem item)
        {
            //var lastItem = this.Read(this.Header.DataCount - 1);

            //if(item.Time - lastItem.Time > TimeSpan)
            //{
                base.Add(item);
            //}
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
