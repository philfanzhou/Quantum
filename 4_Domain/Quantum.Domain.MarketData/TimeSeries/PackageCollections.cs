﻿using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 时间序列数据包裹的集合类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PackageCollections<T> where T : ITimeSeries
    {
        #region Field
        private readonly List<ITimeSeriesPackage<T>> _packages
            = new List<ITimeSeriesPackage<T>>();
        #endregion

        #region Property
        /// <summary>
        /// 获取所有数据包裹
        /// </summary>
        public IEnumerable<ITimeSeriesPackage<T>> Packages { get { return _packages; } }
        #endregion

        #region Public Method
        /// <summary>
        /// 将数据拆分到数据包裹中
        /// </summary>
        /// <param name="datas"></param>
        public void SplitToPackages(IEnumerable<T> datas)
        {
            IOrderedEnumerable<T> orderedDatas = datas.OrderBy(p => p.Time);

            foreach (var data in orderedDatas)
            {
                if (_packages.Count < 1 ||
                    _packages.Last().ContainsTime(data.Time) == false)
                {
                    DateTime startTime;
                    DateTime endTime;
                    GetTimeZone(data.Time, out startTime, out endTime);

                    _packages.Add(new TimeSeriesPackage<T>(startTime, endTime));
                }

                _packages.Last().Add(data);
            }
        }
        #endregion

        #region Abstract Method
        /// <summary>
        /// 获取当前时间应该处于的时间区域
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        protected abstract void GetTimeZone(
            DateTime currentTime, out DateTime startTime, out DateTime endTime);
        #endregion
    }
}
