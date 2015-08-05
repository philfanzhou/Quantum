using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PF.IFS.TongHua.DataReader
{
    internal class DayLineFile : FileBase
    {
        public DayLineFile(string filePath)
            : base(filePath)
        {
        }

        public string GetStockSymbol()
        {
            return Path.GetFileNameWithoutExtension(base.FilePaht);
        }

        public IKlineData GetData(DateTime startTime)
        {
            DayLineInfo info = new DayLineInfo();
            info.Symbol = this.GetStockSymbol();
            info.Items = this.GetKlineData(startTime);

            return info;
        }

        public List<IKlineItem> GetKlineData(DateTime startTime)
        {
            List<IKlineItem> result = new List<IKlineItem>();

            using (FileStream stream = File.OpenRead(base.FilePaht))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    THFileHeader header = StructUtil<THFileHeader>.BytesToStruct(reader.ReadBytes(THFileHeader.StructSize));
                    StructUtil<THColumnHeader>.ReadStructArray(reader, header.FieldCount);

                    if (header.RecordLength == 164)
                    {
                        //读取板块K线数据
                        THKLineMarket[] marketData = StructUtil<THKLineMarket>.ReadStructArray(reader,
                                                                                               header.RecordCount);
                        result.AddRange(marketData.Where(d => d.Date > startTime));
                    }
                    else if (header.RecordLength == 168)
                    {
                        //读取个股K线数据
                        THKLineStock[] stockData = StructUtil<THKLineStock>.ReadStructArray(reader, header.RecordCount);
                        result.AddRange(stockData.Where(d => d.Date > startTime));
                    }
                }
            }

            return result;
        }
    }
}