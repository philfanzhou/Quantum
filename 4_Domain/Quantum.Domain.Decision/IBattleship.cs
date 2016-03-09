using Ore.Infrastructure.MarketData;
using Quantum.Domain.Decision.Keys;
using System.Collections.Generic;

namespace Quantum.Domain.Decision
{
    /// <summary>
    /// Nebuchadnezzar号飞船
    /// </summary>
    public interface IBattleship
    {
        /// <summary>
        /// 根据Neo对数据的要求，给他分配对应的接线员
        /// </summary>
        /// <param name="neo"></param>
        /// <returns></returns>
        Link GetLink(Neo neo);
    }
}
