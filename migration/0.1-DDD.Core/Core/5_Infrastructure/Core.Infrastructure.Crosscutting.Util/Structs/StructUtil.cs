using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Core.Infrastructure.Crosscutting.Util.Structs
{
    public static class StructUtil<T>
    {
        /// <summary>
        /// 将byte[]还原为指定的struct
        /// 不要在循环体中调用此方法
        /// </summary>
        public static T BytesToStruct(byte[] data)
        {
            if (data == null || data.Length < 0)
            {
                return default(T);
            }

            return BytesToStruct(data, 0, data.Length);
        }

        /// <summary>
        /// 将byte[]还原为指定的struct
        /// </summary>
        public static T BytesToStruct(byte[] data, IntPtr buffer)
        {
            if (data == null || data.Length < 0)
            {
                return default(T);
            }

            return BytesToStruct(data, 0, buffer, data.Length);
        }

        /// <summary>
        /// 将byte[]还原为指定的struct
        /// 不要在循环体中调用此方法
        /// </summary>
        public static T BytesToStruct(byte[] data, int startIndex, int length)
        {
            IntPtr buffer = Marshal.AllocHGlobal(length);
            try
            {
                return BytesToStruct(data, startIndex, buffer, length);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// 将byte[]还原为指定的struct
        /// </summary>
        public static T BytesToStruct(byte[] data, int startIndex, IntPtr buffer, int length)
        {
            if (data == null || data.Length < 0)
            {
                return default(T);
            }

            Marshal.Copy(data, startIndex, buffer, length);
            return (T)Marshal.PtrToStructure(buffer, typeof(T));
        }

        /// <summary>
        /// 读取指定长度的结构体数组
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="arrayLength">数组长度</param>
        /// <returns></returns>
        public static T[] ReadStructArray(BinaryReader reader, uint arrayLength)
        {
            T[] result = new T[arrayLength];
            int structSize = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(structSize);
            try
            {
                for (int i = 0; i < arrayLength; i++)
                {
                    byte[] data = reader.ReadBytes(structSize);
                    result[i] = BytesToStruct(data, buffer);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return result;
        }
    }
}
