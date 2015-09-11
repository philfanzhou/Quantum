
using Framework.Infrastructure.MemoryMappedFile;

namespace Quantum.Infrastructure.MarketData.Metadata
{
    public struct MemoryMappedFileHeader : IMemoryMappedFileHeader
    {
        public int DataCount { get; set; }

        public int MaxDataCount { get; set; }
    }

}
