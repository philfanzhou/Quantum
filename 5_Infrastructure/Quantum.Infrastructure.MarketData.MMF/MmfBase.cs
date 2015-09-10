﻿using System;
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

        /// <summary>
        /// 在内存映射文件中整体移动一定长度的byte
        /// </summary>
        /// <param name="destination">数据需要移动到的位置</param>
        /// <param name="position">待移动数据所在位置</param>
        /// <param name="length">待移动数据的长度</param>
        /// <param name="bufferSize">移动数据过程中的Buffer长度</param>
        protected void MoveDataPosition(ref long destination, ref long position, ref long length, int bufferSize)
        {
            // 判断移动方向
            bool leftMove = destination < position;

            byte[] buffer = new byte[bufferSize];
            using (var stream = Mmf.CreateViewStream())
            {
                while (length > 0)
                {
                    if (length < bufferSize)
                    {
                        buffer = new byte[length];
                    }

                    if (!leftMove)
                    {
                        destination -= buffer.Length;
                        position -= buffer.Length;
                    }

                    stream.Seek(position, SeekOrigin.Begin);
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Seek(destination, SeekOrigin.Begin);
                    stream.Write(buffer, 0, buffer.Length);

                    if (leftMove)
                    {
                        destination += buffer.Length;
                        position += buffer.Length;
                    }

                    length -= buffer.Length;
                }

                if (leftMove)
                {
                    // 如果是左移数据，需要抹除最后面的一段位置的废弃数据
                    buffer = new byte[position - destination];
                    stream.Seek(destination, SeekOrigin.Begin);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
