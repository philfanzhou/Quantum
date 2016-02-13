using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.TimeSeries
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
                    packages.Last().Zone.ContainsTime(data.Time) == false)
                {
                    ITimeZone zone = GetTimeZone(data.Time);
                    packages.Add(new TimeSeriesPackage<T>(zone));
                }

                packages.Last().Add(data);
            }

            return packages;
        }
        #endregion

        #region Abstract Method
        /// <summary>
        /// 根据指定的时间和当前Package的TimeZone大小，获取一个TimeZone
        /// </summary>
        /// <param name="currentTime">指定的时间</param>
        /// <returns></returns>
        protected abstract ITimeZone GetTimeZone(DateTime currentTime);
        #endregion

        #region Protected Method
        /// <summary>
        /// 求num在n位上的数字,取个位,取十位 
        /// </summary>
        /// <param name="num">正整数</param>
        /// <param name="n">所求数字位置(个位 1,十位 2 依此类推)</param>
        /// <returns></returns>
        protected int FindNum(int num, int n)
        {
            int power = (int)Math.Pow(10, n);
            return (num - (num / power) * power) * 10 / power;
        }

        /// <summary>
        /// 获取指定时间区间包含的所有时间区域
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        protected IEnumerable<ITimeZone> GetTimeZone(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                throw new ArgumentOutOfRangeException();
            }

            List<ITimeZone> result = new List<ITimeZone>();
            do
            {
                ITimeZone zone = GetTimeZone(startTime);
                result.Add(zone);
                startTime = zone.EndTime;
            } while (startTime <= endTime);

            return result;
        }
        #endregion
    }
}
