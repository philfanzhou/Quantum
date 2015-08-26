using System;

namespace Quantum.Domain.Trading
{
    internal class Market
    {
        private static DateTime _virtualTime = DateTime.MinValue;

        static Market()
        {
            IsVirtual = false;
        }

        /// <summary>
        /// 一手股票包含100股
        /// </summary>
        public const int OneHandStock = 100;

        /// <summary>
        /// 佣金费率
        /// </summary>
        public const decimal CommissionRate = 0.0003m;

        /// <summary>
        /// 印花税率
        /// </summary>
        public const decimal StampDutyRate = 0.001m;

        /// <summary>
        /// 交易费率
        /// </summary>
        public const decimal TransferFeesRate = 0.00002m;

        public static bool IsVirtual { get; set; }

        public static DateTime Time
        {
            get 
            {
                return IsVirtual ? _virtualTime : DateTime.Now;
            }
        }

        public static void SetVirtualMarketTime(DateTime time)
        {
            _virtualTime = time;
        }
    }
}
