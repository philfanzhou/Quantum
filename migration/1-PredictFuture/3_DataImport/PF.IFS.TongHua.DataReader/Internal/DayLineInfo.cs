using System.Collections.Generic;

namespace PF.IFS.TongHua.DataReader
{
    internal class DayLineInfo : IKlineData
    {
        public string Symbol
        {
            get;
            set;
        }

        public List<IKlineItem> Items
        {
            get;
            set;
        }
    }
}