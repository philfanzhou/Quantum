using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;

namespace Quantum.Domain.MarketData
{
    /// <summary>
    /// 时间序列数据包裹
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TimeSeriesPackage<T> : ITimeSeriesPackage<T>
        where T : ITimeSeries
    {
        #region Field
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;

        private List<T> _items = new List<T>();
        #endregion

        #region Constructor
        /// <summary>
        /// 构造时间序列package
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public TimeSeriesPackage(DateTime startTime, DateTime endTime)
        {
            if(startTime > endTime)
            {
                throw new ArgumentOutOfRangeException();
            }

            _startTime = startTime;
            _endTime = endTime;
        }
        #endregion

        #region Property
        DateTime ITimeSeriesPackage<T>.StartTime { get { return _startTime; } }

        DateTime ITimeSeriesPackage<T>.EndTime { get { return _endTime; } }

        IEnumerable<T> ITimeSeriesPackage<T>.Items { get { return _items; } }
        #endregion

        #region Public Method
        bool ITimeSeriesPackage<T>.ContainsTime(DateTime time)
        {
            return (_startTime < time) && (time <= _endTime);
        }

        void ITimeSeriesPackage<T>.Add(T item)
        {
            if(!(this as ITimeSeriesPackage<T>).TryAdd(item))
            {
                throw new InvalidOperationException();
            }
        }

        bool ITimeSeriesPackage<T>.TryAdd(T item)
        {
            if (!(this as ITimeSeriesPackage<T>).ContainsTime(item.Time))
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
            string format = "yyyy/MM/dd hh:mm:ss";
            return string.Format("{0} -- {1}", _startTime.ToString(format), _endTime.ToString(format));
        }
    }
}
