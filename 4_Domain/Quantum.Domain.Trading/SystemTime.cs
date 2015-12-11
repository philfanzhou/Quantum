using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 交易系统时间
    /// </summary>
    public class SystemTime
    {
        private static DateTime _virtualTime = DateTime.MinValue;

        /// <summary>
        /// 获取或设置一个值，指示当前系统是否使用虚拟的时间
        /// </summary>
        public static bool IsVirtual { get; set; }

        /// <summary>
        /// 获取交易系统的当前时间
        /// </summary>
        public static DateTime Now
        {
            get
            {
                return IsVirtual ? _virtualTime : DateTime.Now;
            }
        }

        /// <summary>
        /// 设置交易系统的时间,所设置的时间必须大于当前的虚拟时间
        /// </summary>
        /// <param name="time"></param>
        public static void SetVirtualMarketTime(DateTime time)
        {
            if (time > _virtualTime)
            {
                _virtualTime = time;
            }
        }
    }
}
