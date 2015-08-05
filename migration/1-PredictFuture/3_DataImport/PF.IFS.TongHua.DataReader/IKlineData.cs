using System.Collections.Generic;

namespace PF.IFS.TongHua.DataReader
{
    public interface IKlineData
    {
        string Symbol { get; }

        List<IKlineItem> Items { get; } 
    }
}