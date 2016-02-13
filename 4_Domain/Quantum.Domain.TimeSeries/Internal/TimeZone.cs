using System;

namespace Quantum.Domain.TimeSeries
{
    internal class TimeZone : ITimeZone
    {
        public TimeZone(DateTime startTime, DateTime endTime)
        {
            if(endTime < startTime)
            {
                throw new ArgumentOutOfRangeException();
            }

            EndTime = endTime;
            StartTime = startTime;
        }

        public DateTime EndTime { get; private set; }

        public DateTime StartTime { get; private set; }

        public bool ContainsTime(DateTime time)
        {
            return (StartTime < time) && (time <= EndTime);
        }

        /// <summary>
        /// 已重载
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string format = "yyyy/MM/dd hh:mm:ss";
            return string.Format("{0} -- {1}", StartTime.ToString(format), EndTime.ToString(format));
        }
    }
}
