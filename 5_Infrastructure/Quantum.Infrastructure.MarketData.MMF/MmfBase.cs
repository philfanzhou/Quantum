using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class MmfBase : IDisposable
    {
        protected MemoryMappedFile Mmf;
        protected readonly string Path;
        protected readonly string MapName;

        #region Constructor

        public MmfBase(string path, string mapName)
        {
            this.Mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open, mapName);
            this.Path = path;
            this.MapName = mapName;
        }

        /// <summary>
        /// 子类调用，用于创建映射文件
        /// </summary>
        protected MmfBase(string path, string mapName, long capacity)
        {
            // FileMode一定要使用CreateNew，否则可能出现覆盖文件的情况
            var mmf = MemoryMappedFile.CreateFromFile(path, FileMode.CreateNew, mapName, capacity);
            this.Mmf = mmf;
            this.Path = path;
            this.MapName = mapName;
        }

        #endregion

        #region IDisposable Member

        protected bool Disposed;

        ~MmfBase()
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
                if (Mmf != null)
                {
                    Mmf.Dispose();
                    Mmf = null;
                }
            }

            Disposed = true;
        }

        #endregion

        #region Override

        public override string ToString()
        {
            ThrowIfDisposed();

            return this.Path;
        }

        #endregion
    }
}
