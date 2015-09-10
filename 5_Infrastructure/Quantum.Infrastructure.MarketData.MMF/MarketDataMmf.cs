using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class MarketDataMmf<TDataItem, TDataHeader> : 
        MmfBase,
        IMarketDataMmf<TDataItem, TDataHeader>
        where TDataItem : struct
        where TDataHeader : struct, IMarketDataHeader
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

        private readonly int _bufferSize = Marshal.SizeOf(typeof(TDataItem)) * 100;

        private TDataHeader _header;

        #endregion

        #region Constructor

        public MarketDataMmf(string path, string mapName) : base(path, mapName)
        {
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Read(0, out this._header);
            }
        }

        /// <summary>
        /// 子类调用，用于创建映射文件
        /// </summary>
        protected MarketDataMmf(string path, string mapName, int maxDataCount)
            : base
            (path, 
            mapName, 
            maxDataCount * Marshal.SizeOf(typeof(TDataItem)) + Marshal.SizeOf(typeof(TDataHeader))
            )
        {
            this._header.MaxDataCount = maxDataCount;

            // 创建文件之后要立即更新头，避免创建之后未加数据就关闭后，下次无法打开文件
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Write(0, ref this._header);
            }
        }

        #endregion

        #region Property

        public IMarketDataHeader Header
        {
            get 
            {
                ThrowIfDisposed();
                return this._header;
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
            byte[] buffer = new byte[this._bufferSize];
            using (var stream = Mmf.CreateViewStream())
            {
                while (length > this._bufferSize)
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
            byte[] buffer = new byte[this._bufferSize];
            using (var stream = Mmf.CreateViewStream())
            {
                while (length > this._bufferSize)
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
    }
}
