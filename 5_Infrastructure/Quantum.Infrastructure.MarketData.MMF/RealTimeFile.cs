using Quantum.Infrastructure.MarketData.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class RealTimeFile : MarketDataMmf<RealTimeItem, MarketDataHeader>
    {
        private static string _mapName = "RealTimeFile";
        public const int MaxDataCount = 100;

        public RealTimeFile(string path)
            : base(path, _mapName)
        { }

        protected RealTimeFile(string path, int maxDataCount)
            : base(path, _mapName, maxDataCount) { }

        public static RealTimeFile Create(string path)
        {
            return new RealTimeFile(path, MaxDataCount);
        }

        public static string GetFilePath(string code, DateTime date)
        {
            throw new NotImplementedException();
        }

        public void Get(DateTime begin, DateTime end)
        {

        }
    }
}
