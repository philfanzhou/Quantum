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
    public class RealTimeFile : MarketDataMemoryMappedFile
    {
        public RealTimeFile(string path)
            : base(path)
        { }

        protected RealTimeFile(string path, string mapName, long capacity)
            : base(path, mapName, capacity) { }

        public static RealTimeFile Create(string path)
        {
            return new RealTimeFile(path, "realTimeFile", 1000000);
        }

        public static string GetFilePath(string code, DateTime date)
        {
            throw new NotImplementedException();
        }

        public void Add(RealTimeItem item)
        {
            using (var accessor = mmf.CreateViewAccessor(0, this.capacity))
            {
                int itemSize = Marshal.SizeOf(item.GetType());
                accessor.Write(0, ref item);
            }
        }

        public RealTimeItem Read()
        {
            RealTimeItem result;
            
            using (var accessor = mmf.CreateViewAccessor(0, this.capacity))
            {
                int itemSize = Marshal.SizeOf(typeof(RealTimeItem));
                accessor.Read(0, out result);
            }
            return result;

        }

        public void Get(DateTime begin, DateTime end)
        {

        }
    }
}
