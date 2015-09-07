
namespace Quantum.Domain.MarketData
{
    using System;

    interface IMarketData
    {
        /// <summary>
        /// 时间
        /// </summary>
        DateTime Time { get; set; }
    }
}
