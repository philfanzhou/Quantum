using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Decision.Keys
{
    public interface IKey
    {
        KLineType DataType { get; }

        /// <summary>
        /// 指示当前Key是作为买判断还是卖判断
        /// </summary>
        ActionType Type { get; }

        /// <summary>
        /// 根据指定的时间，和当前依赖的数据类型，
        /// 以及需要判断的指标，计算出所需要的数据从什么时间开始。
        /// </summary>
        /// <example>
        /// 需要日线数据，并且需要计算MA5，至少需要当前时间之前5天的数据。
        /// 返回的结果就是<paramref name="time"/> - 5 天
        /// </example>
        /// <param name="time"></param>
        /// <returns></returns>
        DateTime GetDataStartTime(DateTime time);

        // bool Match();

        
    }


}
