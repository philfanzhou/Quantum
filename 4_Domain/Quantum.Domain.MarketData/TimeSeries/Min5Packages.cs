using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 以5分钟作为时间片的数据包裹
    /// </summary>
    public class Min5Packages<T> : PackageCollections<T>
        where T : ITimeSeries
    {
        protected override DateTime GetStartTime(DateTime time)
        {
            throw new NotImplementedException();
        }
        protected override DateTime GetEndTime(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
