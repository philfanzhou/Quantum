using Quantum.Infrastructure.MarketData.Metadata;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class RealTimeFile : MarketDataMmf<RealTimeItem, MarketDataHeader>
    {
        //protected RealTimeFile(string path)
        //    : base(path)
        //{ }

        protected RealTimeFile(string path, int maxDataCount)
            : base(path, maxDataCount) { }

        public static RealTimeFile Open(string path)
        {
            //return new RealTimeFile(path);
            throw new System.NotImplementedException();
        }

        public static RealTimeFile Create(string path, int maxDataCount)
        {
            return new RealTimeFile(path, maxDataCount);
        }
    }
}
