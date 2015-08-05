using System;
using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct THDateTimeStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private readonly byte[] data;

        internal DateTime Value
        {
            get { return ByteToDateTime(data); }
        }

        private static DateTime ByteToDateTime(byte[] data)
        {
            DateTime dt;

            int intValue = BitConverter.ToInt32(data, 0);
            dt = intValue > 10000 ? IntToDateTime(intValue) : DateTime.MinValue;

            return dt;
        }

        private static DateTime IntToDateTime(int value)
        {
            return new DateTime(value / 10000, (value % 10000) / 100, value % 100);
        }
    }
}
