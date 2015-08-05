using System;
using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    [StructLayout(LayoutKind.Sequential)]
    internal class THIndexRecord
    {
        #region Field

        [MarshalAs(UnmanagedType.I1)]
        private readonly byte marker;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        private readonly byte[] symbol;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly byte[] freeNumber;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private readonly byte[] position;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private readonly byte[] recordNumber;

        #endregion

        #region Property

        /// <summary>
        /// 市场
        /// </summary>
        internal MarketType Market
        {
            get { return (MarketType)marker; }
        }

        /// <summary>
        /// 挂牌代码
        /// </summary>
        internal string Symbol
        {
            get { return StringUtil.ReadString(symbol); }
        }

        /// <summary>
        /// 空闲记录数
        /// </summary>
        internal UInt16 FreeNumber
        {
            get { return BitConverter.ToUInt16(freeNumber, 0); }
        }

        /// <summary>
        /// 记录开始下标
        /// </summary>
        internal UInt32 Position
        {
            get { return BitConverter.ToUInt32(position, 0); }
        }

        /// <summary>
        /// 隶属记录数
        /// </summary>
        internal UInt16 RecordNumber
        {
            get { return BitConverter.ToUInt16(recordNumber, 0); }
        }

        #endregion
    }

    internal enum MarketType : byte
    {
        B = 0x4a,
        H = 80,
        S = 0x10
    }
}
