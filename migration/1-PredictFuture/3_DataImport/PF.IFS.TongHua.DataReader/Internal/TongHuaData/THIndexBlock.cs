using System;
using System.IO;

namespace PF.IFS.TongHua.DataReader
{
    internal class THIndexBlock
    {
        #region Field

        private UInt16 _blockSize;

        private UInt16 _indexCount;

        private THIndexRecord[] _recordList;

        #endregion

        #region Property

        internal UInt16 BlockSize
        {
            get { return _blockSize; }
        }

        internal UInt16 IndexCount
        {
            get { return _indexCount; }
        }

        internal THIndexRecord[] RecordList
        {
            get { return _recordList; }
        }

        #endregion

        internal static THIndexBlock Read(BinaryReader reader)
        {
            THIndexBlock block = new THIndexBlock();
            block._blockSize = reader.ReadUInt16();
            block._indexCount = reader.ReadUInt16();
            block._recordList = StructUtil<THIndexRecord>.ReadStructArray(reader, block.IndexCount);

            return block;
        }
    }
}
