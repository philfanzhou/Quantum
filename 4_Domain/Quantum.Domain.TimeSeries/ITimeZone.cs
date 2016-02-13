using System;

namespace Quantum.Domain.TimeSeries
{
    /// <summary>
    /// 时间序列数据包裹的时间区域
    /// </summary>
    public interface ITimeZone
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        DateTime EndTime { get; }

        /// <summary>
        /// 判断指定时间是否在区域内
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        bool ContainsTime(DateTime time);
    }
}
