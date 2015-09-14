using Framework.Infrastructure.MemoryMappedFile;

namespace Quantum.Infrastructure.MarketData.Repository
{
    public class RealTimeFile
        : MemoryMappedFileBase<RealTimeFileHeader, RealTimeItem>
    {
        private RealTimeFile() { }
    }

    public struct RealTimeFileHeader : IMemoryMappedFileHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }
    }
}
