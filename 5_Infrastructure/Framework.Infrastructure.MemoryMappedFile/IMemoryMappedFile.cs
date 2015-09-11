using System;

namespace Framework.Infrastructure.MemoryMappedFile
{
    public interface IMemoryMappedFile : IDisposable
    {
        IMemoryMappedFileHeader Header { get; }
    }
}
