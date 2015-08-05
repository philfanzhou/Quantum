using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    /// <summary>
    /// 同花顺的板块K线数据
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class THKLineMarket : THKLine
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 136)]
        protected byte[] otherData;
    }
}
