using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;

namespace Quantum.Domain.TimeSeries
{
    /// <summary>
    /// 时间序列数据包裹定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITimeSeriesPackage<T>
        where T : ITimeSeries
    {
        /// <summary>
        /// 获取package的时间区域
        /// </summary>
        ITimeZone Zone { get; }

        /// <summary>
        /// 获取package内的所有Item
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// 将数据添加到package中
        /// </summary>
        /// <exception cref="System.InvalidOperationException">如果当前package无法将数据加入，则抛出异常</exception>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// 将数据添加到package中
        /// </summary>
        /// <param name="item"></param>
        /// <returns>添加成功返回true</returns>
        bool TryAdd(T item);
    }
}
