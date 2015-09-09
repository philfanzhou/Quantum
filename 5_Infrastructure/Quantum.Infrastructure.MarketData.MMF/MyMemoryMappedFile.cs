using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class MyMemoryMappedFile<TDataItem, TDataHeader> : IDisposable
        where TDataItem : struct
        where TDataHeader : struct
    {
        protected MemoryMappedFile mmf;
        protected readonly string path;
        protected readonly string mapName;
        protected readonly long capacity;

        #region Constructor
        
        public MyMemoryMappedFile(string path)
        {
            this.mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open);
            this.path = path;
        }

        /// <summary>
        /// 子类调用，用于创建映射文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mapName"></param>
        /// <param name="capacity"></param>
        protected MyMemoryMappedFile(string path, string mapName, long dataCapacity)
        {
            this.path = path;
            this.mapName = mapName;
            this.capacity = dataCapacity + Marshal.SizeOf(typeof(TDataHeader));

            // FileMode一定要使用CreateNew，否则可能出现覆盖文件的情况
            var mmf = MemoryMappedFile.CreateFromFile(
                path, FileMode.CreateNew, mapName, this.capacity);
            this.mmf = mmf;

            UpdateHeaer(new TDataHeader());
        }

        #endregion

        #region Property

        public string FullPath
        {
            get { return this.path; }
        }

        #endregion

        private void UpdateHeaer(TDataHeader header)
        {
            int headerSize = Marshal.SizeOf(header.GetType());
            using (var accessor = mmf.CreateViewAccessor(0, headerSize))
            {
                accessor.Write(0, ref header);
            }
        }

        public void Add(TDataItem item)
        {
            using (var accessor = mmf.CreateViewAccessor(0, this.capacity))
            {
                int itemSize = Marshal.SizeOf(item.GetType());
                accessor.Write(Marshal.SizeOf(typeof(TDataHeader)), ref item);
            }
        }

        public TDataItem Read()
        {
            TDataItem result;

            using (var accessor = mmf.CreateViewAccessor(0, this.capacity))
            {
                int itemSize = Marshal.SizeOf(typeof(TDataItem));
                accessor.Read(Marshal.SizeOf(typeof(TDataHeader)), out result);
            }
            return result;
        }

        #region Override

        public override string ToString()
        {
            return this.path;
        }

        #endregion

        #region IDisposable Member

        protected bool Disposed;

        ~MyMemoryMappedFile()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if this object has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        protected void ThrowIfDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("MarketDataMemoryMappedFile", "MarketDataMemoryMappedFile has been disposed.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                // Clean up managed resources
                if (mmf != null)
                {
                    mmf.Dispose();
                    mmf = null;
                }
            }

            Disposed = true;
        }

        #endregion
    }
}
