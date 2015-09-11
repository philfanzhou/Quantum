using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Framework.Infrastructure.MemoryMappedFile
{
    public class MemoryMappedFileBase<TDataHeader, TDataItem> : 
        MyMemoryMappedFile,
        IMemoryMappedFile
        where TDataHeader : struct, IMemoryMappedFileHeader
        where TDataItem : struct
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

        protected MemoryMappedFileBase() { }

        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        protected MemoryMappedFileBase(string path) : base(path)
        {
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Read(0, out this._header);
            }
        }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        protected MemoryMappedFileBase(string path, int maxDataCount)
            : base(path, CaculateCapacity(maxDataCount))
        {
            // 创建文件之后要立即更新头，避免创建之后未加数据就关闭后，下次无法打开文件
            this._header.MaxDataCount = maxDataCount;
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                accessor.Write(0, ref this._header);
            }
        }

        private static long CaculateCapacity(int maxDataCount)
        {
            return maxDataCount * Marshal.SizeOf(typeof(TDataItem)) + Marshal.SizeOf(typeof(TDataHeader));
        }

        public static MemoryMappedFileBase<TDataHeader, TDataItem> Open(string path)
        {
            return new MemoryMappedFileBase<TDataHeader, TDataItem>(path);
        }

        public static MemoryMappedFileBase<TDataHeader, TDataItem> Create(string path, int maxDataCount)
        {
            return new MemoryMappedFileBase<TDataHeader, TDataItem>(path, maxDataCount);
        }

        #endregion

        #region Property

        public IMemoryMappedFileHeader Header
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
            Delete(index, 1);
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

            if (index + count < this._header.DataCount)
            {
                // 待移动数据所在位置(左移)
                long position = 0;
                position += this._headerSize;
                position += this._dataItemSize * (index + count);
                // 数据需要移动到的位置(左移)
                long destination = position;
                destination -= this._dataItemSize * count;
                // 待向前移动byte长度
                long length = (this._header.DataCount - (index + count)) * this._dataItemSize;
                // 移动数据
                MoveDataPosition(ref destination, ref position, ref length, this._bufferSize);
            }

            // 更新文件头
            UpdateDataCount(-count);
        }

        public void Update(TDataItem item, int index)
        {
            Update(new[] { item }, index);
        }

        public void Update(IEnumerable<TDataItem> items, int index)
        {
            Insert(items, index, false);
        }

        public TDataItem Read(int index)
        {
            return Read(index, 1).First();
        }

        public IEnumerable<TDataItem> Read(int index, int count)
        {
            ThrowIfDisposed();
            if (index > this._header.DataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (count < 1 || index + count > this._header.DataCount)
                throw new ArgumentOutOfRangeException("count");

            long offset = this._headerSize + this._dataItemSize * index;
            TDataItem[] result = new TDataItem[count];
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize*count))
            {
                accessor.ReadArray(0, result, 0, result.Length);
            }
            return result;
        }

        public IEnumerable<TDataItem> ReadAll()
        {
            return Read(0, this._header.DataCount);
        }

        public void Insert(TDataItem item, int index)
        {
            Insert(new[] { item }, index);
        }

        public void Insert(IEnumerable<TDataItem> items, int index)
        {
            Insert(items, index, true);
        }

        #region Private Method

        private void Insert(IEnumerable<TDataItem> items, int index, bool needMoveData)
        {
            ThrowIfDisposed();
            if (null == items)
                throw new ArgumentNullException("items");
            if (index > this._header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            var array = items.ToArray();
            if (array.Length + index > this._header.MaxDataCount)
                throw new ArgumentOutOfRangeException("items");

            if (needMoveData)
            {
                if (array.Length + this._header.DataCount > this._header.MaxDataCount)
                    throw new ArgumentOutOfRangeException("items");
            }

            if (needMoveData &&
                index < this._header.DataCount)
            {
                // 待移动数据所在位置(右移：从当前有效数据的末尾开始移动)
                long position = 0;
                position += this._headerSize;
                position += this._dataItemSize*this._header.DataCount;
                // 数据需要移动到的位置
                long destination = position;
                destination += this._dataItemSize*array.Length;
                // 需要移动的byte长度
                long length = position;
                length -= this._dataItemSize*index;
                // 移动数据
                MoveDataPosition(ref destination, ref position, ref length, this._bufferSize);
            }

            // 插入数据
            InsertDataToPosition(array, index);

            if (needMoveData)
            {
                if (index > this._header.DataCount)
                {
                    // 如果是在当前已有数据之后的位置插入，更新已有数据数量就需要特殊处理
                    // 等于是中间加入了空白数据
                    UpdateDataCount(index + array.Length - 1);
                }
                else
                {
                    UpdateDataCount(array.Length);
                }
            }
        }

        private void InsertDataToPosition(TDataItem[] array, int index)
        {
            long offset = this._headerSize + this._dataItemSize * index;
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize * array.Length))
            {
                accessor.WriteArray(0, array, 0, array.Length);
            }
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
