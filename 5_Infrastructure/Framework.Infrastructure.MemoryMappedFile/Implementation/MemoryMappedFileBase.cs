using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Framework.Infrastructure.MemoryMappedFile
{
    public class MemoryMappedFileBase<TDataHeader, TDataItem> : 
        MyMemoryMappedFile,
        IMemoryMappedFile<TDataHeader, TDataItem>
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

        private readonly int _bufferSize = Marshal.SizeOf(typeof(TDataItem)) * 100; //Todo: 需要更好的方案来确定缓存长度

        private TDataHeader _header;

        #endregion

        #region Constructor

        protected MemoryMappedFileBase() { }

        /// <summary>
        /// 打开文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        private MemoryMappedFileBase(string path) : base(path)
        {
            ReadHeader();
        }

        /// <summary>
        /// 创建文件调用的构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="maxDataCount"></param>
        private MemoryMappedFileBase(string path, TDataHeader fileHeader)
            : base(path, CaculateCapacity(fileHeader))
        {
            if (fileHeader.MaxDataCount <= 0)
                throw new ArgumentOutOfRangeException("fileHeader");

            // 创建文件之后要立即更新头，避免创建之后未加数据就关闭后，下次无法打开文件
            fileHeader.DataCount = 0;
            this._header = fileHeader;
            WriteHeader();
        }

        private static long CaculateCapacity(TDataHeader fileHeader)
        {
            return fileHeader.MaxDataCount * Marshal.SizeOf(typeof(TDataItem)) + Marshal.SizeOf(typeof(TDataHeader));
        }

        public static IMemoryMappedFile<TDataHeader, TDataItem> Open(string path)
        {
            return new MemoryMappedFileBase<TDataHeader, TDataItem>(path);
        }

        public static IMemoryMappedFile<TDataHeader, TDataItem> Create(string path, TDataHeader fileHeader)
        {
            return new MemoryMappedFileBase<TDataHeader, TDataItem>(path, fileHeader);
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

        #region IMemoryMappedFile Members

        public virtual void Add(TDataItem item)
        {
            Add(new[] { item });
        }

        public virtual void Add(IEnumerable<TDataItem> items)
        {
            Insert(items, this._header.DataCount);
        }

        public virtual void Delete(int index)
        {
            Delete(index, 1);
        }

        public virtual void Delete(int index, int count)
        {
            DoDelete(index, count);
        }

        public virtual void DeleteAll()
        {
            Delete(0, this._header.DataCount);
        }

        public virtual void Update(TDataItem item, int index)
        {
            Update(new[] { item }, index);
        }

        public virtual void Update(IEnumerable<TDataItem> items, int index)
        {
            DoInsert(items, index, false);
        }

        public virtual TDataItem Read(int index)
        {
            return Read(index, 1).FirstOrDefault();
        }

        public virtual IEnumerable<TDataItem> Read(int index, int count)
        {
            return DoRead(index, count);
        }

        public virtual IEnumerable<TDataItem> ReadAll()
        {
            return Read(0, this._header.DataCount);
        }

        public virtual void Insert(TDataItem item, int index)
        {
            Insert(new[] { item }, index);
        }

        public virtual void Insert(IEnumerable<TDataItem> items, int index)
        {
            DoInsert(items, index, true);
        }

        #endregion

        #region Private Method

        private IEnumerable<TDataItem> DoRead(int index, int count)
        {
            ThrowIfDisposed();
            if (index > this._header.DataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (count < 0 || index + count > this._header.DataCount)
                throw new ArgumentOutOfRangeException("count");

            if(count == 0)
            {
                return new TDataItem[0];
            }

            long offset = this._headerSize + this._dataItemSize * index;
            TDataItem[] result = new TDataItem[count];
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize * count))
            {
                accessor.ReadArray(0, result, 0, result.Length);
            }
            return result;
        }

        private void DoDelete(int index, int count)
        {
            ThrowIfDisposed();
            if (index >= this._header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (count > this._header.MaxDataCount || count < 1)
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

        private void DoInsert(IEnumerable<TDataItem> items, int index, bool ChangeDataCount)
        {
            ThrowIfDisposed();
            if (null == items)
                throw new ArgumentNullException("items");
            if (index > this._header.MaxDataCount || index < 0)
                throw new ArgumentOutOfRangeException("index");
            var array = items.ToArray();
            if (array.Length + index > this._header.MaxDataCount)
                throw new ArgumentOutOfRangeException("items");

            if (ChangeDataCount)
            {
                if (array.Length + this._header.DataCount > this._header.MaxDataCount)
                    throw new ArgumentOutOfRangeException("items");
            }

            if (ChangeDataCount &&
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
            long offset = this._headerSize + this._dataItemSize * index;
            using (var accessor = Mmf.CreateViewAccessor(offset, this._dataItemSize * array.Length))
            {
                accessor.WriteArray(0, array, 0, array.Length);
            }

            // 更新文件头
            if (ChangeDataCount)
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

        private void UpdateDataCount(int number)
        {
            this._header.DataCount += number;
            WriteHeader();
        }

        private void ReadHeader()
        {
            // 针对头文件进行特殊读写处理，因为可能在头文件中含有String等托管类型的数据
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                byte[] array = new byte[this._headerSize];
                accessor.ReadArray(0, array, 0, array.Length);
                this._header = BytesToStruct<TDataHeader>(array);
            }
        }

        private void WriteHeader()
        {
            // 针对头文件进行特殊读写处理，因为可能在头文件中含有String等托管类型的数据
            using (var accessor = Mmf.CreateViewAccessor(0, this._headerSize))
            {
                byte[] array = StructToBytes(this._header);
                accessor.WriteArray(0, array, 0, array.Length);
            }
        }

        #endregion
    }
}
