using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class MarketDataMemoryMappedFile : IDisposable
    {
        protected MemoryMappedFile mmf;
        protected readonly string path;
        protected readonly string mapName;
        protected readonly long capacity;

        #region Constructor
        
        public MarketDataMemoryMappedFile(string path)
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
        protected MarketDataMemoryMappedFile(string path, string mapName, long capacity)
        {
            // FileMode一定要使用CreateNew，否则可能出现覆盖文件的情况
            var mmf = MemoryMappedFile.CreateFromFile(
                path, FileMode.CreateNew, mapName, capacity);

            this.mmf = mmf;
            this.path = path;
            this.mapName = mapName;
            this.capacity = capacity;
        }

        #endregion

        #region Property

        public string FullPath
        {
            get { return this.path; }
        }

        #endregion

        #region Override

        public override string ToString()
        {
            return this.path;
        }

        #endregion

        #region IDisposable Member

        protected bool Disposed;

        ~MarketDataMemoryMappedFile()
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
