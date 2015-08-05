using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    /// <summary>
    /// 同花顺的个股数据
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class THKLineStock : THKLine
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        protected byte[] w1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        protected byte[] w2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        protected byte[] w3;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        protected byte[] w4;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        protected byte[] w5;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 120)]
        protected byte[] otherData;

        internal double W1
        {
            get { return GetValue(w1); }
        }

        internal double W2
        {
            get { return GetValue(w2); }
        }

        internal double W3
        {
            get { return GetValue(w3); }
        }

        internal double W4
        {
            get { return GetValue(w4); }
        }

        internal double W5
        {
            get { return GetValue(w5); }
        }
    }
}
