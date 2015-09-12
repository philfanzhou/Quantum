using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.MemoryMappedFile
{
    public interface IMemoryMappedFile<TDataHeader, TDataItem> : IDisposable
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
    {
        IMemoryMappedFileHeader Header { get; }

        void Add(TDataItem item);

        void Add(IEnumerable<TDataItem> items);

        void Delete(int index);

        void Delete(int index, int count);

        void DeleteAll();

        void Update(TDataItem item, int index);

        void Update(IEnumerable<TDataItem> items, int index);

        TDataItem Read(int index);

        IEnumerable<TDataItem> Read(int index, int count);

        IEnumerable<TDataItem> ReadAll();

        void Insert(TDataItem item, int index);

        void Insert(IEnumerable<TDataItem> items, int index);
    }
}
