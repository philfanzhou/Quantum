using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;

namespace Quantum.Domain.TimeSeries
{
    /// <summary>
    /// 时间序列数据包裹
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TimeSeriesPackage<T> : ITimeSeriesPackage<T>
        where T : ITimeSeries
    {
        #region Field
        private readonly ITimeZone _zone;

        private List<T> _items = new List<T>();
        #endregion

        #region Constructor
        /// <summary>
        /// 构造时间序列package
        /// </summary>
        /// <param name="zone"></param>
        public TimeSeriesPackage(ITimeZone zone)
        {
            _zone = zone;
        }
        #endregion

        #region Property
        ITimeZone ITimeSeriesPackage<T>.Zone { get { return _zone; } }

        IEnumerable<T> ITimeSeriesPackage<T>.Items { get { return _items; } }
        #endregion

        #region Public Method

        void ITimeSeriesPackage<T>.Add(T item)
        {
            if(!(this as ITimeSeriesPackage<T>).TryAdd(item))
            {
                throw new InvalidOperationException();
            }
        }

        bool ITimeSeriesPackage<T>.TryAdd(T item)
        {
            if (!_zone.ContainsTime(item.Time))
            {
                return false;
            }

            _items.Add(item);
            return true;
        }
        #endregion

        /// <summary>
        /// 已重载
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if(_zone == null)
            {
                return base.ToString();
            }
            else
            {
                return _zone.ToString();
            }
        }
    }
}
