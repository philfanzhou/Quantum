using System.Runtime.InteropServices;

namespace PF.IFS.TongHua.DataReader
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct THColumnHeader
    {
        #region Field

        [FieldOffset(0)]
        private readonly byte w1;

        [FieldOffset(1)]
        private readonly byte type;

        [FieldOffset(2)]
        private readonly byte w2;

        [FieldOffset(3)]
        private readonly byte size;

        #endregion
    }
}
