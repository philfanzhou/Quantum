using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.MarketData.MMF
{
    public interface IMarketDataMemoryMappedFile
    {
        /// <summary>
        /// 包含的数据总数
        /// </summary>
        int MaxDataCount { get; }
    }
}
