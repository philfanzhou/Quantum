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
    public class RealTimeFile : MyMemoryMappedFile<RealTimeItem, MyDataHeader>
    {
        public RealTimeFile(string path)
            : base(path)
        { }

        protected RealTimeFile(string path, string mapName, int maxDataCount)
            : base(path, mapName, maxDataCount) { }

        public static RealTimeFile Create(string path)
        {
            return new RealTimeFile(path, "realTimeFile", 100);
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
