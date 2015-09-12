using Framework.Infrastructure.MemoryMappedFile;
using Quantum.Infrastructure.MarketData.Metadata;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class RealTimeFile
        : MemoryMappedFileBase<MemoryMappedFileHeader, RealTimeItem>
    {
        private RealTimeFile() { }
    }
}
