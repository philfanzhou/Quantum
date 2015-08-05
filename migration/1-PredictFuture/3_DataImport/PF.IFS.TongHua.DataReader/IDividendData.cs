using System.Collections.Generic;

namespace PF.IFS.TongHua.DataReader
{
    public interface IDividendData
    {
        string Symbol { get; }

        List<IDividendItem> Items { get; }
    }
}