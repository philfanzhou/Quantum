using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public class MarketDataMmf<TDataItem, TDataHeader> : 
        MmfBase,
        IMarketDataMmf
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

        protected MarketDataMmf(string path) : base(path)
        {
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Read(0, out this._header);
            }
        }

        /// <summary>
        /// 子类调用，用于创建映射文件
        /// </summary>
        protected MarketDataMmf(string path, int maxDataCount)
            : base
            (path, 
            maxDataCount * Marshal.SizeOf(typeof(TDataItem)) + Marshal.SizeOf(typeof(TDataHeader))
            )
        {
            // 创建文件之后要立即更新头，避免创建之后未加数据就关闭后，下次无法打开文件
            this._header.MaxDataCount = maxDataCount;
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
            Add(new[] { item });
        }

        public void Add(IEnumerable<TDataItem> items)
        {
            Insert(items, this._header.DataCount);
        }

        public void Delete(int index)
        {
            ThrowIfDisposed();
            if (index > this._header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");

            if (index >= this._header.DataCount)
            {
                return;
            }

            // 数据需要移动到的位置
            long destination = this._headerSize + this._dataItemSize * index;
            // 待向前移动数据所在位置
            long position = destination + this._dataItemSize;
            // 待向前移动byte长度
            long length = (this._header.DataCount - index - 1) * this._dataItemSize;

            // 移动数据
            MoveDataPosition(ref destination, ref position, ref length, this._bufferSize);

            // 更新文件头
            UpdateDataCount(-1);

            //Delete(index, 1);
        }

        public void Delete(int index, int count)
        {
            ThrowIfDisposed();
            if (index >= this._header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            if(count >= this._header.MaxDataCount || count < 1)
                throw new ArgumentOutOfRangeException("count");
            if (index + count > this._header.MaxDataCount)
                throw new ArgumentOutOfRangeException("count");

            if (index >= this._header.DataCount)
            {
                return;
            }

            // 待移动数据所在位置(左移)
            long position = 0;
            position += this._headerSize;
            position += this._dataItemSize*(index + count);

            // 数据需要移动到的位置
            long destination = position;
            destination -= this._dataItemSize*count;

            // 需要移动的byte长度
            long length = this._dataItemSize*this._header.DataCount;
            length -= position;

            // 移动数据
            MoveDataPosition(ref destination, ref position, ref length, this._bufferSize);

            // 更新文件头
            UpdateDataCount(-count);
        }

        public void Update(TDataItem item, int index)
        {
            ThrowIfDisposed();
            if (index > this._header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");

            long offset = this._headerSize + this._dataItemSize * index;
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize))
            {
                accessor.Write(0, ref item);
            }
        }

        public TDataItem Read(int index)
        {
            ThrowIfDisposed();
            if (index > this._header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");

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
            Insert(new[] { item }, index);
        }

        public void Insert(IEnumerable<TDataItem> items, int index)
        {
            ThrowIfDisposed();
            if (null == items)
                throw new ArgumentNullException("items");
            if (index > this._header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            var array = items.ToArray();
            if (array.Length + this._header.DataCount > this._header.MaxDataCount)
                throw new ArgumentOutOfRangeException("items");

            if (index == this._header.DataCount)
            {
                InsertDataToPosition(array, index);
                return;
            }

            // 待移动数据所在位置(右移：从当前有效数据的末尾开始移动)
            long position = 0;
            position += this._headerSize;
            position += this._dataItemSize * this._header.DataCount;

            // 数据需要移动到的位置
            long destination = position;
            destination += this._dataItemSize * array.Length;
            
            // 需要移动的byte长度
            long length = position;
            length -= this._dataItemSize*index;

            // 移动数据
            MoveDataPosition(ref destination, ref position, ref length, this._bufferSize);

            // 插入数据
            InsertDataToPosition(array, index);
        }

        #region Private Method

        private void InsertDataToPosition(TDataItem[] items, int index)
        {
            long offset = this._headerSize + this._dataItemSize * index;
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize * items.Length))
            {
                accessor.WriteArray(0, items, 0, items.Length);
            }

            // update header
            UpdateDataCount(items.Length);
        }

        private void UpdateDataCount(int number)
        {
            this._header.DataCount += number;
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Write(0, ref this._header);
            }
        }

        #endregion
    }
}
