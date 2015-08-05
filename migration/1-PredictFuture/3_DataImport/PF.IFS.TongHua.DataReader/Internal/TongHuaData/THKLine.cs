using System;
using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    /// <summary>
    /// 同花顺日线数据
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class THKLine : IKlineItem
    {
        #region Field

        [MarshalAs(UnmanagedType.Struct)]
        public THDateTimeStruct date;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] open;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] high;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] low;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] close;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] amount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] volume;

        #endregion

        #region Property

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        {
            get { return this.date.Value; }
        }

        /// <summary>
        /// 开盘价
        /// </summary>
        public double Open
        {
            get { return GetValue(this.open); }
        }

        /// <summary>
        /// 最高价
        /// </summary>
        public double High
        {
            get { return GetValue(this.high); }
        }

        /// <summary>
        /// 最低价
        /// </summary>
        public double Low
        {
            get { return GetValue(this.low); }
        }

        /// <summary>
        /// 收盘价
        /// </summary>
        public double Close
        {
            get { return GetValue(this.close); }
        }

        /// <summary>
        /// 成交金额
        /// </summary>
        public double Amount
        {
            get { return GetValue(this.amount); }
        }

        /// <summary>
        /// 成交量
        /// </summary>
        public double Volume
        {
            get { return GetValue(this.volume); }
        }

        /// <summary>
        /// 换手率
        /// </summary>
        public double Turnover
        {
            get { return 0; }
        }

        #endregion

        internal static double GetValue(byte[] byteArray)
        {
            uint data = BitConverter.ToUInt32(byteArray, 0);

            /*
             * 后28位为数据
             * 1011 0000 0000 0001 0111 0111 0011 0010
             * 用0XFFFFFFF相与取数据
             * 0000 1111 1111 1111 1111 1111 1111 1111
             */
            double value = data & 0xFFFFFFF;

            // 数据右移28位，取得最高4位
            // 描述结果的小数点的数据
            byte pointData = (byte)(data >> 0x1C);

            // pointData中的后三位，表示小数点需要移动的位数
            byte movePointCount = (byte)(pointData & 7);
            if (movePointCount != 0)
            {
                // pointData中的第4位，表示小数点的移动方向
                // 非0左移小数点
                bool leftMovePoint = (pointData & 8) != 0;
                if (leftMovePoint)
                {
                    value = value / Math.Pow(10.0, movePointCount);
                }
                else
                {
                    value = value * Math.Pow(10.0, movePointCount);
                }
            }

            return value;
        }
    }
}
