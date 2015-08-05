using System;
using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct THFileHeader
    {
        /// <summary>
        /// 本结构的字节大小
        /// </summary>
        internal static readonly int StructSize = Marshal.SizeOf(typeof(THFileHeader));

        #region Field

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private readonly byte[] sign;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly byte[] w1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private readonly byte[] recordCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly byte[] headerLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly byte[] recordLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly byte[] fieldCount;

        #endregion

        #region Property

        internal uint RecordCount
        {
            get
            {
                uint value = BitConverter.ToUInt32(recordCount, 0) & 0xFFFFFF;
                return value;
            }
        }

        internal ushort RecordLength
        {
            get { return BitConverter.ToUInt16(recordLength, 0); }
        }

        internal ushort FieldCount
        {
            get { return BitConverter.ToUInt16(fieldCount, 0); }
        }

        #endregion
    }
}
