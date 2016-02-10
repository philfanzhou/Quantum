using Ore.Infrastructure.MarketData;
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
        #region Public Method
        /// <summary>
        /// 将数据拆分到数据包裹中
        /// </summary>
        /// <param name="datas"></param>
        public IEnumerable<ITimeSeriesPackage<T>> SplitToPackages(IEnumerable<T> datas)
        {
            IOrderedEnumerable<T> orderedDatas = datas.OrderBy(p => p.Time);
            var packages = new List<ITimeSeriesPackage<T>>();

            foreach (var data in orderedDatas)
            {
                if (packages.Count < 1 ||
                    packages.Last().ContainsTime(data.Time) == false)
                {
                    DateTime startTime;
                    DateTime endTime;
                    GetTimeZone(data.Time, out startTime, out endTime);

                    packages.Add(new TimeSeriesPackage<T>(startTime, endTime));
                }

                packages.Last().Add(data);
            }

            return packages;
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
