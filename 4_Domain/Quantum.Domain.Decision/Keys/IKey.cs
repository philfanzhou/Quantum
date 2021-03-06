﻿using Ore.Infrastructure.MarketData;
using System;
using System.Runtime.Serialization;

namespace Quantum.Domain.Decision.Keys
{
    public interface IKey : ISerializable
    {
        /// <summary>
        /// 获取Key所需要的数据类型
        /// </summary>
        KLineType DataType { get; }

        /// <summary>
        /// 获取Key的类型
        /// </summary>
        KeyType KeyType { get; }

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
        
        /// <summary>
        /// 根据现有数据，判断当前的Key是否Match
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        bool Match(Link link);
    }
}
