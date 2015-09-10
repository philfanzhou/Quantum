using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class MyMemoryMappedFile<TDataItem, TDataHeader> : 
        IMarketDataMmf<TDataItem, TDataHeader>,
        IDisposable
        where TDataItem : struct
        where TDataHeader : struct, IMarketDataMmfHeader
    {
        #region Field

        /// <summary>
        /// 头文件长度
        /// </summary>
        private readonly int _headerSize = Marshal.SizeOf(typeof(TDataHeader));
        /// <summary>
        /// 单个数据长度
        /// </summary>
        private readonly int _dataItemSize = Marshal.SizeOf(typeof(TDataItem));

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

            long capacity = maxDataCount * this._dataItemSize + this._headerSize;
            // FileMode一定要使用CreateNew，否则可能出现覆盖文件的情况
            var mmf = MemoryMappedFile.CreateFromFile(
                path, FileMode.CreateNew, mapName, capacity);
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

        public IMarketDataMmfHeader Header
        {
            get 
            {
                ThrowIfDisposed();
                return this._header;
            }
        }

        private int BufferSize
        {
            get
            {
                return this._dataItemSize * 100;
            }
        }

        #endregion

        public void Add(TDataItem item)
        {
            ThrowIfDisposed();

            long offset = this._headerSize + this._dataItemSize * this._header.DataCount;
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize))
            {                
                accessor.Write(0, ref item);
            }

            // update header
            UpdateDataCount(1);
        }

        public void Delete(int index)
        {
            ThrowIfDisposed();

            // 数据需要移动到的位置
            long destination = this._headerSize + this._dataItemSize * index;
            // 待向前移动数据所在位置
            long position = destination + this._dataItemSize;
            // 待向前移动byte长度
            long length = (this._header.DataCount - index - 1) * this._dataItemSize;
            
            #region 将后面的数据整体移动向前
            byte[] buffer = new byte[this.BufferSize];
            using (var stream = Mmf.CreateViewStream())
            {
                while (length > this.BufferSize)
                {
                    stream.Seek(position, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Seek(destination, SeekOrigin.Begin);
                    stream.Write(buffer, 0, buffer.Length);

                    destination += buffer.Length;
                    position += buffer.Length;
                    length -= buffer.Length;
                }

                if (length > 0)
                {
                    buffer = new byte[length];

                    stream.Seek(position, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Seek(destination, SeekOrigin.Begin);
                    stream.Write(buffer, 0, buffer.Length);

                    destination += buffer.Length;
                }

                // Append a empty array to erase the data at the end of stream.
                buffer = new byte[this._dataItemSize];
                stream.Seek(destination, SeekOrigin.Begin);
                stream.Write(buffer, 0, buffer.Length);
            }
            #endregion

            // update header
            UpdateDataCount(-1);
        }

        public void Update(TDataItem item, int index)
        {
            ThrowIfDisposed();

            long offset = this._headerSize + this._dataItemSize * index;
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize))
            {
                accessor.Write(0, ref item);
            }
        }

        public TDataItem Read(int index)
        {
            ThrowIfDisposed();

            long offset = this._headerSize + this._dataItemSize * index;
            TDataItem result;
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize))
            {
                accessor.Read(0, out result);
            }
            return result;
        }

        public void Insert(TDataItem item, int index)
        {
            ThrowIfDisposed();

            // 数据更新位置偏移量
            long destination = this._headerSize + this._dataItemSize * (this._header.DataCount + 1);
            // 待向前移动数据所在位置
            long position = destination - this._dataItemSize;
            // 待向前移动byte长度
            long length = (this._header.DataCount - index) * this._dataItemSize;

            #region 将数据整体向后移动
            byte[] buffer = new byte[this.BufferSize];
            using (var stream = Mmf.CreateViewStream())
            {
                while (length > this.BufferSize)
                {
                    stream.Seek(position - buffer.Length, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Seek(destination - buffer.Length, SeekOrigin.Begin);
                    stream.Write(buffer, 0, buffer.Length);

                    destination -= buffer.Length;
                    position -= buffer.Length;

                    length -= buffer.Length;
                }

                if (length > 0)
                {
                    buffer = new byte[length];

                    stream.Seek(position - buffer.Length, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Seek(destination - buffer.Length, SeekOrigin.Begin);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            #endregion

            // Add data to position
            long offset = this._headerSize + this._dataItemSize * index;
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize))
            {
                accessor.Write(0, ref item);
            }

            // update header
            UpdateDataCount(1);
        }

        private void UpdateDataCount(int number)
        {
            this._header.DataCount += number;
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Write(0, ref this._header);
            }
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
