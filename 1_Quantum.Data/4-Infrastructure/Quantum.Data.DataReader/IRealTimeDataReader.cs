using Quantum.Data.Metadata;
using System.Collections.Generic;

namespace Quantum.Data.DataReader
{
    /// <summary>
    /// 实时行情数据读取器接口定义
    /// </summary>
    public interface IRealTimeDataReader
    {
        IRealTimeData GetData(string code);

        IEnumerable<IRealTimeData> GetData(IEnumerable<string> codes);
    }
}
