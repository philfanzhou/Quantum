using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class MyMemoryMappedFile<TDataItem, TDataHeader> : IDisposable
        where TDataItem : struct
        where TDataHeader : struct, IMmfDataHeader
    {
        #region Field

        /// <summary>
        /// 文件总长度
        /// </summary>
        private readonly long _capacity;
        /// <summary>
        /// 头文件长度
        /// </summary>
        private readonly long _headerSize = Marshal.SizeOf(typeof(TDataHeader));
        /// <summary>
        /// 单个数据长度
        /// </summary>
        private readonly long _dataItemSize = Marshal.SizeOf(typeof(TDataItem));

        protected readonly string Path;
        protected readonly string MapName;

        protected MemoryMappedFile Mmf;
        private TDataHeader _header;

        #endregion

        #region Constructor

        public MyMemoryMappedFile(string path)
        {
            this.Mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open);
            this.Path = path;

            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Read(0, out this._header);
            }
        }

        /// <summary>
        /// 子类调用，用于创建映射文件
        /// </summary>
        protected MyMemoryMappedFile(string path, string mapName, int maxDataCount)
        {
            this.Path = path;
            this.MapName = mapName;
            this._capacity = maxDataCount * this._dataItemSize + this._headerSize;

            // FileMode一定要使用CreateNew，否则可能出现覆盖文件的情况
            var mmf = MemoryMappedFile.CreateFromFile(
                path, FileMode.CreateNew, mapName, this._capacity);
            this.Mmf = mmf;

            // 创建文件之后要立即更新头，避免创建之后未加数据就关闭后，下次无法打开文件
            this._header.MaxDataCount = maxDataCount;
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Write(0, ref this._header);
            }
        }

        #endregion

        #region Property

        public string FullPath
        {
            get
            {
                ThrowIfDisposed();
                return this.Path;
            }
        }

        #endregion

        public void Add(TDataItem item)
        {
            ThrowIfDisposed();

            using (var accessor = Mmf.CreateViewAccessor(0, this._capacity))
            {
                long position =
                    this._headerSize +
                    this._dataItemSize*this._header.DataCount;

                accessor.Write(position, ref item);

                // update header
                this._header.DataCount ++;
                accessor.Write(0, ref this._header);
            }
        }

        public TDataItem Read()
        {
            ThrowIfDisposed();

            TDataItem result;
            using (var accessor = Mmf.CreateViewAccessor(0, this._capacity))
            {
                accessor.Read(this._headerSize, out result);
            }
            return result;
        }

        #region Override

        public override string ToString()
        {
            ThrowIfDisposed();

            return this.Path;
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
                if (Mmf != null)
                {
                    Mmf.Dispose();
                    Mmf = null;
                }
            }

            Disposed = true;
        }

        #endregion
    }
}
