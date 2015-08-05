using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    internal static class StructUtil<T>
    {
        /// <summary>
        /// 将byte[]还原为指定的struct
        /// 不要在循环体中调用此方法
        /// </summary>
        internal static T BytesToStruct(byte[] data)
        {
            return BytesToStruct(data, 0, data.Length);
        }

        /// <summary>
        /// 将byte[]还原为指定的struct
        /// </summary>
        internal static T BytesToStruct(byte[] data, IntPtr buffer)
        {
            return BytesToStruct(data, 0, buffer, data.Length);
        }

        /// <summary>
        /// 将byte[]还原为指定的struct
        /// 不要在循环体中调用此方法
        /// </summary>
        internal static T BytesToStruct(byte[] data, int startIndex, int length)
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
        internal static T BytesToStruct(byte[] data, int startIndex, IntPtr buffer, int length)
        {
            Marshal.Copy(data, startIndex, buffer, length);
            return (T)Marshal.PtrToStructure(buffer, typeof(T));
        }

        /// <summary>
        /// 读取指定长度的结构体数组
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="arrayLength">数组长度</param>
        /// <returns></returns>
        internal static T[] ReadStructArray(BinaryReader reader, uint arrayLength)
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
