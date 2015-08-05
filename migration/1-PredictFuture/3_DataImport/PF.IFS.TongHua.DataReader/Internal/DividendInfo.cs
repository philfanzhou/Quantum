using System.Collections.Generic;

namespace PF.IFS.TongHua.DataReader
{
    internal class DividendInfo : IDividendData
    {
        public string Symbol
        {
            get;
            set;
        }

        public List<IDividendItem> Items
        {
            get;
            set;
        }
    }
}